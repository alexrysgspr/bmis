
using System.ComponentModel.DataAnnotations;

namespace Bmis.Web.Controllers.Setup;
public class SetupViewModel
{
    public SetupViewModel()
    {
        Officials.AddRange(Enumerable.Range(1,10).Select(x => new OfficialViewModel()));
    }

    [Required]
    public string Name { get; set; }
    public List<OfficialViewModel> Officials { get; set; } = new();
    [Required]
    public string AdminUsername { get; set; }
    [Required]
    public string AdminPassword { get; set; }

    [Compare("AdminPassword", ErrorMessage = "The password and confirmation password do not match.")]
    public string AdminConfirmPassword { get; set; }
    public IFormFile Logo { get; set; }
}

public class OfficialViewModel
{
    [Required]
    public string Position { get; set; }
    [Required]
    public string Name { get; set; }
    public IFormFile Image { get; set; }
}
