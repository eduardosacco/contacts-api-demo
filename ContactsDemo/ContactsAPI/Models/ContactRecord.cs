using System;
using System.ComponentModel.DataAnnotations;

namespace ContactsAPI.Models
{
    public class ContactRecord
    {
        /// <example>0</example>
        public long Id { get; set; }

        /// <example>John</example>
        [Required]
        public string Name { get; set; }

        /// <example>Doe</example>
        [Required]
        public string LastName { get; set; }

        /// <example>Contoso</example>
        public string Company { get; set; }

        /// <example>john.doe@contoso.com</example>
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }

        /// <example>1990-03-23</example>
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        /// <example>19345766</example>
        [RegularExpression(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumberPersonal { get; set; }

        /// <example>19344766</example>
        [RegularExpression(@"^^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumberWork { get; set; }

        /// <example>USA</example>
        public string Country { get; set; }

        /// <example>Massachussetts</example>
        public string State { get; set; }

        /// <example>Boston</example>
        public string City { get; set; }

        /// <example>Hollister St 1442</example>
        public string Address { get; set; }
    }
}
