using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GraduateWork.Server.Models.Request;
using GraduateWork.Server.Models.Response;

namespace GraduateWork.Server.Data.Entities
{
    /// <summary>
    /// Represent university entity.
    /// </summary>
    [Table("universities")]
    public class UniversityEntity
    {
        #region Properties

        /// <summary>
        /// Gets/Sets uniq entrant identifier.
        /// </summary>
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets/Sets university full name.
        /// </summary>
        [Column("full_name")]
        public string FullName { get; set; }

        /// <summary>
        /// Gets/Sets university level of accreditation.
        /// </summary>
        [Column("level_of_accreditation")]
        public string LevelOfAccreditation { get; set; }

        #endregion

        #region Foreign keys

        /// <summary>
        /// Gets/Sets university associated with universities.
        /// </summary>
        public IEnumerable<StatementEntity> Statements { get; set; }

        /// <summary>
        /// Gets/Sets university associated with university specialities.
        /// </summary>
        public IEnumerable<UniversitySpecialityEntity> UniversitySpecialities { get; set; }

        #endregion
        
        /// <summary>
        /// Basic constructor.
        /// </summary>
        public UniversityEntity()
        { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="request"><see cref="UniversityRequest"/> instance.</param>
        public UniversityEntity(UniversityRequest request)
        {
            FullName = request.FullName;
            LevelOfAccreditation = request.LevelOfAccreditation;
        }

        /// <summary>
        /// Convert to <see cref="UniversityDto"/>.
        /// </summary>
        public UniversityDto ToDto()
        {
            return new UniversityDto
            {
                FullName = FullName,
                LevelOfAccreditation = LevelOfAccreditation
            };
        }
    }
}