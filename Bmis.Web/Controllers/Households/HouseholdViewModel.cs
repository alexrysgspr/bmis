using Bmis.Web.Controllers.Residents;

namespace Bmis.Web.Controllers.Households
{
    public class HouseHoldViewModel
    {
        public int AddressId { get; set; }
        public string Address { get; set; }
        public string Purok { get; set; }
        public int TotalMembers { get; set; }
        public int TotalMale { get; set; }
        public int TotalFemale { get; set; }
        public int TotalVoters { get; set; }
        public int TotalPwd { get; set; }
        public List<ResidentViewModel> Residents { get; set; }
    }

    public static class HouseHoldViewModelExtensions
    {
    }
}
