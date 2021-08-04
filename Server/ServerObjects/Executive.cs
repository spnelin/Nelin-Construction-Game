using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server.ServerObjects
{
    public class Executive
    {
        public string Name { get; private set; }

        public string Text { get; private set; }

        public int Salary => (int)Math.Ceiling(_salary * SalaryAdjustment);

        private int _salary;

        public double SalaryAdjustment { get; private set; }

        public ExecutiveAbility Ability { get; private set; }

        public ExecutiveTaskType CurrentTask { get; private set; }

        public bool ActionReady => CurrentTask == ExecutiveTaskType.None && !JustHired;

        public bool JustHired { get; private set; }

        public void TakeAction(ExecutiveTaskType currentTask)
        {
            CurrentTask = currentTask;
        }

        public void ResetAction()
        {
            CurrentTask = ExecutiveTaskType.None;
            JustHired = false;
        }

        public void SetSalaryAdjustment(double reduction)
        {
            SalaryAdjustment = reduction;
        }

        public void ResetSalaryAdjustment()
        {
            SetSalaryAdjustment(1);
        }

        public Executive(string name, string text, int salary, ExecutiveAbility ability, bool allowImmediateAction = false)
        {
            Name = name;
            Text = text;
            CurrentTask = ExecutiveTaskType.None;
            _salary = salary;
            Ability = ability;
            JustHired = !allowImmediateAction;
            SalaryAdjustment = 1;
        }
    }

    public enum ExecutiveAbility
    {
        None,
        ReduceConcretePrice,
        ReduceConcretePricePlus,
        ReduceSteelPrice,
        ReduceSteelPricePlus,
        ReduceGlassPrice,
        ReduceGlassPricePlus,
        BetterExecutiveHiring,
        BetterOpportunityFinding,
        BetterIntrigueFinding,
        ReduceSalaryExpenses,
        BetterBridgeBuilding,
        BetterTunnelBuilding,
        BetterBuildingBuilding
    }
}
