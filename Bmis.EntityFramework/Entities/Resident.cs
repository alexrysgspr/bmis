﻿namespace Bmis.EntityFramework.Entities
{
    public class Resident
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Extension { get; set; }
        public DateTime Birthdate { get; set; }
        public CivilStatus CivilStatus { get; set; }
        public VoterStatus VoterStatus { get; set; }
        public Gender Gender { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public bool IsPwd { get; set; }
        public string Disability { get; set; }
        public Address Address { get; set; }
        public int AddressId { get; set; }
    }

}