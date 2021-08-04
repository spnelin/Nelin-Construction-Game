using BuildingWebsite.Server.ClientObjects;
using BuildingWebsite.Server.ServerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server
{
    public class GameView
    {
        private Game Game { get; set; }

        public GameView(Game game, PlayerState state, Player player)
        {
            Game = game;
            PlayerState = state;
            GameInfo = new GameInfo(game);
            PlayerInfoList = new PlayerGenericInfoList(game, player);
            Executives = new ExecutiveInfoList(player);
            GameLog = new LogInfo(game.GameLog);
            ProjectPipeline = new ProjectPipelineView(game.ProjectPipeline, player);
            BidSession = new BidSessionView(game.BidSession, player);
            Chat = new ChatListView(game.ChatList, player);
            Modal = new NoModal();
            Hand = player != null ? new CardHandView(player.Hand, player) : null;
        }

        public PlayerState PlayerState { get; private set; }

        public void UpdatePlayerState(PlayerState newState)
        {
            PlayerState = newState;
        }


        #region Info Objects
        private GameInfo GameInfo { get; set; }

        private ChatListView Chat { get; set; }

        private PlayerGenericInfoList PlayerInfoList { get; set; }

        private ExecutiveInfoList Executives { get; set; }

        private LogInfo GameLog { get; set; }

        private ProjectPipelineView ProjectPipeline { get; set; }

        private BidSessionView BidSession { get; set; }
        
        private CardHandView Hand { get; set; }

        public IModal Modal { get; private set; }
        #endregion


        private List<Revision> Revisions = new List<Revision>();

        public void PushRevision(string identifier)
        {
            Revisions.Add(new Revision(identifier));
        }

        public void PushModal(IModal modal)
        {
            Modal = modal;
            PushRevision(Constants.OBJECT_MODAL);
        }

        public void ClearModal()
        {
            Modal = new NoModal();
            PushRevision(Constants.OBJECT_MODAL);
        }

        public bool HasNewRevision(int clientRevisionNum)
        {
            int latestRevisionNum = Revisions.Count;
            return latestRevisionNum > clientRevisionNum;
        }

        public ClientRevision GetRevision(int clientRevisionNum)
        {
            int latestRevisionNum = Revisions.Count;
            if (clientRevisionNum == latestRevisionNum)
            {
                return ClientRevision.EmptyRevision(PlayerState, Game);
            }
            if (clientRevisionNum < 0 || clientRevisionNum >= latestRevisionNum)
            {
                throw new Exception("Bad Revision #");
            }
            if (PlayerState == PlayerState.Player)
            {
                ClientRevision revision = new ClientRevision(PlayerState, Game);
                foreach (Revision rev in Revisions.GetRange(clientRevisionNum, latestRevisionNum - clientRevisionNum))
                {
                    if (!revision.ChangedObjects.Exists(a => a.Identifier == rev.ChangedObject))
                    {
                        revision.ChangedObjects.Add(ChangedObjectFromIdentifier(rev.ChangedObject));
                    }
                }
                return revision;
            }
            else
            {
                //For observers, we just want to return empty revisions for now
                return ClientRevision.EmptyRevision(PlayerState, Game);
            }
        }

        public ClientRevision GetComprehensiveRevision()
        { 
            ChangedObject[] objects;
            if (PlayerState == PlayerState.Observer)
            {
                //For now, just return empty revision
                objects = new ChangedObject[1];
                objects[0] = new ChangedObject() { Identifier = Constants.OBJECT_GAME_LOG, Object = GameLog };
                return new ClientRevision(PlayerState, Game, objects);
            }
            objects = new ChangedObject[9];
            objects[0] = new ChangedObject() { Identifier = Constants.OBJECT_PLAYER_INFO, Object = PlayerInfoList };
            objects[1] = new ChangedObject() { Identifier = Constants.OBJECT_EXEC_INFO, Object = Executives };
            objects[2] = new ChangedObject() { Identifier = Constants.OBJECT_GAME_LOG, Object = GameLog };
            objects[3] = new ChangedObject() { Identifier = Constants.OBJECT_PROJECT_PIPELINE, Object = ProjectPipeline };
            objects[4] = new ChangedObject() { Identifier = Constants.OBJECT_GAME, Object = GameInfo };
            objects[5] = new ChangedObject() { Identifier = Constants.OBJECT_BID_SESSION, Object = BidSession };
            objects[6] = new ChangedObject() { Identifier = Constants.OBJECT_CHAT, Object = Chat };
            objects[7] = new ChangedObject() { Identifier = Constants.OBJECT_MODAL, Object = Modal };
            objects[8] = new ChangedObject() { Identifier = Constants.OBJECT_HAND, Object = Hand };
            return new ClientRevision(PlayerState, Game, objects);
        }

        public string LatestRevisionNumber => Revisions.Count.ToString();

        private ChangedObject ChangedObjectFromIdentifier(string identifier)
        {
            ChangedObject ret = new ChangedObject();
            ret.Identifier = identifier;
            switch (identifier)
            {
                case Constants.OBJECT_PLAYER_INFO:
                    ret.Object = PlayerInfoList;
                    break;
                case Constants.OBJECT_EXEC_INFO:
                    ret.Object = Executives;
                    break;
                case Constants.OBJECT_GAME_LOG:
                    ret.Object = GameLog;
                    break;
                case Constants.OBJECT_PROJECT_PIPELINE:
                    ret.Object = ProjectPipeline;
                    break;
                case Constants.OBJECT_GAME:
                    ret.Object = GameInfo;
                    break;
                case Constants.OBJECT_BID_SESSION:
                    ret.Object = BidSession;
                    break;
                case Constants.OBJECT_CHAT:
                    ret.Object = Chat;
                    break;
                case Constants.OBJECT_MODAL:
                    ret.Object = Modal;
                    break;
                case Constants.OBJECT_HAND:
                    ret.Object = Hand;
                    break;
                default:
                    ret.Identifier = Constants.OBJECT_UNIDENTIFIED;
                    ret.Object = "";
                    break;
            }
            return ret;
        }
    }
}
