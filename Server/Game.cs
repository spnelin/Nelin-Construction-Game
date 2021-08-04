using BuildingWebsite.Server.ClientObjects;
using BuildingWebsite.Server.ServerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server
{
    public partial class Game
    {
        private object Lock = new object();

        public static Dictionary<int, Game> Games { get; private set; }

        private static int NextGameId;

        private Dictionary<User, GameView> PlayerGameViews;

        private Dictionary<User, Player> Players;

        private Dictionary<Player, ModalType> ExpectedActions;

        private int NextPlayerId;

        private int NextChatId;

        private readonly int PublicChatId;

        public List<Player> PlayerList => Players.Values.ToList();

        public delegate void AddPlayerHandler(Player player);

        public delegate void RemovePlayerHandler(Player player);

        public event AddPlayerHandler AddPlayer;

        public event RemovePlayerHandler RemovePlayer;

        private List<User> Observers;

        private User Owner;

        private GameView ObserverGameView;

        public BidSession BidSession { get; private set; }

        public int Id { get; private set; }

        public GameState CurrentState { get; private set; }

        public GameSettings Settings { get; private set; }

        public Log GameLog { get; private set; }

        public ChatList ChatList { get; private set; }

        public ProjectPipeline ProjectPipeline { get; private set; }

        public Deck<Executive> ExecutiveDeck { get; private set; }

        public Deck<Opportunity> OpportunityDeck { get; private set; }

        public Deck<Intrigue> SkulduggeryDeck { get; private set; }

        public int TurnNumber { get; private set; }

        static Game()
        {
            Games = new Dictionary<int, Game>();
            NextGameId = 0;
        }

        private Game(User owner)
        {
            PlayerGameViews = new Dictionary<User, GameView>();
            Players = new Dictionary<User, Player>();
            ExpectedActions = new Dictionary<Player, ModalType>();
            Observers = new List<User>();
            GameLog = new Log(50);
            PublicChatId = NextChatId++;
            ChatList = new ChatList();
            ChatList.Chats.Add(new Chat(PublicChatId, "Public Chat", new List<Player>()));
            Settings = new GameSettings();
            BidSession = new BidSession();
            CurrentState = GameState.NotStarted;
            //Not obvious: the GameView needs to be initialized AFTER the GameLog, or else its LogInfo will be pointing at null.
            ObserverGameView = new GameView(this, PlayerState.Observer, null);
            Owner = owner;
            Observers.Add(owner);
            //Observers.Add(User.Users["TestPlayer"]);
            //Observers.Add(User.Users["TestPlayer2"]);
            //TryBecomePlayer(User.Users["TestPlayer"]);
            //TryBecomePlayer(User.Users["TestPlayer2"]);
            Setup();
        }

        public static int CreateGame(User owner)
        {
            int newGameId = ++NextGameId;
            Games.Add(newGameId, new Game(owner));
            return newGameId;
        }

        public GameJoinStatus TryJoin(User user)
        {
            //In future, this will do some actual checking and verification
            Observers.Add(user);
            AddGameLogEntry(user.Name + " joined the game as an observer.");
            return GameJoinStatus.Join;
        }

        public GameView TryBecomePlayer(User user)
        {
            if (Observers.Contains(user) && CurrentState == GameState.NotStarted)
            {
                lock(Lock) {
                    Observers.Remove(user);
                    Player player = new Player(user, NextPlayerId++);
                    ChatList.Chats.Where(chat => chat.Id == PublicChatId).First().AddPlayer(player);
                    Players.Add(user, player);
                    ExpectedActions.Add(player, ModalType.None);
                    //AddPlayer needs to be invoked before the GameView can be created, because the GameView subscribes to AddPlayer
                    AddPlayer.Invoke(player);
                    PlayerGameViews.Add(user, new GameView(this, PlayerState.Player, player));
                    AddGameLogEntry(user.Name + " joined the game.");
                    PushGameRevision(Constants.OBJECT_PLAYER_INFO);
                    return PlayerGameViews[user];
                }
            }
            return null;
        }

        public GameView GetGameView(User user)
        {
            lock (Lock)
            {
                if (PlayerGameViews.ContainsKey(user))
                {
                    return PlayerGameViews[user];
                }
                else if (Observers.Contains(user))
                {
                    return ObserverGameView;
                }
                else
                {
                    return null;
                }
            }
        }

        public void TryCreateChat(User user, string name, params int[] playerIds)
        {
            Player currentPlayer = Players[user];
            if (!IsValidPlayerAction(currentPlayer))
            {
                return;
            }
            lock (Lock)
            {
                if (playerIds.Length == 0)
                {
                    return;
                }
                List<Player> players = new List<Player>();
                players.Add(currentPlayer);
                foreach (int playerId in playerIds)
                {
                    Player player = Players.Values.Where(play => play.Id == playerId).First();
                    if (!players.Contains(player))
                    {
                        players.Add(player);
                    }
                }
                //no duplicate chats
                if (ChatList.Chats.Exists(c => (c.Id != ChatList.Chats[0].Id) && c.IncludedPlayers.TrueForAll(p => playerIds.Contains(p.Id)))) {
                    return;
                }
                ChatList.Chats.Add(new Chat(NextChatId++, name, players));
                PushGameRevision(Constants.OBJECT_CHAT);
            }
        }

        private bool IsValidPlayerAction(Player player, GameState state = GameState.Any, ModalType type = ModalType.None)
        {
            if (state != GameState.Any && CurrentState != state)
            {
                return false;
            }
            if (ExpectedActions[player] != type)
            {
                return false;
            }
            return true;
        }

        private void PushPlayerModal(Player player, IModal modal)
        {
            PlayerGameViews[player.User].PushModal(modal);
            ExpectedActions[player] = modal.Type;
        }

        private void ClearPlayerModal(Player player)
        {
            //Todo: special modal handling for modals demanding responses
            switch (ExpectedActions[player])
            {
                case ModalType.ExecHire:
                    ExecutiveDeck.Discard(((HireExecutiveModal)PlayerGameViews[player.User].Modal).Executives.ToArray());
                    break;
                case ModalType.OpportunitySearch:
                    OpportunityDeck.Discard(((OpportunitySearchModal)PlayerGameViews[player.User].Modal).Opportunities.ToArray());
                    break;
                case ModalType.IntrigueSearch:
                    SkulduggeryDeck.Discard(((IntrigueSearchModal)PlayerGameViews[player.User].Modal).Intrigues.ToArray());
                    break;
            }
            PlayerGameViews[player.User].ClearModal();
            ExpectedActions[player] = ModalType.None;
        }

        public bool EnterChatLine(User user, int chatId, string entry)
        {
            Player player = Players[user];
            if (!IsValidPlayerAction(player))
            {
                return false;
            }
            lock (Lock)
            {
                if (!ChatList.Chats.Exists(c => c.Id == chatId))
                {
                    return false;
                }
                Chat chat = ChatList.Chats.Where(c => c.Id == chatId).First();
                if (!chat.IncludedPlayers.Contains(player))
                {
                    return false;
                }
                chat.AddEntry(player, entry);
                PushGameRevision(Constants.OBJECT_CHAT);
                return true;
            }
        }

        public void ReadyAdvance(User user, GameState expectedNextState)
        {
            Player player = Players[user];
            if (!IsValidPlayerAction(player))
            {
                return;
            }
            lock (Lock)
            {
                //If this is a mis-timed request, don't do anything
                if (!IsExpectedNextState(expectedNextState))
                {
                    return;
                }
                player.SetReadiness(true);
                AddGameLogEntry(user.Name + " is ready to advance.");
                if (AllReadyAdvance())
                {
                    Advance(expectedNextState);
                }
                else
                {
                    PushGameRevision(Constants.OBJECT_PLAYER_INFO);
                }
            }
        }

        public void UnreadyAdvance(User user)
        {
            Player player = Players[user];
            if (!IsValidPlayerAction(player))
            {
                return;
            }
            lock (Lock)
            {
                if (player.IsReady)
                {
                    player.SetReadiness(false);
                    AddGameLogEntry(user.Name + " is no longer ready to advance.");
                    PushGameRevision(Constants.OBJECT_PLAYER_INFO);
                }
            }
        }

        private bool AllReadyAdvance()
        {
            return !Players.Any(a => a.Value.IsReady == false);
        }

        private void Advance(GameState expectedNextState)
        {
            lock (Lock)
            {
                //If this is a mis-timed request, don't do anything
                if (!IsExpectedNextState(expectedNextState))
                {
                    return;
                }
                foreach (Player player in Players.Values)
                {
                    player.SetReadiness(false);
                    ClearPlayerModal(player);
                    PushGameRevision(Constants.OBJECT_MODAL);
                    PushGameRevision(Constants.OBJECT_PLAYER_INFO);
                }
                switch (expectedNextState)
                {
                    case GameState.WorkPhase:
                        AdvanceToWorkPhase();
                        break;
                    case GameState.BidPhase:
                        AdvanceToBidPhase();
                        break;
                }
            }
        }

        private void AdvanceToWorkPhase()
        {
            lock (Lock)
            {
                //If we're not starting the game out, do all start-of-turn actions
                if (CurrentState != GameState.NotStarted)
                {
                    StartOfTurnActions();
                }
                CurrentState = GameState.WorkPhase;
                PushGameRevision(Constants.OBJECT_GAME);
                AddGameLogEntry("Advanced to Work Phase");
            }
        }

        private void StartOfTurnActions()
        {
            TurnNumber++;
            foreach (Player player in Players.Values)
            {
                if (player.Bankrupt)
                {
                    if (TurnNumber > 10)
                    {
                        player.ResolveBankruptcy(true);
                        AddGameLogEntry(player.User.Name + " has discharged their debt. They keep their reputation and experience, but lose all capital and start over.");
                    }
                    else
                    {
                        foreach (Project project in player.CompletedProjects)
                        {
                            if (!project.IsPrivate)
                            {
                                ProjectPipeline.DiscardProject(project);
                            }
                        }
                        player.ResolveBankruptcy(false);
                        AddGameLogEntry(player.User.Name + " has discharged their debt. They lose everything and start over.");
                    }
                }
                else
                {
                    PaySalaries(player);
                    player.ResetForTurn();
                }
            }
            ProjectPipeline.MoveNextTurn(TurnNumber);
            PushGameRevision(Constants.OBJECT_PROJECT_PIPELINE);
            PushGameRevision(Constants.OBJECT_PLAYER_INFO);
            PushGameRevision(Constants.OBJECT_EXEC_INFO);
        }

        private void AdvanceToBidPhase()
        {
            lock (Lock)
            {
                CurrentState = GameState.BidPhase;
                foreach (Player player in Players.Values)
                {
                    PayOverdueProjectFees(player);
                    if (player.Money == 0)
                    {
                        player.GoBankrupt();
                        foreach (Project project in player.CurrentProjects)
                        {
                            if (!project.IsPrivate)
                            {
                                ProjectPipeline.DiscardProject(project);
                            }
                        }
                        AddGameLogEntry(player.User.Name + " has gone bankrupt. They will discharge their debts and start anew at the beginning of the next turn.");
                    }
                }
                SetupBids();
                PushGameRevision(Constants.OBJECT_GAME);
                AddGameLogEntry("Advanced to Bid phase");
            }
        }

        private bool IsExpectedNextState(GameState expectedNextState)
        {
            return expectedNextState == ExpectedNextState(CurrentState);
        }

        private void AddGameLogEntry(string entry)
        {
            GameLog.AddEntry(entry);
            PushGameRevision(Constants.OBJECT_GAME_LOG);
        }

        private void PushGameRevision(string identifier)
        {
            foreach (GameView view in PlayerGameViews.Values)
            {
                //push overall game revision
                view.PushRevision(identifier);
            }
            ObserverGameView.PushRevision(identifier);
        }

        public static GameState ExpectedNextState(GameState currentState)
        {
            switch (currentState)
            {
                case GameState.NotStarted:
                case GameState.BidPhase:
                    return GameState.WorkPhase;
                case GameState.Ended:
                    return GameState.InvalidState;
                case GameState.WorkPhase:
                    return GameState.BidPhase;
                default:
                    throw new Exception("Unhandled current state");
            }
        }

        private void PayOverdueProjectFees(Player player)
        {
            foreach (Project project in player.CurrentProjects)
            {
                if (TurnNumber >= project.TurnDueBy)
                {
                    int fine = (project.MaxBid + project.Price) / 20; //Fine is average of max bid and actual price
                    player.AdjustMoney(fine, true);
                    AddGameLogEntry(player.User.Name + " was fined " + Utility.MoneyString(fine) + " for their overdue project " + project.Name + ".");
                    PushGameRevision(Constants.OBJECT_PLAYER_INFO);
                }
            }
        }

        private void PaySalaries(Player player)
        {
            int workerPayment = player.TotalWorkers * Settings.WorkerSalary;
            int executivePayment = 0;
            foreach (Executive exec in player.Executives)
            {
                executivePayment += exec.Salary;
            }
            PushPlayerModal(player, new CostReportModal(workerPayment, executivePayment));
            player.AdjustMoney(workerPayment + executivePayment, true);
            AddGameLogEntry(player.User.Name + " paid " + Utility.MoneyString(workerPayment + executivePayment) + " to their employees.");
            PushGameRevision(Constants.OBJECT_PLAYER_INFO);
        }

        public void ClearModal(User user, GameState phase, ModalType type)
        {
            lock (Lock)
            {
                Player player = Players[user];
                if (IsValidPlayerAction(player, phase, type))
                {
                    ClearPlayerModal(player);
                }
            }
        }
    }

    public enum GameState
    {
        InvalidState = 0,
        NotStarted = 1,
        WorkPhase = 2,
        BidPhase = 3,
        Any = 998,
        Ended = 999
    }

    public enum GameJoinStatus
    {
        Join = 1,
        Wait = 2,
        Forbidden = 3
    }
}
