using System.Threading.Tasks;
using ContactsAPI.Data;
using ContactsAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ContactsAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        [HttpGet]
        public async Task<ActionResult<ContactRecord>> GetContactRecordById(long id)
        {
            var contactRecord = await _contactRecordsService.GetContactRecord(id);

            if (contactRecord == null)
            {
                return NotFound();
            }

            return contactRecord;
        }

        [HttpPost]
        public async Task<ActionResult<ContactRecord>> AddContactRecord(ContactRecord contactRecord)
        {
            await _contactRecordsService.InsertContactRecord(contactRecord);

            return CreatedAtAction(nameof(AddContactRecord), new { id = contactRecord.Id }, contactRecord);
        }

        
    }
}
