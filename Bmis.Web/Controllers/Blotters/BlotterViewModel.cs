using Bmis.EntityFramework.Entities;
using Microsoft.Build.Framework;

namespace Bmis.Web.Controllers.Blotters
{
    public class BlotterViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Complainant { get; set; }
        [Required]
        public string Respondent { get; set; }
        [Required]
        public string Details { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string BlotterType { get; set; }
        public DateTime DateTime { get; set; }
        public BlotterStatus Status { get; set; }
    }

    public static class BlotterViewModelExtensions
    {
        public static BlotterViewModel ToViewModel(this Blotter entity)
        {
            return new BlotterViewModel
            {
                Complainant = entity.Complainant,
                BlotterType = entity.BlotterType,
                DateTime = entity.DateTime,
                Details = entity.Details,
                Location = entity.Location,
                Respondent = entity.Respondent,
                Status = entity.Status,
                Id = entity.Id
            };
        }

        public static Blotter ToBlotter(this BlotterViewModel model)
        {
            return new Blotter
            {
                Complainant = model.Complainant,
                BlotterType = model.BlotterType,
                DateTime = model.DateTime,
                Details = model.Details,
                Location = model.Location,
                Respondent = model.Respondent,
                Status = model.Status
            };
        }

        public static void ToUpdatedBlotter(this Blotter entity, BlotterViewModel model)
        {
            entity.Complainant = model.Complainant;
            entity.BlotterType = model.BlotterType;
            entity.DateTime = model.DateTime;
            entity.Details = model.Details;
            entity.Location = model.Location;
            entity.Respondent = model.Respondent;
        }
    }
}
