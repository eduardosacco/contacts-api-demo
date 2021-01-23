using ContactsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Data
{
    public class ContactRecordsContext : DbContext
    {
        public ContactRecordsContext(DbContextOptions<ContactRecordsContext> options)
            : base(options) { }

        public DbSet<ContactRecord> ContactRecords { get; set; }

        public DbSet<ProfileImage> ProfileImages { get; set; }
    }
}