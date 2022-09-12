namespace Bmis.Web
{
    public static class DataGenerator
    {
        public static string GenerateString()
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            

            //var addresses = await _context.Addresses.ToListAsync();

            //var resident = Enumerable.Range(0, 500).Select(x => new Resident
            //{
            //    IsPwd = x % 2 == 0,
            //    Disability = x % 2 == 0 ? DataGenerator.GenerateString() : null,
            //    Address = addresses[random.Next(0, 6)],
            //    Gender = Enum.GetValues<Gender>()[random.Next(0, 1)],
            //    Birthdate = DataGenerator.GenerateDateTime(),
            //    FirstName = DataGenerator.GenerateString(),
            //    CivilStatus = Enum.GetValues<CivilStatus>()[random.Next(0, 3)],
            //    ContactNo = DataGenerator.GenerateString(),
            //    Email = DataGenerator.GenerateString(),
            //    LastName = DataGenerator.GenerateString(),
            //    MiddleName = DataGenerator.GenerateString(),
            //    VoterStatus = Enum.GetValues<VoterStatus>()[random.Next(0, 1)]
            //});
            return new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static DateTime GenerateDateTime()
        {
            var rnd = new Random();
            var dateToday = DateTime.Now;

            var rndYear = rnd.Next(1995, dateToday.Year);
            var rndMonth = rnd.Next(1, 12);
            var rndDay = rnd.Next(1, 28);

            var generatedDate = new DateTime(rndYear, rndMonth, rndDay);

            return generatedDate;
        }
    }
}
