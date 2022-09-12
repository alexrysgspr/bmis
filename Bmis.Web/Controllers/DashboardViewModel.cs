namespace Bmis.Web.Controllers
{
    public class DashboardViewModel
    {
        public int TotalResidents { get; set; }
        public int TotalActiveVoters { get; set; }
        public int TotalPwd { get; set; }

        public List<PurokPopulation> PurokPopulations { get; set; } = new List<PurokPopulation>();

        public List<PopulationClassification> PopulationClassifications { get; set; } = new List<PopulationClassification>();
    }

    public class PurokPopulation
    {
        public string Purok { get; set; }
        public int Total { get; set; }
    }

    public class PopulationClassification
    {
        public string Key { get; set; }
        public int Total { get; set; }
    }
}
