using Microsoft.AspNetCore.Identity;

namespace Bmis.EntityFramework.Entities;

public class ApplicationUser : IdentityUser
{
    public int? BarangayId { get; set; }
    public Barangay Barangay { get; set; }
}