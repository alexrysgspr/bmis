namespace Bmis.Web.Extensions
{
    public static class BoolExtensions
    {
        public static string ToYesOrNo(this bool boolean)
        {
            return boolean ? "Yes" : "No";
        }
    }
}
