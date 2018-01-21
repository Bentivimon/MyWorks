using EntityModels.Entitys;
using Microsoft.EntityFrameworkCore;

namespace GraduateWorkApi.Context
{
    public class DatabaseContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<CertificateOfTestingEntity> CertificateOfTestings { get; set; }
        public DbSet<CertificateOfSecondaryEducationEntity> CertificateOfSecondaryEducations { get; set; }
        public DbSet<EntrantEntity> Entrants { get; set; }
        public DbSet<SpecialityEntity> Specialtys { get; set; }
        public DbSet<StatementEntity> Statements { get; set; }
        public DbSet<UniversityEntity> Universitys { get; set; }

        public DatabaseContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UniversitySpeciality>()
                .HasKey(t => new { t.SpecialtyId, t.UniversityId });

            builder.Entity<UniversitySpeciality>()
                .HasOne(sc => sc.University)
                .WithMany(s => s.UniversitySpecialities)
                .HasForeignKey(sc => sc.UniversityId);

            builder.Entity<UniversitySpeciality>()
                .HasOne(sc => sc.Specialty)
                .WithMany(c => c.UniversitySpecialities)
                .HasForeignKey(sc => sc.SpecialtyId);
        }
    }
}
