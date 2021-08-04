using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server.ServerObjects
{
    public class BidSession
    {
        public List<Project> Projects { get; private set; }

        public int CurrentBidIndex { get; private set; }

        public Dictionary<Player, int> CurrentBids { get; private set; }

        public List<Player> CurrentBiddingPlayers { get; private set; }

        public Dictionary<Player, string> DisqualifiedPlayers { get; private set; }

        public Project CurrentProject => Projects == null ? Project.HiddenProject : Projects.Count > CurrentBidIndex ? Projects[CurrentBidIndex] : Project.HiddenProject;

        public int CurrentMaxBid { get; private set; }

        public BidSession()
        {
            CurrentBids = new Dictionary<Player, int>();
            CurrentBiddingPlayers = new List<Player>();
            DisqualifiedPlayers = new Dictionary<Player, string>();
        }

        public void InitiateBidSession(List<Project> projects)
        {
            Projects = projects;
            CurrentBids.Clear();
            CurrentBiddingPlayers.Clear();
            DisqualifiedPlayers.Clear();
            CurrentBidIndex = 0;
            if (CurrentProject != null)
            {
                CurrentMaxBid = CurrentProject.MaxBid;
            }
        }

        public bool MoveNextProject()
        {
            CurrentBids.Clear();
            CurrentBiddingPlayers.Clear();
            DisqualifiedPlayers.Clear();
            if (CurrentBidIndex + 1 == Projects.Count)
            {
                return false;
            }
            else
            {
                CurrentBidIndex++;
                CurrentMaxBid = CurrentProject.MaxBid;
                return true;
            }
        }

        public void AddBidders(IEnumerable<Player> players)
        {
            CurrentBiddingPlayers.AddRange(players);
        }

        public void DisqualifyPlayer(Player player, string reason)
        {
            if (CurrentBiddingPlayers.Remove(player))
            {
                DisqualifiedPlayers.Add(player, reason);
            }
        }

        public bool TryAddPlayerBid(Player player, int bid)
        {
            if (!CurrentBiddingPlayers.Contains(player))
            {
                return false;
            }
            CurrentBids.Add(player, bid);
            return true;
        }

        public void RemovePlayerBid(Player player)
        {
            CurrentBids.Remove(player);
        }

        public void SetRunoffBid(int newMaxBid)
        {
            CurrentMaxBid = newMaxBid;
            CurrentBids.Clear();
        }

        public bool AllBidsSubmitted()
        {
            return CurrentBids.Count == CurrentBiddingPlayers.Count;
        }
    }
}
