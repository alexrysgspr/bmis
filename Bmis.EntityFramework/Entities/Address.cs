namespace Bmis.EntityFramework.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string StreetAddress { get; set; }
        public string Purok { get; set; }
        public List<Resident> Residents { get; set; }
        public int BarangayId { get; set; }
        public Barangay Barangay { get; set; }
    }
}
