using GestaoPessoas.Entities;
using GestaoPessoas.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Emit;

namespace GestaoPessoas.DataSources
{
    public class GestaoPessoasDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AttendanceRecord>()
                .Property(p => p.EventType)
                .HasConversion(v => v.ToString(), v => Enum.Parse<AttendanceEventType>(v));

            builder.Entity<Worker>()
                .HasMany(w => w.Projects)
                .WithMany(p => p.Workers);
        }

        public virtual DbSet<Worker> Workers => Set<Worker>();
        public virtual DbSet<Project> Projects => Set<Project>();

        public virtual DbSet<AttendanceRecord> AttendanceRecords => Set<AttendanceRecord>();

        public GestaoPessoasDbContext(DbContextOptions<GestaoPessoasDbContext> options) : base(options)
        {


        }
    }
}
