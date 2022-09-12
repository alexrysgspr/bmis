namespace Bmis.EntityFramework.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string AddressLine { get; set; }
        public string Purok { get; set; }
        public List<Resident> Residents { get; set; }
    }
}
