namespace Bmis.Web.Models;

public static class Roles
{
    public static string SuperAdmin = nameof(SuperAdmin);
    public static string Admin = nameof(Admin);

    public static string[] All = { SuperAdmin, Admin };
}
