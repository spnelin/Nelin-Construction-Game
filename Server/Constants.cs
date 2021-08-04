using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server
{
    public class Constants
    {
        //cookie names
        public const string COOKIE_LAST_REV = "LastRevision";
        public const string COOKIE_SESSION_ID = "SessionId";

        //object identifiers
        //server-to-client
        public const string OBJECT_GAME = "GAME";
        public const string OBJECT_CHAT = "CHAT";
        public const string OBJECT_HAND = "HAND";
        public const string OBJECT_PLAYER_INFO = "PLAYER_INFO";
        public const string OBJECT_EXEC_INFO = "EXEC_INFO";
        public const string OBJECT_GAME_LOG = "GAME_LOG";
        public const string OBJECT_PROJECT_PIPELINE = "PROJECT_PIPELINE";
        public const string OBJECT_BID_SESSION = "BID_SESSION";
        public const string OBJECT_MODAL = "MODAL";

        //client-only
        public const string OBJECT_OTHER_PLAYERS = "OTHER_PLAYERS";
        public const string OBJECT_CURRENT_PLAYER = "CURRENT_PLAYER";
        public const string OBJECT_CURRENT_PROJECTS = "CURRENT_PROJECTS";

        //misc
        public const string OBJECT_UNIDENTIFIED = "UNKNOWN";
    }
}
