using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server.ServerObjects
{
    public class Chat
    {
        public int Id { get; private set; }

        public string Name { get; private set; }

        public List<Player> IncludedPlayers { get; private set; }

        public Log Log { get; private set; }

        public void AddEntry(Player player, string entry)
        {
            Log.AddEntry(player.User.Name + ": " + entry);
        }

        public void AddPlayer(Player player)
        {
            IncludedPlayers.Add(player);
        }

        public Chat(int id, string name, List<Player> players)
        {
            Id = id;
            IncludedPlayers = players;
            Log = new Log(50);
            Name = name;
            Log.AddEntry("This is the start of " + Name + "!");
        }
    }

    public class ChatList
    {
        public List<Chat> Chats { get; private set; }

        public ChatList()
        {
            Chats = new List<Chat>();
        }
    }
}
