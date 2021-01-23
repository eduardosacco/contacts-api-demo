using System.Collections.Generic;
using System.Threading.Tasks;
using ContactsAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ContactsAPI.Services
{
    public class ContactRecordsService : IContactRecordsService
    {
        private readonly ILogger<ContactRecordsService> _logger;
        private readonly DataContext _context;

        public ContactRecordsService(ILogger<ContactRecordsService> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IEnumerable<ContactRecord>> GetContactRecords()
        {
            return await _context.ContactRecords.ToListAsync();
        }

        public async Task<ContactRecord> GetContactRecord(long id)
        {
            var contactRecord = await _context.ContactRecords.FindAsync(id);

            if (contactRecord == null)
            {
                return null;
            }

            return contactRecord;
        }

        public async Task InsertContactRecord(ContactRecord contactRecord)
        {
            _context.ContactRecords.Add(contactRecord);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteContactRecord(long id)
        {
            var contactRecord = await _context.ContactRecords.FindAsync(id);
            _context.ContactRecords.Remove(contactRecord);
            await _context.SaveChangesAsync();
        }
    }
}
