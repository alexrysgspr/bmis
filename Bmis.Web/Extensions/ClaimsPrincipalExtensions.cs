using System.Security.Claims;
using Bmis.Web.Models;

namespace Bmis.Web.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetBarangayId(this ClaimsPrincipal principal)
    {
        var claimsPrincipal = principal;
        var barangay = claimsPrincipal?.Claims.FirstOrDefault(c => c.Type == Claims.Barangay);
        
        return int.Parse(barangay?.Value);
    }

    public static string GetSubjectIdSafe(this ClaimsPrincipal principal)
    {
        var claim = principal.FindFirst("sub");

        return claim?.Value;
    }
}
