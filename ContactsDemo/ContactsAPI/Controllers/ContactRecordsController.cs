using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactsAPI.Models;
using ContactsAPI.Services;
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

        public ContactRecordsController(
            IContactRecordsService contactRecordsService,
            ILogger<ContactRecordsController> logger)
        {
            _contactRecordsService = contactRecordsService;
            _logger = logger;
        }

        /// <summary>
        /// Creates new contact record a specific TodoItem.
        /// </summary>
        [HttpPost]
        [Route("record")]
        public async Task<ActionResult<ContactRecord>> AddContactRecord(ContactRecord contactRecord)
        {
            await _contactRecordsService.InsertOrUpdateContactRecord(contactRecord);

            return CreatedAtAction(nameof(AddContactRecord), new { id = contactRecord.Id }, contactRecord);
        }

        [HttpDelete]
        [Route("record")]
        public async Task<IActionResult> DeleteContactRecordById(long id)
        {
            await _contactRecordsService.DeleteContactRecord(id);

            return NoContent();
        }

        [HttpGet]
        [Route("record/id")]
        public async Task<ActionResult<ContactRecord>> GetContactRecordByPhonenumber(long id)
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

        [HttpGet]
        [Route("record/phonenumber")]
        public async Task<ActionResult<ContactRecord>> GetContactRecordByPhonenumber(string phoneNumber)
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

        [HttpGet]
        [Route("record/emailaddress")]
        public async Task<ActionResult<ContactRecord>> GetContactRecordByEmailAddress(string emailAddress)
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

        [HttpGet]
        [Route("records")]
        public async Task<ActionResult<IEnumerable<ContactRecord>>> GetContactRecords()
        {
            return await _contactRecordsService.GetContactRecords().ToListAsync();
        }

        [HttpGet]
        [Route("records/state")]
        public async Task<ActionResult<IEnumerable<ContactRecord>>> GetContactRecordsByState(string state)
        {
            return await _contactRecordsService
                .GetContactRecords().Where(x => string.Equals(state, x.State, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }

        [HttpGet]
        [Route("profileImage")]
        public async Task<FileContentResult> GetContactRecordProfileImageById(long id)
        {
            var profileImage = await _contactRecordsService.GetContactRecordProfileImage(id);

            return File(profileImage.Image, "image/png");
        }
    }
}
