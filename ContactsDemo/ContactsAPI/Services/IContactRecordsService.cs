using System.Collections.Generic;
using System.Threading.Tasks;
using ContactsAPI.Data;

namespace ContactsAPI.Services
{
    public interface IContactRecordsService
    {
        Task DeleteContactRecord(long id);
        Task<ContactRecord> GetContactRecord(long id);
        Task<IEnumerable<ContactRecord>> GetContactRecords();
        Task InsertContactRecord(ContactRecord contactRecord);
    }
}