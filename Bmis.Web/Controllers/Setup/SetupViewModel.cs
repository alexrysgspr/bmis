
namespace Bmis.Web.Controllers.Setup;
public class SetupViewModel
{
    public string Name { get; set; }
    public string Logo { get; set; }
    public List<Official> Officials { get; set; }
    public string AdminUsername { get; set; }
    public string AdminPassword { get; set; }
    public string AdminConfirmPassword { get; set; }
}

public class Official
{
    public string Position { get; set; }
    public string Name { get; set; }
}
