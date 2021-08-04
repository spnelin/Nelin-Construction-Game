using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server.ServerObjects
{
    public class Opportunity : ICard
    {
        public CardType CardType => CardType.Opportunity;

        public string Name { get; private set; }

        public OpportunityType Type { get; private set; }

        public string Description { get; private set; }

        public Opportunity(string name, OpportunityType type, string description)
        {
            Name = name;
            Type = type;
            Description = description;
        }
    }

    public class MaterialDealOpportunity : Opportunity
    {
        public MaterialType Material { get; private set; }

        public int MaxQuantity { get; private set; }

        public int Price { get; private set; }

        public MaterialDealOpportunity(string name, string description, MaterialType material, int price, int maxQuantity) : base(name, OpportunityType.MaterialDeal, description)
        {
            Material = material;
            MaxQuantity = maxQuantity;
            Price = price;
        }
    }

    public class PrivateProjectOpportunity : Opportunity
    {
        private Project ProjectTemplate { get; set; }

        public Project Project
        {
            get
            {
                return new Project(ProjectTemplate.Name, ProjectTemplate.Text, ProjectTemplate.TotalConcrete, ProjectTemplate.TotalSteel, ProjectTemplate.TotalGlass, ProjectTemplate.MaxWorkers, ProjectTemplate.MaxBid, ProjectTemplate.TurnsToComplete, ProjectTemplate.Type, ProjectTemplate.Tier, ProjectTemplate.Requirements, true);
            }
        }

        public PrivateProjectOpportunity(string name, string description, Project template) : base(name, OpportunityType.PrivateProject, description)
        {
            ProjectTemplate = template;
        }
    }

    public class ExecutiveHireOpportunity : Opportunity
    { 
        public int ExecutiveCount { get; private set; }

        public double SalaryAdjustment { get; private set; }

        public ExecutiveHireOpportunity(string name, string description, int executiveCount, double salaryAdjustment) : base(name, OpportunityType.ExecutiveHire, description)
        {
            ExecutiveCount = executiveCount;
            SalaryAdjustment = salaryAdjustment;
        }
    }

    public class MoneyWindfallOpportunity : Opportunity
    {
        public int Quantity { get; private set; }

        public MoneyWindfallOpportunity(string name, string description, int quantity) : base(name, OpportunityType.MoneyWindfall, description)
        {
            Quantity = quantity;
        }
    }

    public enum OpportunityType
    {
        MaterialDeal,
        PrivateProject,
        ExecutiveHire,
        MoneyWindfall,
        Overtime
    }
}
