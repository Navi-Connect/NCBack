using Microsoft.EntityFrameworkCore;
using NCBack.Models;

namespace NCBack.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    
    public DbSet<User> Users => Set<User>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder
            .UseNpgsql("Server=127.0.0.1; Port=5432 ; Database=NaviConnect; User Id=postgres; Password=postgres;");
    }
}