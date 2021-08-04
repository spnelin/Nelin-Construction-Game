using BuildingWebsite.Server.ServerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server.ClientObjects
{
    public class ProjectView
    {
        private Project BackingInfo { get; set; }

        private Player ViewingPlayer { get; set; }

        public string Name => BackingInfo.Name;

        public string Text => BackingInfo.Text;

        public int CurrentConcrete => BackingInfo.CurrentConcrete;

        public int TotalConcrete => BackingInfo.TotalConcrete;

        public int CurrentSteel => BackingInfo.CurrentSteel;

        public int TotalSteel => BackingInfo.TotalSteel;

        public int CurrentGlass => BackingInfo.CurrentGlass;

        public int TotalGlass => BackingInfo.TotalGlass;

        public int WorkersThisTurn => BackingInfo.WorkersThisTurn;

        public int MaxWorkers => BackingInfo.MaxWorkers;

        public int MaxBid => BackingInfo.MaxBid;

        public int Price => BackingInfo.Price;

        public int TurnsToComplete => BackingInfo.TurnsToComplete;

        public int TurnDueBy => BackingInfo.TurnDueBy;

        public ProjectType Type => BackingInfo.Type;

        public ProjectTier Tier => BackingInfo.Tier;

        public bool Completed => BackingInfo.Completed;

        public bool IsPrivate => BackingInfo.IsPrivate;

        public List<ProjectRequirement> Requirements => BackingInfo.Requirements;

        public bool CanBid => BackingInfo.PlayerQualifiedForProject(ViewingPlayer);

        public ProjectView(Project backingInfo, Player viewingPlayer)
        {
            if (backingInfo == null)
            {
                throw new Exception();
            }
            BackingInfo = backingInfo;
            ViewingPlayer = viewingPlayer;
        }
    }
}
