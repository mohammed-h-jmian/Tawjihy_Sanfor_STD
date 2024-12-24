using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using tawjihy.Data.Models;

namespace tawjihy.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Majoring> Majorings { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Visitor> Visitors { get; set; }
    }
}