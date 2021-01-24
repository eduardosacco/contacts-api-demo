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

        public async Task AddUpdateContactRecord(ContactRecord contactRecord)
        {
            _context.Entry(contactRecord).State = contactRecord.Id == 0 ?
                EntityState.Added : EntityState.Modified;

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
            }

            var profileImage = await _context.ProfileImages.FirstOrDefaultAsync(x => x.ContactRecordId == id);
            if (profileImage != null)
            {
                _context.ProfileImages.Remove(profileImage);
            }

            if (_context.ChangeTracker.HasChanges())
            {
                await _context.SaveChangesAsync();
            }

        }

        public async Task AddUpdateContactRecordProfileImage(ProfileImage profileImage)
        {
            var existingProfileImage = await _context.ProfileImages.FirstOrDefaultAsync(x => x.ContactRecordId == profileImage.ContactRecordId);

            if (existingProfileImage != null)
            {
                existingProfileImage.Image = profileImage.Image;
            }
            else
            {
                _context.Entry(profileImage).State = EntityState.Added;
            }

            if (_context.ChangeTracker.HasChanges())
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ProfileImage> GetContactRecordProfileImage(long contactRecordId)
        {
            return await _context.ProfileImages.FirstOrDefaultAsync(x => x.ContactRecordId == contactRecordId);
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
