using BuildingWebsite.Server.ServerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server.ClientObjects
{
    public class ExecutiveInfo
    {
        private Executive BackingData { get; set; }

        public string Name => BackingData.Name;

        public string Text => BackingData.Text;

        public int Salary => BackingData.Salary;

        public bool ActionReady => BackingData.ActionReady;

        public ExecutiveTaskType CurrentTask => BackingData.CurrentTask;

        public ExecutiveAbility Ability => BackingData.Ability;

        public ExecutiveInfo(Executive backingData)
        {
            BackingData = backingData;
        }
    }

    public class ExecutiveInfoList
    {
        private Player BackingData { get; set; }

        public List<ExecutiveInfo> Executives => BackingData.Executives.Select(data => new ExecutiveInfo(data)).ToList();

        public List<ExecutiveTask> TaskList => ExecutiveTask.DefaultTasks;

        public ExecutiveInfoList(Player backingData)
        {
            BackingData = backingData;
        }
    }
}
