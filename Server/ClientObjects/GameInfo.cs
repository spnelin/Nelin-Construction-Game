using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server.ClientObjects
{
    public class GameInfo
    {
        private Game BackingInfo { get; set; }

        public int TurnNumber => BackingInfo.TurnNumber;

        public GameSettings Settings => BackingInfo.Settings;

        public GameState CurrentState => BackingInfo.CurrentState;

        public GameInfo(Game backingInfo)
        {
            BackingInfo = backingInfo;
        }
    }
}
