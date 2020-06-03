using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoovieBlog.Models;

namespace MoovieBlog.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Moovie> Moovies { get; set; }
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}