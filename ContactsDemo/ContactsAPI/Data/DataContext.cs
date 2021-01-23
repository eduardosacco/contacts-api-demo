using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }

        public DbSet<ContactRecord> ContactRecords { get; set; }

        public DbSet<ProfileImage> ProfileImages { get; set; }
    }
}