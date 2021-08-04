using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server.ServerObjects
{
    public class ProjectPipeline
    {
        private TierPipeline Tier1Pipeline;

        private TierPipeline Tier2Pipeline;

        private TierPipeline Tier3Pipeline;

        //Todo: make this less hardcoded per tier
        public ProjectPipeline(List<Project> tier1, List<Project> tier2, List<Project> tier3)
        {
            Tier1Pipeline = new TierPipeline(tier1, 15, 4, 3);
            Tier2Pipeline = new TierPipeline(tier2, 7, 3, 3);
            Tier3Pipeline = new TierPipeline(tier3, 3, 2, 3);
        }

        public List<Project> BiddableProjects()
        {
            List<Project> ret = new List<Project>();
            ret.AddRange(Tier1Pipeline.BiddableProjects);
            ret.AddRange(Tier2Pipeline.BiddableProjects);
            ret.AddRange(Tier3Pipeline.BiddableProjects);
            return ret;
        }

        public List<Project> UpcomingProjects(ProjectTier tier)
        {
            switch (tier)
            {
                case ProjectTier.Tier1:
                    return Tier1Pipeline.UpcomingProjects.ToList();
                case ProjectTier.Tier2:
                    return Tier2Pipeline.UpcomingProjects.ToList();
                case ProjectTier.Tier3:
                    return Tier3Pipeline.UpcomingProjects.ToList();
                default:
                    throw new ArgumentException("Unhandled tier type. Could not retrieve upcoming projects.");
            }
        }

        public void MoveNextTurn(int currentTurn)
        {
            Tier1Pipeline.FillBiddableProjects(currentTurn);
            Tier2Pipeline.FillBiddableProjects(currentTurn);
            Tier3Pipeline.FillBiddableProjects(currentTurn);
        }

        public void RemoveProject(Project project)
        {
            switch (project.Tier)
            {
                case ProjectTier.Tier1:
                    Tier1Pipeline.TakeProject(project);
                    break;
                case ProjectTier.Tier2:
                    Tier2Pipeline.TakeProject(project);
                    break;
                case ProjectTier.Tier3:
                    Tier3Pipeline.TakeProject(project);
                    break;
                default:
                    throw new ArgumentException("Project " + project + " has an unhandled tier type.");
            }
        }

        public void DiscardProject(Project project)
        {
            switch (project.Tier)
            {
                case ProjectTier.Tier1:
                    Tier1Pipeline.DiscardProject(project);
                    break;
                case ProjectTier.Tier2:
                    Tier2Pipeline.DiscardProject(project);
                    break;
                case ProjectTier.Tier3:
                    Tier3Pipeline.DiscardProject(project);
                    break;
                default:
                    throw new ArgumentException("Project " + project + " has an unhandled tier type.");
            }
        }

        public int NextTurnNewProjectCount(ProjectTier tier)
        {
            switch (tier)
            {
                case ProjectTier.Tier1:
                    return Tier1Pipeline.NextTurnNewProjectCount();
                case ProjectTier.Tier2:
                    return Tier2Pipeline.NextTurnNewProjectCount();
                case ProjectTier.Tier3:
                    return Tier3Pipeline.NextTurnNewProjectCount();
                default:
                    throw new ArgumentException("Unhandled tier type. Could not retrieve upcoming projects.");
            }
        }

        private class TierPipeline
        {
            public List<Project> ProjectDeck { get; private set; }

            public List<Project> DiscardedProjects { get; private set; }

            public List<Project> BiddableProjects { get; private set; }

            public Queue<Project> UpcomingProjects { get; private set; }

            public int NewProjectFrequency { get; private set; }

            public int MaxBiddableProjects { get; private set; }

            public int MaxUpcomingProjects { get; private set; }

            private int Accumulation { get; set; }

            private Random rand = new Random();

            public TierPipeline(List<Project> projectDeck, int newProjectFrequency, int maxBiddableProjects, int maxUpcomingProjects)
            {
                ProjectDeck = projectDeck;
                NewProjectFrequency = newProjectFrequency;
                MaxBiddableProjects = maxBiddableProjects;
                MaxUpcomingProjects = maxUpcomingProjects;
                DiscardedProjects = new List<Project>();
                BiddableProjects = new List<Project>();
                UpcomingProjects = new Queue<Project>();
                //Fill out Upcoming Projects and Biddable Projects - order matters so that we have Upcoming Projects to draw on for the Biddables
                EnqueueUpcomingProjects();
                FillBiddableProjects(0);
            }

            private void EnqueueUpcomingProjects()
            {
                while (UpcomingProjects.Count < MaxUpcomingProjects)
                {
                    //Reshuffle discarded projects into the ProjectDeck
                    if (ProjectDeck.Count == 0)
                    {
                        if (DiscardedProjects.Count == 0)
                        {
                            //If we don't have anything in the deck or in discard, then don't try to enqueue anything
                            break;
                        }
                        ProjectDeck.AddRange(DiscardedProjects);
                        DiscardedProjects.Clear();
                    }
                    //Add new project to Upcoming Projects
                    int index = rand.Next(0, ProjectDeck.Count);
                    UpcomingProjects.Enqueue(ProjectDeck[index]);
                    ProjectDeck.RemoveAt(index);
                }
            }

            public void FillBiddableProjects(int currentTurn)
            {
                //Add to Accumulation, and then enqueue a project for each 10 that can be evenly subtracted out of it up to the max project number
                Accumulation += NewProjectFrequency;
                while (Accumulation > 10 && BiddableProjects.Count < MaxBiddableProjects)
                {
                    //If we have any upcoming projects to add to the biddables, do so
                    if (UpcomingProjects.Count > 0)
                    {
                        Project project = UpcomingProjects.Dequeue();
                        project.SetDueByTurn(currentTurn);
                        BiddableProjects.Add(project);
                    }
                    Accumulation -= 10;
                }
                //Cap accumulation between rounds to 10 - we don't want to have massive queue spillover
                if (Accumulation > 10)
                {
                    Accumulation = 10;
                }
                //Enqueue further upcoming projects so nothing's ever missing
                EnqueueUpcomingProjects();
            }

            public int NextTurnNewProjectCount()
            {
                return Accumulation + NewProjectFrequency / 10;
            }

            public void TakeProject(Project project)
            {
                if (BiddableProjects.Contains(project))
                {
                    BiddableProjects.Remove(project);
                }
                else
                {
                    throw new InvalidOperationException("Project " + project.Name + " does not exist in this project pipeline.");
                }
            }

            public void DiscardProject(Project project)
            {
                DiscardedProjects.Add(project);
            }
        }
    }
}