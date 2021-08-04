using BuildingWebsite.Server.ServerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server.ClientObjects
{
    public class CardHandView
    {
        private CardHand BackingInfo { get; set; }

        public List<ICard> Cards => BackingInfo.OwningPlayer == ViewingPlayer ? BackingInfo.ToList() : BackingInfo.Select(c => HiddenCard.Card).ToList();

        private Player ViewingPlayer { get; set; }

        public CardHandView(CardHand backingInfo, Player viewingPlayer)
        {
            BackingInfo = backingInfo;
            ViewingPlayer = viewingPlayer;
        }
    }
}