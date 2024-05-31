using Microsoft.EntityFrameworkCore;
using triakl.Models;

namespace triakl.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options) 
        { 
        
        }

        public DbSet<Room> Room { get; set; } 
        
        public DbSet<Guest> Guest { get; set; }
    }
}