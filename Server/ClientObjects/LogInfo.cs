using BuildingWebsite.Server.ServerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server.ClientObjects
{
    public class LogInfo
    {
        private Log BackingInfo { get; set; }

        public List<LogEntry> LogEntries => BackingInfo.LogEntries;

        public LogInfo(Log backingInfo)
        {
            BackingInfo = backingInfo;
        }
    }
}
