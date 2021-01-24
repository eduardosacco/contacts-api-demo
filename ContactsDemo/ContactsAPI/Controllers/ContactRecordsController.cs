using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ContactsAPI.Models;
using ContactsAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ContactsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactRecordsController : ControllerBase
    {
        private readonly ILogger<ContactRecordsController> _logger;
        private readonly IContactRecordsService _contactRecordsService;

        private static readonly List<byte[]> _supportedFileSignatures =
        new List<byte[]>
        {
            // .jpeg
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 }, 
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
            // .png
            new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }
        };

        public ContactRecordsController(
            IContactRecordsService contactRecordsService,
            ILogger<ContactRecordsController> logger)
        {
            _contactRecordsService = contactRecordsService;
            _logger = logger;
        }

        /// <summary>Creates contact record if Id = 0, updates a contact record if Id is specified</summary>
        [HttpPost]
        [Route("record")]
        public async Task<ActionResult<ContactRecord>> AddUpdateContactRecord([Required] ContactRecord contactRecord)
        {
            await _contactRecordsService.AddUpdateContactRecord(contactRecord);

            return CreatedAtAction(nameof(AddUpdateContactRecord), new { id = contactRecord.Id }, contactRecord);
        }

        /// <summary>Deletes contact with specified Id</summary>
        /// <param name="id" example="2">Id</param>
        [HttpDelete]
        [Route("record")]
        public async Task<IActionResult> DeleteContactRecordById([Required] long id)
        {
            await _contactRecordsService.DeleteContactRecord(id);

            return NoContent();
        }

        /// <summary>Get contact with specified Id</summary>
        /// <param name="id" example="2">Id</param>
        [HttpGet]
        [Route("record/id")]
        public async Task<ActionResult<ContactRecord>> GetContactRecordById([Required] long id)
        {
            var contactRecord = await _contactRecordsService
                .GetContactRecords(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (contactRecord == null)
            {
                return NotFound($"No contact record found for id: '{id}'.");
            }

            return contactRecord;
        }

        /// <summary>Get contact with specified phone number</summary>
        /// <param name="phoneNumber" example="12345678">Phone number</param>
        [HttpGet]
        [Route("record/phoneNumber")]
        public async Task<ActionResult<ContactRecord>> GetContactRecordByPhonenumber([Required] string phoneNumber)
        {
            var contactRecord = await _contactRecordsService
                .GetContactRecords(x => x.PhoneNumberPersonal == phoneNumber || x.PhoneNumberWork == phoneNumber)
                .FirstOrDefaultAsync();

            if (contactRecord == null)
            {
                return NotFound($"No contact record found for phone number: '{phoneNumber}'.");
            }

            return contactRecord;
        }

        /// <summary>Get contact with specified email address</summary>
        /// /// <param name="emailAddress" example="john.doe@contoso.com">Email address</param>
        [HttpGet]
        [Route("record/emailAddress")]
        public async Task<ActionResult<ContactRecord>> GetContactRecordByEmailAddress([Required] string emailAddress)
        {
            var contactRecord = await _contactRecordsService
                .GetContactRecords(x => string.Equals(emailAddress, x.EmailAddress, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefaultAsync();

            if (contactRecord == null)
            {
                return NotFound($"No contact record found for email address: '{emailAddress}'.");
            }

            return contactRecord;
        }

        /// <summary>Get all contact records</summary>
        [HttpGet]
        [Route("records")]
        public async Task<ActionResult<IEnumerable<ContactRecord>>> GetContactRecords()
        {
            return await _contactRecordsService.GetContactRecords().ToListAsync();
        }

        /// <summary>Get all contact with the specified location. </summary>
        /// <param name="country" example="USA">Country</param>
        /// <param name="state" example="Washington">Country</param>
        /// <param name="city" example="Snoqualmie">Country</param>
        [HttpGet]
        [Route("records/location")]
        public async Task<ActionResult<IEnumerable<ContactRecord>>>
            GetContactRecordsByState([Required] string country, string state = null, string city = null)
        {
            return await _contactRecordsService
                .GetContactRecords().Where(
                    x => string.Equals(country, x.Country, StringComparison.OrdinalIgnoreCase) &&
                    string.IsNullOrWhiteSpace(state) || string.Equals(state, x.State, StringComparison.OrdinalIgnoreCase) &&
                    string.IsNullOrWhiteSpace(city) || string.Equals(city, x.City, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }

        /// <summary>Set contact record profile image for specified id</summary>
        /// <param name="contactRecordId" example="1">Id of the contact record to add the image to</param>
        /// <param name="file">Image file (.jpeg .png supported)</param>
        [HttpPost]
        [Route("profileImage")]
        public async Task<ActionResult> AddUpdateContactRecordProfileImageById([Required] long contactRecordId, [Required] IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                // Upload the file if less than 256 KB
                if (memoryStream.Length < 262144)
                {
                    var bytes = memoryStream.ToArray();

                    // Validate file signature
                    var headerLength = _supportedFileSignatures.Max(m => m.Length);
                    var imageTypeSupported = _supportedFileSignatures.Any(signature =>
                    bytes.Take(signature.Length).SequenceEqual(signature));

                    if (imageTypeSupported)
                    {
                        var profileImage = new ProfileImage
                        {
                            Id = 0,
                            ContactRecordId = contactRecordId,
                            Image = bytes
                        };

                        await _contactRecordsService.
                            AddUpdateContactRecordProfileImage(profileImage);
                    }
                    else
                    {
                        return BadRequest("The file type must be .jpeg or .png.");
                    }
                }
                else
                {
                    return BadRequest("The file must not be larger than 256 KB.");
                }
            }

            return Ok("File uploaded successfully.");
        }

        /// <summary>Get contact record profile image for specified id</summary>
        [HttpGet]
        [Route("profileImage")]
        public async Task<FileContentResult> GetContactRecordProfileImageById([Required] long contactRecordId)
        {
            var profileImage = await _contactRecordsService.GetContactRecordProfileImage(contactRecordId);

            return File(profileImage.Image, "image/png");
        }
    }
}
