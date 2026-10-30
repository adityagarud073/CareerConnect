using Microsoft.EntityFrameworkCore;
using SmartRecruitment.Models;

namespace SmartRecruitment.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Job> Jobs { get; set; }

    public DbSet<Resume> Resumes { get; set; }

    public DbSet<Application> Applications { get; set; }


}