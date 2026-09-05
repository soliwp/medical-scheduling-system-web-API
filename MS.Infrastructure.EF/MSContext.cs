using Microsoft.EntityFrameworkCore;
using MS.Domain.Entities.appointment;
using MS.Domain.Entities.authentication;
using MS.Domain.Entities.doctors;
using MS.Domain.Entities.patients;
using MS.Domain.Entities.Schedules;

namespace MS.Infrastructure.EF;
public class MSContext : DbContext
{
    public DbSet<Doctor> doctors { get; set; }
    public DbSet<Patient> patients { get; set; }
    public DbSet<Schedule> schedules { get; set; }
    public DbSet<Appointment> appointments { get; set; }
    public DbSet<User> users { get; set; }
    public DbSet<Role> roles { get; set; }
    public MSContext(DbContextOptions<MSContext> options) :base(options) {}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MSContext).Assembly);
    }
}
