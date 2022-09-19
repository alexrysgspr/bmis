namespace Bmis.EntityFramework.Entities;

public class Barangay
{
    public int Id { get; set; }
    public string Logo { get; set; }
    public string Name { get; set; }
    public string Officials { get; set; }
    public string Host { get; set; }
    public string ContactNo { get; set; }
    public string Municipality { get; set; }
    public string Province { get; set; }
    public string ZipCode { get; set; }
    public ICollection<Resident> Residents { get; set; }
    public ICollection<Blotter> Blotters { get; set; }
    public ICollection<Address> Addresses { get; set; }
}

public class Official
{
    public string Position { get; set; }
    public string Image { get; set; }
    public string Name { get; set; }
}