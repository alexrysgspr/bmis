namespace Bmis.EntityFramework.Entities
{
    public class Barangay
    {
        public int Id { get; set; }
        public string Logo { get; set; }
        public string Name { get; set; }
        public string Officials { get; set; }
        public string Host { get; set; }
        public ICollection<Resident> Residents { get; set; }
        public ICollection<Blotter> Blotters { get; set; }
        public ICollection<Address> Addresses { get; set; }
    }
}
