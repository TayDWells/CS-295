using Microsoft.EntityFrameworkCore;

namespace BarberShop.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Service> Services { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the primary key for the Appointment entity
            modelBuilder.Entity<Appointment>()
                .HasKey(a => a.ApptId);

            // Configure relationships and other model-specific settings here
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Client)
                .WithMany()
                .HasForeignKey(a => a.ClientId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Service)
                .WithMany()
                .HasForeignKey(a => a.ServiceId);
        }
    }
}
