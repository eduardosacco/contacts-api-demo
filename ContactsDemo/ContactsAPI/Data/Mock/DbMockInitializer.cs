using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ContactsAPI.Models;

namespace ContactsAPI.Data
{
    public static class DbMockInitializer
    {
        public static void Initialize(ContactRecordsContext context)
        {
            context.Database.EnsureCreated();

            if (context.ContactRecords.Any())
            {
                return;
            }

            //Add mock contact records to context
            context.ContactRecords.AddRange(
                new ContactRecord
                {
                    Id = 1,
                    Name = "Dale",
                    LastName = "Cooper",
                    Company = "JavaCoffee",
                    EmailAddress = "dale.cooper@javacoffee.com",
                    BirthDate = DateTime.Parse("1982-08-22"),
                    PhoneNumberPersonal = "3444555",
                    PhoneNumberWork = "3441255",
                    Country = "USA",
                    State = "Washington",
                    City = "Snoqualmie",
                    Address = "Sycamore St 144",
                },
                new ContactRecord
                {
                    Id = 2,
                    Name = "Norma",
                    LastName = "jennings",
                    Company = "R.R. Dinner",
                    EmailAddress = "norma@rrdiner.com",
                    BirthDate = DateTime.Parse("1980-12-4"),
                    PhoneNumberPersonal = "3186405",
                    PhoneNumberWork = "3153729",
                    Country = "USA",
                    State = "Washington",
                    City = "Snoqualmie",
                    Address = "Weyland St 243",
                },
                new ContactRecord
                {
                    Id = 3,
                    Name = "Robert",
                    LastName = "Briggs",
                    Company = "Bopper Car Service",
                    EmailAddress = "bopper@boppercar.com",
                    BirthDate = DateTime.Parse("1990-03-12"),
                    PhoneNumberPersonal = "4698765",
                    PhoneNumberWork = "4689723",
                    Country = "USA",
                    State = "Oregon",
                    City = "Portland",
                    Address = "Acacia St 720",
                }
            );

            context.SaveChanges();

            //Add mock profile pics to context
            try
            {
                var workingDirectory = Environment.CurrentDirectory;
                var files = Directory.GetFiles("Data\\Mock", "*.png", SearchOption.AllDirectories).ToList();
                var rx = new Regex(@"profile-pic-(\d+).png");

                foreach (var file in files)
                {
                    var stringId = string.Empty;
                    var imageBytes = File.ReadAllBytes(workingDirectory + "\\" + file);
                    var match = rx.Match(file);

                    if (match.Success)
                    {
                        stringId = match.Groups[1].Value;
                    }

                    context.ProfileImages.Add(new ProfileImage
                    {
                        Id = Int64.Parse(stringId),
                        Image = imageBytes
                    });
                }
            }
            catch (Exception)
            {
                // There was a problem when loading images mock data
            }

            context.SaveChanges();
        }
    }
}

