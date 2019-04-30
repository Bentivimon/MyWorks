using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduateWork.Server.Data.Entities
{
    /// <summary>
    /// Represent statement entity.
    /// </summary>
    [Table("statements")]
    public class StatementEntity
    {
        #region Properties

        /// <summary>
        /// Gets/Sets uniq entrant identifier.
        /// </summary>
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets/Sets statement total score.
        /// </summary>
        [Column("total_score")]
        public float TotalScore { get; set; }

        /// <summary>
        /// Gets/Sets statement extra score.
        /// </summary>
        [Column("extra_score")]
        public float ExtraScore { get; set; }

        /// <summary>
        /// Gets/Sets statement associated with entrant by id.
        /// </summary>
        [Column("entrant_id")]
        public Guid EntrantId { get; set; }

        /// <summary>
        /// Gets/Sets statement associated with university by id.
        /// </summary>
        [Column("university_id")]
        public Guid UniversityId { get; set; }

        #endregion

        #region Foreign keys

        /// <summary>
        /// Gets/Sets statement associated with entrant entity.
        /// </summary>
        public EntrantEntity Entrant { get; set; }

        /// <summary>
        /// Gets/Sets statement associated with university entity.
        /// </summary>
        public UniversityEntity University { get; set; }
        
        #endregion
    }
}