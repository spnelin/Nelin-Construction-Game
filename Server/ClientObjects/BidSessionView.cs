using BuildingWebsite.Server.ServerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server.ClientObjects
{
    public class BidSessionView
    {
        private BidSession BackingInfo { get; set; }

        private Player ViewingPlayer { get; set; }

        public BidSessionView(BidSession backingInfo, Player viewingPlayer)
        {
            BackingInfo = backingInfo;
            ViewingPlayer = viewingPlayer;
        }

        public ProjectView CurrentProject => new ProjectView(BackingInfo.CurrentProject, ViewingPlayer);

        public int CurrentBid => BackingInfo.CurrentBids.ContainsKey(ViewingPlayer) ? BackingInfo.CurrentBids[ViewingPlayer] : -1;

        public int CurrentBidIndex => BackingInfo.CurrentBidIndex;

        public bool DisqualifiedFromBidding => BackingInfo.DisqualifiedPlayers.ContainsKey(ViewingPlayer);

        public string DisqualificationReason => DisqualifiedFromBidding ? BackingInfo.DisqualifiedPlayers[ViewingPlayer] : "";
    }
}
