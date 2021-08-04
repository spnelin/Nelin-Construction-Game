using BuildingWebsite.Server.ServerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server.ClientObjects
{
    public class NoModal : IModal
    {
        public ModalType Type => ModalType.None;
    }

    public class CostReportModal : IModal
    {
        public ModalType Type => ModalType.CostReport;

        public int WorkerSalary { get; private set; }

        public int ExecutiveSalary { get; private set; }

        public CostReportModal(int workerSalary, int executiveSalary)
        {
            WorkerSalary = workerSalary;
            ExecutiveSalary = executiveSalary;
        }
    }

    public class ProjectCompletionModal : IModal
    {
        public ModalType Type => ModalType.ProjectCompletion;

        public Project Project { get; private set; }

        public ProjectCompletionModal(Project project)
        {
            Project = project;
        }
    }

    public class HireExecutiveModal : IModal
    {
        public ModalType Type => ModalType.ExecHire;

        public List<Executive> Executives { get; private set; }

        public HireExecutiveModal(List<Executive> executives)
        {
            Executives = executives;
        }
    }

    public class OpportunitySearchModal : IModal
    {
        public ModalType Type => ModalType.OpportunitySearch;

        public List<Opportunity> Opportunities { get; private set; }

        public OpportunitySearchModal(List<Opportunity> opportunities)
        {
            Opportunities = opportunities;
        }
    }

    public class IntrigueSearchModal : IModal
    {
        public ModalType Type => ModalType.IntrigueSearch;

        public List<Intrigue> Intrigues { get; private set; }

        public IntrigueSearchModal(List<Intrigue> intrigues)
        {
            Intrigues = intrigues;
        }
    }

    public class MaterialDealModal : IModal
    {
        public ModalType Type => ModalType.MaterialDeal;

        public MaterialType Material { get; private set; }
        
        public int Price { get; private set; }

        public int MaxQuantity { get; private set; }

        public int PlayerHandIndex { get; private set; }

        public MaterialDealModal(MaterialType material, int price, int maxQuantity, int playerHandIndex)
        {
            Material = material;
            Price = price;
            MaxQuantity = maxQuantity;
            PlayerHandIndex = playerHandIndex;
        }
    }

    public interface IModal
    {
        ModalType Type { get; }
    }

    public enum ModalType
    {
        None,
        CostReport,
        ExecHire,
        OpportunitySearch,
        IntrigueSearch,
        ProjectCompletion,
        MaterialDeal
    }
}
