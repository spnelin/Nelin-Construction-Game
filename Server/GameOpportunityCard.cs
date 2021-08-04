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
        public void PlayCard(User user, int cardIndex)
        {
            Player player = Players[user];
            if (!IsValidPlayerAction(player, GameState.WorkPhase))
            {
                return;
            }
            lock (Lock)
            {
                if (cardIndex < 0 || cardIndex >= player.Hand.Count)
                {
                    AddGameLogEntry(user.Name + " tried to use an invalid opportunity.");
                    return;
                }
                ICard card = player.Hand[cardIndex];
                if (card.CardType == CardType.Opportunity)
                {
                    Opportunity opportunity = (Opportunity)card;
                    switch (opportunity.Type)
                    {
                        case OpportunityType.ExecutiveHire:
                            player.Hand.RemoveAt(cardIndex);
                            PlayExecutiveHireOpportunity(player, (ExecutiveHireOpportunity)opportunity);
                            break;
                        case OpportunityType.MaterialDeal:
                            PlayMaterialDealOpportunity(player, (MaterialDealOpportunity)opportunity, cardIndex);
                            break;
                        case OpportunityType.MoneyWindfall:
                            player.Hand.RemoveAt(cardIndex);
                            PlayMoneyWindfallOpportunity(player, (MoneyWindfallOpportunity)opportunity);
                            break;
                        case OpportunityType.PrivateProject:
                            player.Hand.RemoveAt(cardIndex);
                            PlayPrivateProjectOpportunity(player, (PrivateProjectOpportunity)opportunity);
                            break;
                    }
                    OpportunityDeck.Discard(opportunity);
                }
                else
                {
                    //Intrigue to go here
                }
                PushGameRevision(Constants.OBJECT_HAND);
            }
        }

        public void PlayExecutiveHireOpportunity(Player player, ExecutiveHireOpportunity opportunity)
        {
            List<Executive> executives = ExecutiveDeck.Draw(opportunity.ExecutiveCount);
            executives.ForEach(a => a.SetSalaryAdjustment(opportunity.SalaryAdjustment));
            PushPlayerModal(player, new HireExecutiveModal(executives));
            AddGameLogEntry(player.User.Name + " played " + opportunity.Name + ", getting the chance to hire an executive!");
        }

        public void PlayMaterialDealOpportunity(Player player, MaterialDealOpportunity opportunity, int playerHandIndex)
        {
            PushPlayerModal(player, new MaterialDealModal(opportunity.Material, opportunity.Price, opportunity.MaxQuantity, playerHandIndex));
        }

        public void PlayMoneyWindfallOpportunity(Player player, MoneyWindfallOpportunity opportunity)
        {
            player.AdjustMoney(opportunity.Quantity);
            AddGameLogEntry(player.User.Name + " played " + opportunity.Name + ", gaining $" + opportunity.Quantity + "000!");
            PushGameRevision(Constants.OBJECT_PLAYER_INFO);
        }

        public void PlayPrivateProjectOpportunity(Player player, PrivateProjectOpportunity opportunity)
        {
            Project project = opportunity.Project;
            project.SetDueByTurn(TurnNumber);
            player.AddProject(project, true);
            AddGameLogEntry(player.User.Name + " played " + opportunity.Name + ", getting the chance to build an extra project!");
            PushGameRevision(Constants.OBJECT_HAND);
            PushGameRevision(Constants.OBJECT_PLAYER_INFO);
        }
    }
}
