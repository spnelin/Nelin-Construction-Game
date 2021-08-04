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
        public void AssignExecutive(User user, int executiveIndex, ExecutiveTaskType task)
        {
            Player player = Players[user];
            if (!IsValidPlayerAction(player, GameState.WorkPhase))
            {
                return;
            }
            if (task == ExecutiveTaskType.None)
            {
                AddGameLogEntry(user.Name + " tried to use an invalid task type.");
                return;
            }
            lock (Lock)
            {
                if (executiveIndex < 0 || executiveIndex >= player.Executives.Count)
                {
                    AddGameLogEntry(user.Name + " tried to use an invalid executive.");
                    return;
                }
                Executive exec = player.Executives[executiveIndex];
                int drawNumber;
                exec.TakeAction(task);
                switch (task)
                {
                    case ExecutiveTaskType.BuyMaterials:
                        //no further handling needed
                        break;
                    case ExecutiveTaskType.FindOpportunity:
                        drawNumber = exec.Ability == ExecutiveAbility.BetterOpportunityFinding ? 4 : 3;
                        PushPlayerModal(player, new OpportunitySearchModal(OpportunityDeck.Draw(drawNumber)));
                        break;
                    case ExecutiveTaskType.FindSkulduggery:
                        //todo: skulduggery
                        break;
                    case ExecutiveTaskType.HireExecutive:
                        drawNumber = exec.Ability == ExecutiveAbility.BetterExecutiveHiring ? 3 : 2;
                        PushPlayerModal(player, new HireExecutiveModal(ExecutiveDeck.Draw(drawNumber)));
                        break;
                    case ExecutiveTaskType.HireWorkers:
                        //no further handling needed
                        break;
                    default:
                        throw new Exception("Unhandled task type.");
                }
                PushGameRevision(Constants.OBJECT_EXEC_INFO);
            }
        }

        public void BuyMaterials(User user, MaterialType type, int quantity)
        {
            Player player = Players[user];
            if (!IsValidPlayerAction(player, GameState.WorkPhase)) {
                return;
            }
            lock (Lock) {
                if (CurrentState != GameState.WorkPhase)
                {
                    AddGameLogEntry(user.Name + " tried to buy materials in the wrong phase.");
                    return;
                }
                if (quantity < 0) { return; }
                int totalCost = CostByMaterial(type, player.Executives) * quantity;
                if (totalCost == 0)
                {
                    return;
                }
                if (totalCost > player.Money)
                {
                    AddGameLogEntry(user.Name + " tried to buy more of a material than they could afford.");
                    return;
                }
                player.AdjustMoney(totalCost, true);
                switch(type)
                {
                    case MaterialType.Concrete:
                        player.AdjustConcrete(quantity);
                        break;
                    case MaterialType.Steel:
                        player.AdjustSteel(quantity);
                        break;
                    case MaterialType.Glass:
                        player.AdjustGlass(quantity);
                        break;
                }
                AddGameLogEntry(user.Name + " bought " + quantity + " units of " + MaterialName(type) + ".");
                PushGameRevision(Constants.OBJECT_PLAYER_INFO);
                PushGameRevision(Constants.OBJECT_EXEC_INFO);
            }
        }

        public void BuyMaterialDeal(User user, int quantity)
        {
            Player player = Players[user];
            if (!IsValidPlayerAction(player, GameState.WorkPhase, ModalType.MaterialDeal))
            {
                return;
            }
            lock (Lock)
            {
                if (quantity < 1) { return; }
                MaterialDealModal modal = (MaterialDealModal)PlayerGameViews[user].Modal;
                if (modal.MaxQuantity < quantity)
                {
                    AddGameLogEntry(user.Name + "tried to order more materials on a deal than they should have been able to.");
                    return;
                }
                MaterialType type = modal.Material;
                int totalCost = modal.Price * quantity;
                
                if (totalCost > player.Money)
                {
                    AddGameLogEntry(user.Name + " tried to buy more of a material than they could afford.");
                    return;
                }
                player.AdjustMoney(totalCost, true);
                switch (type)
                {
                    case MaterialType.Concrete:
                        player.AdjustConcrete(quantity);
                        break;
                    case MaterialType.Steel:
                        player.AdjustSteel(quantity);
                        break;
                    case MaterialType.Glass:
                        player.AdjustGlass(quantity);
                        break;
                }
                AddGameLogEntry(user.Name + " bought " + quantity + " units of " + MaterialName(type) + " for below market price in a special deal.");
                player.Hand.RemoveAt(modal.PlayerHandIndex);
                ClearPlayerModal(player);
                PushGameRevision(Constants.OBJECT_PLAYER_INFO);
                PushGameRevision(Constants.OBJECT_EXEC_INFO);
                PushGameRevision(Constants.OBJECT_HAND);
            }
        }

        private int CostByMaterial(MaterialType type, List<Executive> executives)
        {
            bool discount = false;
            switch (type)
            {
                case MaterialType.Concrete:
                    foreach (Executive executive in executives)
                    {
                        switch (executive.Ability)
                        {
                            case ExecutiveAbility.ReduceConcretePrice:
                                discount = true;
                                break;
                            case ExecutiveAbility.ReduceConcretePricePlus:
                                return Settings.ConcretePriceDiscountPlus; //max discount, no need to look further
                        }
                    }
                    return discount ? Settings.ConcretePriceDiscount : Settings.ConcretePrice;
                case MaterialType.Steel:
                    foreach (Executive executive in executives)
                    {
                        switch (executive.Ability)
                        {
                            case ExecutiveAbility.ReduceSteelPrice:
                                discount = true;
                                break;
                            case ExecutiveAbility.ReduceSteelPricePlus:
                                return Settings.SteelPriceDiscountPlus; //max discount, no need to look further
                        }
                    }
                    return discount ? Settings.SteelPriceDiscount : Settings.SteelPrice;
                case MaterialType.Glass:
                    foreach (Executive executive in executives)
                    {
                        switch (executive.Ability)
                        {
                            case ExecutiveAbility.ReduceGlassPrice:
                                discount = true;
                                break;
                            case ExecutiveAbility.ReduceGlassPricePlus:
                                return Settings.GlassPriceDiscountPlus; //max discount, no need to look further
                        }
                    }
                    return discount ? Settings.GlassPriceDiscount : Settings.GlassPrice;
                default:
                    throw new Exception("Unhandled enumeration");
            }
        }

        //todo: make this into Constants?
        private string MaterialName(MaterialType type, bool isCapitalized = false)
        {
            switch (type)
            {
                case MaterialType.Concrete:
                    return (isCapitalized ? "C" : "c") + "oncrete";
                case MaterialType.Steel:
                    return (isCapitalized ? "S" : "s") + "teel";
                case MaterialType.Glass:
                    return (isCapitalized ? "G" : "g") + "lass";
                default:
                    throw new Exception("Unhandled enumeration");
            }
        }

        public void TryHireWorkers(User user, int quantity)
        {
            Player player = Players[user];
            if (!IsValidPlayerAction(player, GameState.WorkPhase)) {
                return;
            }
            lock (Lock)
            {
                if (CurrentState != GameState.WorkPhase)
                {
                    AddGameLogEntry(user.Name + " tried to hire workers in the wrong phase.");
                    return;
                }
                if (quantity < 1) { return; }
                int totalCost = Settings.WorkerSalary * quantity;
                if (totalCost > player.Money)
                {
                    AddGameLogEntry(user.Name + " tried to hire more workers than they could afford.");
                    return;
                }
                player.AdjustMoney(totalCost, true);
                player.AdjustTotalWorkers(quantity);
                AddGameLogEntry(user.Name + " hired " + quantity + " workers.");
                PushGameRevision(Constants.OBJECT_PLAYER_INFO);
                PushGameRevision(Constants.OBJECT_EXEC_INFO);
            }
        }

        public void HireExecutive(User user, int executiveIndex)
        {
            Player player = Players[user];
            if (!IsValidPlayerAction(player, GameState.WorkPhase, ModalType.ExecHire)) {
                return;
            }
            lock (Lock)
            {
                HireExecutiveModal modal = (HireExecutiveModal)PlayerGameViews[user].Modal;
                if (modal.Executives.Count <= executiveIndex || executiveIndex < 0)
                {
                    AddGameLogEntry(user.Name + " tried to hire an invalid executive.");
                    return;
                }
                Executive executive = modal.Executives[executiveIndex];
                if (executive.Salary > player.Money)
                {
                    AddGameLogEntry(user.Name + " tried to hire an executive they couldn't afford.");
                    return;
                }
                player.AdjustMoney(executive.Salary, true);
                player.AddExecutive(executive);
                for (int i = 0; i < modal.Executives.Count; i++)
                {
                    if (i == executiveIndex)
                    {
                        continue;
                    }
                    ExecutiveDeck.Discard(modal.Executives[i]);
                }
                ClearPlayerModal(player);
                AddGameLogEntry(user.Name + " hired the new executive " + executive.Name + ".");
                PushGameRevision(Constants.OBJECT_PLAYER_INFO);
                PushGameRevision(Constants.OBJECT_EXEC_INFO);
            }
        }

        public void AddMaterialsToProject(User user, int projIndex, int concrete, int steel, int glass)
        {
            if (concrete + steel + glass < 1)
            {
                return;
            }
            Player player = Players[user];
            if (!IsValidPlayerAction(player, GameState.WorkPhase)) {
                return;
            }
            lock (Lock)
            {
                List<Project> projects = player.CurrentProjects;
                if ((projIndex < 0) || (projIndex >= projects.Count))
                {
                    AddGameLogEntry(user.Name + " tried to work on an invalid project.");
                    return;
                }
                Project project = projects[projIndex];
                int workersRequired = concrete + steel + glass;
                ExecutiveAbility betterBuildingAbility = ExecutiveAbility.None;
                switch (project.Type)
                {
                    case ProjectType.Bridge:
                        betterBuildingAbility = ExecutiveAbility.BetterBridgeBuilding;
                        break;
                    case ProjectType.Building:
                        betterBuildingAbility = ExecutiveAbility.BetterBuildingBuilding;
                        break;
                    case ProjectType.Tunnel:
                        betterBuildingAbility = ExecutiveAbility.BetterTunnelBuilding;
                        break;
                }
                if (betterBuildingAbility != ExecutiveAbility.None)
                {
                    if (player.Executives.Exists(e => e.Ability == betterBuildingAbility))
                    {
                        workersRequired = (workersRequired / 2) + workersRequired % 2;
                    }
                }
                if (concrete > project.TotalConcrete - project.CurrentConcrete)
                {
                    AddGameLogEntry(user.Name + " tried to use more concrete than a project needed.");
                    return;
                }
                if (concrete > player.Concrete)
                {
                    AddGameLogEntry(user.Name + " tried to use more concrete than they had.");
                    return;
                }
                if (steel > project.TotalSteel - project.CurrentSteel)
                {
                    AddGameLogEntry(user.Name + " tried to use more steel than a project needed.");
                    return;
                }
                if (steel > player.Steel)
                {
                    AddGameLogEntry(user.Name + " tried to use more steel than they had.");
                    return;
                }
                if (glass > project.TotalGlass - project.CurrentGlass)
                {
                    AddGameLogEntry(user.Name + " tried to use more glass than a project needed.");
                    return;
                }
                if (glass > player.Glass)
                {
                    AddGameLogEntry(user.Name + " tried to use more glass than they had.");
                    return;
                }
                if (workersRequired > player.FreeWorkers)
                {
                    AddGameLogEntry(user.Name + " tried to use more free workers than they had.");
                    return;
                }
                if (workersRequired > project.MaxWorkers - project.WorkersThisTurn)
                {
                    AddGameLogEntry(user.Name + " tried to assign more workers to this project this turn than are allowed.");
                    return;
                }
                player.AdjustFreeWorkers(workersRequired, true);
                player.AdjustConcrete(concrete, true);
                player.AdjustSteel(steel, true);
                player.AdjustGlass(glass, true);
                if (project.AddMaterialsToProjectAndCheckIfCompleted(concrete, steel, glass))
                {
                    player.CompleteProject(project);
                    AddGameLogEntry(user.Name + " completed construction project " + project.Name + ".");
                    PushPlayerModal(player, new ProjectCompletionModal(project));
                    PushGameRevision(Constants.OBJECT_PLAYER_INFO);
                }
                else
                {
                    //grammar is a pain
                    string log = user.Name + " added ";
                    if (concrete > 0)
                    {
                        log += concrete + " concrete";
                        if (steel > 0)
                        {
                            if (glass > 0)
                            {
                                log += ", " + steel + " steel, and " + glass + " glass";
                            }
                            else
                            {
                                log += " and " + steel + " steel";
                            }
                        }
                        else if (glass > 0)
                        {
                            log += " and " + glass + " glass";
                        }
                    }
                    else if (steel > 0)
                    {
                        log += steel + " steel";
                        if (glass > 0)
                        {
                            log += " and " + glass + " glass";
                        }
                    }
                    log += " to their project " + project.Name + ".";
                    AddGameLogEntry(log);
                }
                PushGameRevision(Constants.OBJECT_PLAYER_INFO);
            }
        }

        public void ViewCostReport(User user)
        {
            lock (Lock)
            {
                Player player = Players[user];
                if (IsValidPlayerAction(player, GameState.WorkPhase, ModalType.CostReport))
                {
                    ClearPlayerModal(player);
                }
            }
        }

        public void FindOpportunity(User user, int opportunityIndex)
        {
            Player player = Players[user];
            if (!IsValidPlayerAction(player, GameState.WorkPhase, ModalType.OpportunitySearch))
            {
                return;
            }
            lock (Lock)
            {
                OpportunitySearchModal modal = (OpportunitySearchModal)PlayerGameViews[user].Modal;
                if (modal.Opportunities.Count <= opportunityIndex || opportunityIndex < 0)
                {
                    AddGameLogEntry(user.Name + " tried to hire an invalid executive.");
                    return;
                }
                Opportunity opportunity = modal.Opportunities[opportunityIndex];
                player.Hand.Add(opportunity);
                for (int i = 0; i < modal.Opportunities.Count; i++)
                {
                    if (i == opportunityIndex)
                    {
                        continue;
                    }
                    OpportunityDeck.Discard(modal.Opportunities[i]);
                }
                ClearPlayerModal(player);
                AddGameLogEntry(user.Name + " searched out an opportunity.");
                PushGameRevision(Constants.OBJECT_HAND);
            }
        }

        public void FireExecutive(User user, int executiveIndex)
        {
            Player player = Players[user];
            if (!IsValidPlayerAction(player, GameState.WorkPhase))
            {
                return;
            }
            lock (Lock)
            {
                if (executiveIndex < 0 || executiveIndex >= player.Executives.Count)
                {
                    throw new IndexOutOfRangeException(user.Name + " used an invalid executive index.");
                }
                if (executiveIndex == 0)
                {
                    throw new ArgumentException(user.Name + " tried to fire themselves as CEO.");
                }
                Executive executive = player.Executives[executiveIndex];
                executive.ResetSalaryAdjustment();
                ExecutiveDeck.Discard(executive);
                player.Executives.RemoveAt(executiveIndex);
                AddGameLogEntry(user.Name + " fired their executive " + executive.Name + ".");
                PushGameRevision(Constants.OBJECT_EXEC_INFO);
            }
        }

        public void FireWorkers(User user, int workersToFire)
        {
            Player player = Players[user];
            if (!IsValidPlayerAction(player, GameState.WorkPhase))
            {
                return;
            }
            if (workersToFire == 0)
            {
                return;
            }
            lock (Lock)
            {
                if (workersToFire > player.TotalWorkers)
                {
                    throw new ArgumentException(user.Name + " tried to fire more workers than they have.");
                }
                player.AdjustTotalWorkers(workersToFire, true);
                AddGameLogEntry(user.Name + " fired " + workersToFire + " workers.");
                PushGameRevision(Constants.OBJECT_PLAYER_INFO);
            }
        }
    }
}
