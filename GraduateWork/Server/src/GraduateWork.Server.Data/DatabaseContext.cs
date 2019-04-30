using GraduateWork.Server.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GraduateWork.Server.Data
{
    /// <summary>
    /// Represent database context.
    /// </summary>
    public class DatabaseContext: DbContext
    {
        /// <summary>
        /// Gets/Sets user entities.
        /// </summary>
        public DbSet<UserEntity> Users { get; set; }

        /// <summary>
        /// Gets/Sets certificate of testing entities.
        /// </summary>
        public DbSet<CertificateOfTestingEntity> CertificateOfTestings { get; set; }

        /// <summary>
        /// Gets/Sets certificate of secondary education entities.
        /// </summary>
        public DbSet<CertificateOfSecondaryEducationEntity> CertificateOfSecondaryEducations { get; set; }

        /// <summary>
        /// Gets/Sets entrant entities.
        /// </summary>
        public DbSet<EntrantEntity> Entrants { get; set; }

        /// <summary>
        /// Gets/Sets speciality entities.
        /// </summary>
        public DbSet<SpecialityEntity> Specialties { get; set; }

        /// <summary>
        /// Gets/Sets statement entities.
        /// </summary>
        public DbSet<StatementEntity> Statements { get; set; }

        /// <summary>
        /// Gets/Sets university entities.
        /// </summary>
        public DbSet<UniversityEntity> Universities { get; set; }

        /// <summary>
        /// Constructor for initialize <see cref="DatabaseContext"/> instance with options.
        /// </summary>
        /// <param name="options"> Database context. </param>
        public DatabaseContext(DbContextOptions options)
            : base(options)
        {
        }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UniversitySpecialityEntity>()
                .HasKey(t => new { t.SpecialtyId, t.UniversityId });

            builder.Entity<UniversitySpecialityEntity>()
                .HasOne(sc => sc.University)
                .WithMany(s => s.UniversitySpecialities)
                .HasForeignKey(sc => sc.UniversityId);

            builder.Entity<UniversitySpecialityEntity>()
                .HasOne(sc => sc.Specialty)
                .WithMany(c => c.UniversitySpecialities)
                .HasForeignKey(sc => sc.SpecialtyId);
        }
    }
}
