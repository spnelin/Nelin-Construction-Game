using BuildingWebsite.Server.ServerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server
{
    public partial class Game
    {
        public void SubmitBidForProject(User user, int bidQuantity, int expectedBidIndex)
        {
            Player player = Players[user];
            if (!IsValidPlayerAction(player, GameState.BidPhase))
            {
                return;
            }
            lock (Lock)
            {
                if (CurrentState != GameState.BidPhase) //this no longer triggers
                {
                    AddGameLogEntry(user.Name + " tried to bid for a project in the wrong phase.");
                    return;
                }
                else if (bidQuantity < -1)
                {
                    AddGameLogEntry(user.Name + " tried to bid an invalid quantity.");
                    return;
                }
                if (expectedBidIndex != BidSession.CurrentBidIndex)
                {
                    return;
                }
                if (bidQuantity > BidSession.CurrentProject.MaxBid)
                {
                    AddGameLogEntry(user.Name + " tried to bid an invalid quantity.");
                    return;
                }
                if (bidQuantity == -1)
                {
                    AddGameLogEntry(user.Name + " canceled their bid.");
                    BidSession.RemovePlayerBid(player);
                    PushGameRevision(Constants.OBJECT_BID_SESSION);
                    return;
                }
                else if (!BidSession.TryAddPlayerBid(player, bidQuantity))
                {
                    AddGameLogEntry(user.Name + " tried to bid on a project they weren't supposed to.");
                    return;
                }
                if (BidSession.AllBidsSubmitted())
                {
                    HandleSubmittedBids();
                }
                PushGameRevision(Constants.OBJECT_BID_SESSION);
            }
        }

        private void SetupBids()
        {
            BidSession.InitiateBidSession(ProjectPipeline.BiddableProjects());
            SetupBidders();
            PushGameRevision(Constants.OBJECT_BID_SESSION);
            PushGameRevision(Constants.OBJECT_PROJECT_PIPELINE);
        }

        private void SetupBidders()
        {
            Project project = BidSession.CurrentProject;
            BidSession.AddBidders(Players.Values);
            foreach (Player player in Players.Values)
            {
                if (player.BidThisTurn)
                {
                    BidSession.DisqualifyPlayer(player, "Has already won a bid this turn.");
                }
                else if (!project.PlayerQualifiedForProject(player))
                {
                    BidSession.DisqualifyPlayer(player, "Required projects to qualify have not been completed.");
                }
            }
        }

        private void HandleSubmittedBids()
        {
            Project project = BidSession.CurrentProject;
            AddGameLogEntry("Bids submitted for " + project.Name + ":");
            int lowestBid = project.MaxBid + 1;
            List<Player> lowestBidders = new List<Player>();
            foreach (Player player in BidSession.CurrentBids.Keys)
            {
                int bid = BidSession.CurrentBids[player];
                //0 is the number I'm using for "skip bid"
                if (bid == 0)
                {
                    continue;
                }
                AddGameLogEntry(player.User.Name + " bid " + Utility.MoneyString(bid) + ".");
                if (bid < lowestBid)
                {
                    lowestBid = BidSession.CurrentBids[player];
                    lowestBidders.Clear();
                    lowestBidders.Add(player);
                    continue;
                }
                else if (BidSession.CurrentBids[player] == lowestBid)
                {
                    lowestBidders.Add(player);
                }
            }
            if (lowestBidders.Count == 1)
            {
                AddGameLogEntry(lowestBidders[0].User.Name + " won the bid for " + BidSession.CurrentProject.Name + ".");
                GrantBid(lowestBidders[0], lowestBid);
                MoveNextProject();
            }
            else if (lowestBidders.Count == 0)
            {
                AddGameLogEntry("Nobody submitted a bid. Bidding has ended for this project.");
                MoveNextProject();
            }
            else if (lowestBid == BidSession.CurrentMaxBid)
            {
                string players = "";
                foreach (Player player in lowestBidders)
                {
                    players += player.User.Name + ", ";
                }
                players = players.Substring(0, players.Length - 2);
                AddGameLogEntry("The bid was tied between: " + players + ". Since all bids were tied at the maximum bid of " + BidSession.CurrentMaxBid + ", the bidding has ended with no winner.");
                MoveNextProject();
            }
            else
            {
                string players = "";
                foreach (Player player in lowestBidders)
                {
                    players += player.User.Name + ", ";
                }
                players = players.Substring(0, players.Length - 2);
                AddGameLogEntry("The lowest bid was tied between: " + players + ". Proceeding to a runoff bid.");
                SetupRunoffBid(lowestBidders, lowestBid);
            }
        }

        private void GrantBid(Player player, int bid)
        {
            player.AddProject(BidSession.CurrentProject);
            ProjectPipeline.RemoveProject(BidSession.CurrentProject);
            BidSession.CurrentProject.SetPrice(bid);
            //For now, money comes when you win a bid
            player.AdjustMoney(bid);
            AddGameLogEntry(player.User.Name + " was awarded the bid price of " + Utility.MoneyString(bid) + ".");
            PushGameRevision(Constants.OBJECT_PLAYER_INFO);
        }

        private void MoveNextProject()
        {
            if (BidSession.MoveNextProject())
            {
                SetupBidders();
                if (BidSession.CurrentBiddingPlayers.Count == 0)
                {
                    AddGameLogEntry("No players were qualified to bid on " + BidSession.CurrentProject.Name + ". Skipping.");
                    MoveNextProject();
                }
            }
            else
            {
                AddGameLogEntry("All projects bid on. Proceeding to next turn.");
                AdvanceToWorkPhase();
            }
        }

        private void SetupRunoffBid(List<Player> players, int lowBid)
        {
            BidSession.CurrentBiddingPlayers.Where(player => !players.Contains(player)).ToList().ForEach(player => BidSession.DisqualifyPlayer(player, "Not part of this runoff bid."));
            BidSession.SetRunoffBid(lowBid);
        }
    }
}
