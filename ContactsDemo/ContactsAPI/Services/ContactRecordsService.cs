using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ContactsAPI.Data;
using ContactsAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ContactsAPI.Services
{
    public class ContactRecordsService : IContactRecordsService
    {
        private readonly ILogger<ContactRecordsService> _logger;
        private readonly ContactRecordsContext _context;

        public ContactRecordsService(ILogger<ContactRecordsService> logger, ContactRecordsContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task InsertOrUpdateContactRecord(ContactRecord contactRecord)
        {
            var entry = _context.Entry(contactRecord);

            if (entry.State == EntityState.Detached)
            {
                _context.ContactRecords.Add(contactRecord);
            }

            if (_context.ChangeTracker.HasChanges())
            {
                await _context.SaveChangesAsync();
            }
        }

        public IQueryable<ContactRecord> GetContactRecords(Expression<Func<ContactRecord, bool>> filter = null, bool avoidTrackingChanges = true)
        {
            var contactRecords = avoidTrackingChanges ? _context.ContactRecords.AsNoTracking() : _context.ContactRecords;

            return filter == null ? contactRecords.AsQueryable() : contactRecords.Where(filter).AsQueryable();
        }

        public async Task DeleteContactRecord(long id)
        {
            var contactRecord = await _context.ContactRecords.FirstOrDefaultAsync(x => x.Id == id);

            if (contactRecord != null)
            {
                _context.ContactRecords.Remove(contactRecord);
                await _context.SaveChangesAsync();
            }
        }

        public async Task InsertOrUpdateContactRecordProfileImage(ProfileImage profileImage)
        {
            var entry = _context.Entry(profileImage);

            if (entry.State == EntityState.Detached)
            {
                _context.ProfileImages.Add(profileImage);
            }

            if (_context.ChangeTracker.HasChanges())
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ProfileImage> GetContactRecordProfileImage(long id)
        {
            return await _context.ProfileImages.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task DeleteContactRecordProfileImage(long id)
        {
            var profileImage = await _context.ProfileImages.FirstOrDefaultAsync(x => x.Id == id);

            if (profileImage != null)
            {
                _context.ProfileImages.Remove(profileImage);
                await _context.SaveChangesAsync();
            }
        }
    }
}
