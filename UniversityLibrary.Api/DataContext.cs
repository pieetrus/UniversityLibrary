using Microsoft.EntityFrameworkCore;
using UniversityLibrary.Api.Model;

namespace UniversityLibrary.Api;

public class DataContext(IConfiguration configuration) : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to sqlite database
        options.UseSqlite(configuration.GetConnectionString("UniversityDatabase"));
    }

    public DbSet<Student> Students { get; set; }

}