using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server.ServerObjects
{
    /// <summary>
    /// A simple log with a maximum number of lines. Do not manipulate LogEntries directly (could make it ReadOnly, but where's the fun in that?).
    /// </summary>
    public class Log
    {
        public List<LogEntry> LogEntries { get; private set; }

        public int MaxEntries { get; private set; }

        public void AddEntry(string entry)
        {
            LogEntries.Add(new LogEntry(entry));
            if (LogEntries.Count > MaxEntries)
            {
                LogEntries.RemoveAt(0);
            }
        }

        public Log(int maxEntries)
        {
            LogEntries = new List<LogEntry>();
            MaxEntries = maxEntries;
        }
    }

    public class LogEntry
    {
        public double Timestamp { get; private set; }

        public string Text { get; private set; }

        private static DateTime UnixStartDate = new DateTime(1970, 1, 1);

        public LogEntry(string text)
        {
            Timestamp = new TimeSpan(DateTime.UtcNow.Ticks - UnixStartDate.Ticks).TotalMilliseconds;
            Text = text;
        }
    }
}
