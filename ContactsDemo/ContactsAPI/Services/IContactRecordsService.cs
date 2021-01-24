using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ContactsAPI.Models;

namespace ContactsAPI.Services
{
    public interface IContactRecordsService
    {
        Task AddUpdateContactRecord(ContactRecord contactRecord);

        IQueryable<ContactRecord> GetContactRecords(Expression<Func<ContactRecord, bool>> filter = null, bool avoidTrackingChanges = true);

        Task DeleteContactRecord(long id);

        Task AddUpdateContactRecordProfileImage(ProfileImage profileImage);

        Task<ProfileImage> GetContactRecordProfileImage(long id);

        Task DeleteContactRecordProfileImage(long id);

    }
}