using BuildingWebsite.Server.ServerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server.ClientObjects
{
    /// <summary>
    /// Interface to ship player data to the client as JSON.
    /// </summary>
    public class PlayerGenericInfo
    {
        private Player BackingData { get; set; }

        public bool IsReadyToAdvance => BackingData.IsReady;

        public string Name => BackingData.User.Name;

        public int Id => BackingData.Id;

        public int Money => BackingData.Money;

        public int FreeWorkers => BackingData.FreeWorkers;

        public int TotalWorkers => BackingData.TotalWorkers;

        public int Concrete => BackingData.Concrete;

        public int Steel => BackingData.Steel;

        public int Glass => BackingData.Glass;

        public List<ProjectView> CurrentProjects => BackingData.CurrentProjects.Select(a => new ProjectView(a, BackingData)).ToList();

        public List<ProjectView> CompletedProjects => BackingData.CompletedProjects.Select(a => new ProjectView(a, BackingData)).ToList();

        public bool BuyMaterialsUnlocked => BackingData.Executives.Exists(e => e.CurrentTask == ExecutiveTaskType.BuyMaterials);

        public bool HireWorkersUnlocked => BackingData.Executives.Exists(e => e.CurrentTask == ExecutiveTaskType.HireWorkers);

        public PlayerGenericInfo(Player backingData)
        {
            BackingData = backingData;
        }
    }

    public class PlayerGenericInfoList
    {
        private List<Player> BackingData { get; set; }

        public List<PlayerGenericInfo> OtherPlayers { get; private set; }

        public PlayerGenericInfo OwningPlayer { get; private set; }

        public PlayerGenericInfoList(Game game, Player player)
        {
            BackingData = new List<Player>(game.PlayerList);
            if (player != null)
            {
                BackingData.Remove(player);
            }
            OtherPlayers = BackingData.Select(p => new PlayerGenericInfo(p)).ToList();
            game.AddPlayer += AddPlayerHandler;
            game.RemovePlayer += RemovePlayerHandler;
            OwningPlayer = new PlayerGenericInfo(player);
        }

        private void AddPlayerHandler(Player player)
        {
            BackingData.Add(player);
            OtherPlayers = BackingData.Select(p => new PlayerGenericInfo(p)).ToList();
        }

        private void RemovePlayerHandler(Player player)
        {
            BackingData.Remove(player);
            OtherPlayers = BackingData.Select(p => new PlayerGenericInfo(p)).ToList();
        }
    }

    public class PlayerMinimalInfo
    {
        private Player BackingData { get; set; }

        public string Name => BackingData.User.Name;

        public int Id => BackingData.Id;

        public PlayerMinimalInfo(Player backingData)
        {
            BackingData = backingData;
        }
    }
}
