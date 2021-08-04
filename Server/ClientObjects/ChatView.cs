using BuildingWebsite.Server.ServerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server.ClientObjects
{
    public class ChatView
    {
        private Chat BackingInfo { get; set; }

        public Log Log => BackingInfo.Log;

        public string Name => BackingInfo.Name;

        public int Id => BackingInfo.Id;

        public List<PlayerMinimalInfo> Players => BackingInfo.IncludedPlayers.Select(p => new PlayerMinimalInfo(p)).ToList();

        public ChatView(Chat backingInfo)
        {
            BackingInfo = backingInfo;
        }
    }

    public class ChatListView
    {
        private ChatList BackingInfo { get; set; }

        private Player OwningPlayer { get; set; }

        public List<ChatView> Chats => BackingInfo.Chats.Where(chat => chat.IncludedPlayers.Contains(OwningPlayer)).Select(chat => new ChatView(chat)).ToList();

        public ChatListView(ChatList backingInfo, Player player)
        {
            BackingInfo = backingInfo;
            OwningPlayer = player;
        }
    }
}
