using assignmentAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace assignmentAPI.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }

        public DbSet<UserDetailsModel> Users { get; set; }

        public DbSet<MenuControlModel> Menu { get; set; }

        public DbSet<StudentsGridModel> Students { get; set; }
    }
}
