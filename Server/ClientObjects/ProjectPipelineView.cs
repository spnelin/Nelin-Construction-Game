using BuildingWebsite.Server.ServerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server.ClientObjects
{
    public class ProjectPipelineView
    {
        private ProjectPipeline BackingInfo { get; set; }

        private Player ViewingPlayer { get; set; }

        public List<ProjectView> BiddableProjects => BackingInfo.BiddableProjects().Select(a => new ProjectView(a, ViewingPlayer)).ToList();

        public List<ProjectView> Tier1Projects => BackingInfo.UpcomingProjects(ProjectTier.Tier1).Select((project, index) => new ProjectView(InterpretViewableProject(project, index), ViewingPlayer)).ToList();

        public List<ProjectView> Tier2Projects => BackingInfo.UpcomingProjects(ProjectTier.Tier2).Select((project, index) => new ProjectView(InterpretViewableProject(project, index), ViewingPlayer)).ToList();

        public List<ProjectView> Tier3Projects => BackingInfo.UpcomingProjects(ProjectTier.Tier3).Select((project, index) => new ProjectView(InterpretViewableProject(project, index), ViewingPlayer)).ToList();

        public int Tier1NextTurnNewProjectCount => BackingInfo.NextTurnNewProjectCount(ProjectTier.Tier1);

        public int Tier2NextTurnNewProjectCount => BackingInfo.NextTurnNewProjectCount(ProjectTier.Tier2);

        public int Tier3NextTurnNewProjectCount => BackingInfo.NextTurnNewProjectCount(ProjectTier.Tier3);

        private Project InterpretViewableProject(Project project, int index)
        {
            if (project == null)
            {
                return null;
            }
            if (index < 0)
            {
                return project;
            }
            return Project.HiddenProject;
        }

        public ProjectPipelineView(ProjectPipeline backingInfo, Player viewingPlayer)
        {
            BackingInfo = backingInfo;
            ViewingPlayer = viewingPlayer;
        }
    }
}
