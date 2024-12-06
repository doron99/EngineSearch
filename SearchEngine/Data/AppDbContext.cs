using Microsoft.EntityFrameworkCore;
using SearchEngine.Models;

namespace SearchEngine.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<SearchRecord> SearchRecords { get; set; }  // Represents the "People" table

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)  // Pass the options to the base constructor
        {
        }
    }
}
