using System.ComponentModel.DataAnnotations;
using Bmis.EntityFramework.Entities;

namespace Bmis.Web.Controllers.Residents
{
    public class ResidentViewModel
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Extension { get; set; }
        public IFormFile Image { get; set; }
        public string ImageUrl { get; set; }
        public DateTime Birthdate { get; set; }
        [Required]
        public CivilStatus CivilStatus { get; set; }
        [Required]
        public VoterStatus VoterStatus { get; set; }
        [Required]
        public Gender Gender { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        [Required]
        public string StreetAddress { get; set; }
        public int? AddressId { get; set; }
        [Required]
        public string Purok { get; set; }
        public bool IsPwd { get; set; }
        public string Disability { get; set; }

        public int GetAge()
        {
            var today = DateTime.Today;
            var a = (today.Year * 100 + today.Month) * 100 + today.Day;
            var b = (Birthdate.Year * 100 + Birthdate.Month) * 100 + Birthdate.Day;

            return (a - b) / 10000;
        }

        public string GetFullName()
        {
            var name = $"{FirstName} {MiddleName} {LastName}";

            if (!string.IsNullOrEmpty(Extension))
            {
                name += ($" {Extension}");
            }

            return name;
        }
    }

    public static class ResidentViewModelExtensions
    {
        public static ResidentViewModel ToViewModel(this Resident entity)
        {
            return new ResidentViewModel
            {
                Id = entity.Id,
                StreetAddress = $"{entity.Address.StreetAddress}",
                Purok = entity.Address.Purok,
                Birthdate = entity.Birthdate,
                CivilStatus = entity.CivilStatus,
                ContactNo = entity.ContactNo,
                Email = entity.Email,
                Extension = entity.Extension,
                FirstName = entity.FirstName,
                Gender = entity.Gender,
                LastName = entity.LastName,
                MiddleName = entity.MiddleName,
                VoterStatus = entity.VoterStatus,
                IsPwd = entity.IsPwd,
                Disability = entity.Disability,
                AddressId = entity.AddressId,
                ImageUrl = entity.ImageUrl
            };
        }

        public static Resident ToResident(this ResidentViewModel model, string fileName = null)
        {
            var resident = new Resident
            {
                Birthdate = model.Birthdate,
                CivilStatus = model.CivilStatus,
                ContactNo = model.ContactNo,
                Email = model.Email,
                Extension = model.Extension,
                FirstName = model.FirstName,
                Gender = model.Gender,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                VoterStatus = model.VoterStatus,
                IsPwd = model.IsPwd,
                Disability = model.Disability,
                ImageUrl = fileName
            };

            if (model.AddressId > 0)
            {
                resident.AddressId = model.AddressId.Value;
            }
            else
            {
                resident.Address = new Address
                {
                    StreetAddress = model.StreetAddress,
                    Purok = model.Purok
                };
            }

            return resident;
        }

        public static void ToUpdatedResident(this Resident entity, ResidentViewModel model)
        {
            entity.Birthdate = model.Birthdate;
            entity.CivilStatus = model.CivilStatus;
            entity.ContactNo = model.ContactNo;
            entity.Email = model.Email;
            entity.Extension = model.Extension;
            entity.FirstName = model.FirstName;
            entity.Gender = model.Gender;
            entity.LastName = model.LastName;
            entity.MiddleName = model.MiddleName;
            entity.VoterStatus = model.VoterStatus;
            entity.IsPwd = model.IsPwd;
            entity.Disability = model.Disability;

            if (model.AddressId > 0)
            {
                entity.AddressId = model.AddressId.Value;
            }
            else
            {
                entity.Address = new Address
                {
                    StreetAddress = model.StreetAddress,
                    Purok = model.Purok
                };
            }
        }
    }
}
