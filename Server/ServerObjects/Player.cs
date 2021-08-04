using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server.ServerObjects
{
    public class Player
    {
        public User User { get; private set; }

        public int Id { get; private set; }

        public bool Bankrupt { get; private set; }

        public List<Executive> Executives { get; private set; }

        public List<Project> CurrentProjects { get; private set; }

        public List<Project> CompletedProjects { get; private set; }

        public CardHand Hand { get; private set; }

        public bool IsReady { get; private set; }

        public int Money { get; private set; }

        public int TotalWorkers { get; private set; }

        public int FreeWorkers { get; private set; }

        public int Concrete { get; private set; }

        public int Steel { get; private set; }

        public int Glass { get; private set; }

        public bool BidThisTurn { get; private set; }

        public void AddProject(Project project, bool notBid = false)
        {
            CurrentProjects.Add(project);
            if (!notBid)
            {
                BidThisTurn = true;
            }
        }

        public void CompleteProject(Project project)
        {
            if (CurrentProjects.Remove(project))
            {
                CompletedProjects.Add(project);
            }
        }

        public void SetReadiness(bool readyState)
        {
            IsReady = readyState;
        }

        public void AdjustMoney(int adjustment, bool subtract = false)
        {
            if (subtract)
            {
                Money -= adjustment;
            }
            else
            {
                Money += adjustment;
            }
            if (Money < 0)
            {
                Money = 0;
            }
        }

        public void AdjustConcrete(int adjustment, bool subtract = false)
        {
            if (subtract)
            {
                Concrete -= adjustment;
            }
            else
            {
                Concrete += adjustment;
            }
            if (Concrete < 0)
            {
                Concrete = 0;
            }
        }

        public void AdjustSteel(int adjustment, bool subtract = false)
        {
            if (subtract)
            {
                Steel -= adjustment;
            }
            else
            {
                Steel += adjustment;
            }
            if (Steel < 0)
            {
                Steel = 0;
            }
        }

        public void AdjustGlass(int adjustment, bool subtract = false)
        {
            if (subtract)
            {
                Glass -= adjustment;
            }
            else
            {
                Glass += adjustment;
            }
            if (Glass < 0)
            {
                Glass = 0;
            }
        }

        public void AdjustTotalWorkers(int adjustment, bool subtract = false)
        {
            if (subtract)
            {
                TotalWorkers -= adjustment;
            }
            else
            {
                TotalWorkers += adjustment;
            }
            if (TotalWorkers < 0)
            {
                TotalWorkers = 0;
            }
            if (FreeWorkers > TotalWorkers)
            {
                FreeWorkers = TotalWorkers;
            }
        }

        public void AdjustFreeWorkers(int adjustment, bool subtract = false)
        {
            if (subtract)
            {
                FreeWorkers -= adjustment;
            }
            else
            {
                FreeWorkers += adjustment;
            }
            if (FreeWorkers < 0)
            {
                FreeWorkers = 0;
            }
        }

        public void AddExecutive(Executive executive)
        {
            Executives.Add(executive);
        }

        public void ResetForTurn()
        {
            ResetEmployees();
            ClearProjectWorkers();
            ClearBidStatus();
        }

        /// <summary>
        /// Resets all employees - workers and executives.
        /// </summary>
        public void ResetEmployees()
        {
            FreeWorkers = TotalWorkers;
            foreach (Executive executive in Executives)
            {
                executive.ResetAction();
            }
        }

        public void ClearProjectWorkers()
        {
            foreach (Project proj in CurrentProjects)
            {
                proj.ResetWorkers();
            }
        }

        public void ClearBidStatus()
        {
            BidThisTurn = false;
        }

        public Player(User user, int id)
        {
            User = user;
            Id = id;
            CompletedProjects = new List<Project>();
            ReadyPlayer();
        }

        public void GoBankrupt()
        {
            Bankrupt = true;
        }

        public void ResolveBankruptcy(bool keepProjects)
        {
            ReadyPlayer();
            if (!keepProjects)
            {
                CompletedProjects = new List<Project>();
            }
        }

        private void ReadyPlayer()
        {
            Money = 500000000;
            IsReady = false;
            Bankrupt = false;
            Executives = new List<Executive>();
            Executives.Add(new Executive("CEO", "This is you - an executive willing to work for no pay!", 0, ExecutiveAbility.None, true));
            CurrentProjects = new List<Project>();
            Hand = new CardHand(this);
            TotalWorkers = 2;
            FreeWorkers = 2;
        }
    }

    public enum MaterialType
    {
        Concrete,
        Steel,
        Glass
    }
}
