namespace Bmis.EntityFramework.Entities
{
    public class Blotter
    {
        public int Id { get; set; }
        public string Complainant { get; set; }
        public string Respondent { get; set; }
        public string Details { get; set; }
        public string Location { get; set; }
        public string BlotterType { get; set; }
        public DateTime DateTime { get; set; }
        public BlotterStatus Status { get; set; }
    }
}
