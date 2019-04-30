using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduateWork.Server.Data.Entities
{
    /// <summary>
    /// Represent university specialities entity.
    /// </summary>
    [Table("university_specialities")]
    public class UniversitySpecialityEntity
    {
        #region Properties
        
        /// <summary>
        /// Gets/Sets university id.
        /// </summary>
        [Column("university_id")]
        public Guid UniversityId { get; set; }

        /// <summary>
        /// Gets/Sets specialty id.
        /// </summary>
        [Column("specialty_id")]
        public Guid SpecialtyId { get; set; }

        #endregion

        #region Foreign keys

        /// <summary>
        /// Gets/Sets associated with university.
        /// </summary>
        public UniversityEntity University { get; set; }

        /// <summary>
        /// Gets/Sets associated with specialty.
        /// </summary>
        public SpecialityEntity Specialty { get; set; }
        
        #endregion

    }
}