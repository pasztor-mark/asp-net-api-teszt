using api_teszt.Models;
using Microsoft.EntityFrameworkCore;

namespace api_teszt.Database;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> opt) : base(opt)
    {
        
    }
    public DbSet<User> users { get; set; }
    public DbSet<Note> notes { get; set; }

}