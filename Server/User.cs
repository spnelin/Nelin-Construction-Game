using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server
{
    public class User
    {
        public string Name { get; private set; }

        public string SessionId
        {
            get
            {
                return _sessionId;
            }
            set
            {
                if (_sessionId != string.Empty)
                {
                    Sessions.Remove(_sessionId);
                }
                _sessionId = value;
                if (value != string.Empty)
                {
                    Sessions[value] = this;
                }
            }
        }

        private string _sessionId = string.Empty;

        public static Dictionary<string, User> Users { get; private set; }

        public static Dictionary<string, User> Sessions { get; private set; }

        static User()
        {
            Users = new Dictionary<string, User>();
            Sessions = new Dictionary<string, User>();
            new User("TestPlayer");
            new User("TestPlayer2");
        }

        public static string GetSession()
        {
            return Guid.NewGuid().ToString(); //let's not protect against guid collision lol
        }

        private User(string name)
        {
            Name = name;
            Users[name] = this;
        }

        public static User CreatePlayer(string name)
        {
            if (Users.ContainsKey(name))
            {
                return null;
            }
            return new User(name);
        }
    }
}
