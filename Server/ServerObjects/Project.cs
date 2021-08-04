using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server.ServerObjects
{
    public class Project
    {
        public string Name { get; private set; }

        public string Text { get; private set; }

        public int CurrentConcrete { get; private set; }

        public int TotalConcrete { get; private set; }

        public int CurrentSteel { get; private set; }

        public int TotalSteel { get; private set; }

        public int CurrentGlass { get; private set; }

        public int TotalGlass { get; private set; }

        public int WorkersThisTurn { get; private set; }

        public int MaxWorkers { get; private set; }

        public int MaxBid { get; private set; }

        public int Price { get; private set; }

        public int TurnsToComplete { get; private set; }

        public int TurnDueBy { get; private set; }

        public ProjectType Type { get; private set; }

        public ProjectTier Tier { get; private set; }

        public bool Completed { get; private set; }

        public bool IsPrivate { get; private set; }

        public List<ProjectRequirement> Requirements { get; private set; }

        //This is a very sweet and trusting method, please validate its input elsewhere
        public bool AddMaterialsToProjectAndCheckIfCompleted(int concrete, int steel, int glass)
        {
            CurrentConcrete += concrete;
            CurrentSteel += steel;
            CurrentGlass += glass;
            WorkersThisTurn += concrete + steel + glass;
            if (CurrentConcrete == TotalConcrete && CurrentSteel == TotalSteel && CurrentGlass == TotalGlass)
            {
                Completed = true;
                return true;
            }
            return false;
        }

        public bool PlayerQualifiedForProject(Player player)
        {
            bool ret = true;
            foreach (ProjectRequirement req in Requirements)
            {
                if (req.Quantity > player.CompletedProjects.Where(proj => proj.Tier == req.Tier && proj.Type == req.Type).Count())
                {
                    ret = false;
                }
            }
            return ret;
        }

        public void SetDueByTurn(int currentTurn)
        {
            TurnDueBy = currentTurn + TurnsToComplete;
        }

        public void SetPrice(int price)
        {
            Price = price;
        }

        public void AdjustMaxBid(int maxBid)
        {
            MaxBid = maxBid;
        }

        public void ResetWorkers()
        {
            WorkersThisTurn = 0;
        }

        public Project(string name, string text, int concrete, int steel, int glass, int maxWorkers, int maxBid, int turnsToBuild, ProjectType type, ProjectTier tier, List<ProjectRequirement> requirements, bool isPrivate = false)
        {
            Name = name;
            Text = text;
            TotalConcrete = concrete;
            TotalSteel = steel;
            TotalGlass = glass;
            MaxWorkers = maxWorkers;
            MaxBid = maxBid;
            TurnsToComplete = turnsToBuild;
            Type = type;
            Tier = tier;
            Requirements = requirements;
            Completed = false;
            IsPrivate = isPrivate;
        }

        public static Project HiddenProject = new Project("Unknown", "An unknown project, to be revealed later.", 0, 0, 0, 0, 0, 0, ProjectType.Unknown, ProjectTier.Unknown, new List<ProjectRequirement>());
    }

    public class ProjectRequirement
    {
        public ProjectType Type { get; private set; }

        public ProjectTier Tier { get; private set; }

        public int Quantity { get; private set; }

        public ProjectRequirement(ProjectType type, ProjectTier tier, int quantity)
        {
            Type = type;
            Tier = tier;
            Quantity = quantity;
        }
    }

    public enum ProjectType
    {
        Building,
        Bridge,
        Tunnel,
        Unknown = 999
    }

    public enum ProjectTier
    {
        Tier1,
        Tier2,
        Tier3,
        Unknown = 999
    }
}
