using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server.ServerObjects
{
    public class ExecutiveTask
    {
        public string Name { get; private set; }

        public string Text { get; private set; }

        public ExecutiveTaskType Type { get; private set; }

        public static List<ExecutiveTask> DefaultTasks { get; private set; }

        static ExecutiveTask()
        {
            DefaultTasks = new List<ExecutiveTask>();
            DefaultTasks.Add(new ExecutiveTask("Buy Materials", "Assign this executive to buy construction materials off the general market. (This unlocks material purchases for the rest of the turn.)", ExecutiveTaskType.BuyMaterials));
            DefaultTasks.Add(new ExecutiveTask("Hire Executive", "Send this executive out to scout and hire a new executive for your company.", ExecutiveTaskType.HireExecutive));
            DefaultTasks.Add(new ExecutiveTask("Hire Workers", "Have this executive hire some additional construction workers for your company. (This unlocks worker hiring for the rest of the turn.)", ExecutiveTaskType.HireWorkers));
            DefaultTasks.Add(new ExecutiveTask("Find Opportunities", "Task this executive with finding good business opportunities.", ExecutiveTaskType.FindOpportunity));
            DefaultTasks.Add(new ExecutiveTask("Engage in Skulduggery", "Unofficially assign this executive out to find clandestine opportunities.", ExecutiveTaskType.FindSkulduggery));
        }

        public ExecutiveTask(string name, string text, ExecutiveTaskType type)
        {
            Name = name;
            Text = text;
            Type = type;
        }
    }

    public enum ExecutiveTaskType
    {
        BuyMaterials,
        HireExecutive,
        HireWorkers,
        FindOpportunity,
        FindSkulduggery,
        None,
    }
}
