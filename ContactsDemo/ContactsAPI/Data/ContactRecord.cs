using System;

namespace ContactsAPI.Data
{
    public class ContactRecord
    {
        public long Id { get; set; }

        public Guid ProfileImageId { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Company { get; set; }

        public string EmailAddress { get; set; }

        public DateTime BirthDate { get; set; }

        public string PhoneNumberPersonal { get; set; }

        public string PhoneNumberWork { get; set; }

        public string Address { get; set; }
    }
}
