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

        /// <summary>
        /// Gets/Sets university ownership.
        /// </summary>
        [Column("ownership")]
        public string Ownership { get; set; }

        /// <summary>
        /// Gets/Sets university chief.
        /// </summary>
        [Column("chief")]
        public string Chief { get; set; }

        /// <summary>
        /// Gets/Sets university subordination.
        /// </summary>
        [Column("subordination")]
        public string Subordination { get; set; }

        /// <summary>
        /// Gets/Sets university post index.
        /// </summary>
        [Column("post_index")]
        public string PostIndex { get; set; }

        /// <summary>
        /// Gets/Sets university address.
        /// </summary>
        [Column("address")]
        public string Address { get; set; }

        /// <summary>
        /// Gets/Sets university phone.
        /// </summary>
        [Column("phone")]
        public string Phone { get; set; }

        /// <summary>
        /// Gets/Sets university site.
        /// </summary>
        [Column("site")]
        public string Site { get; set; }

        /// <summary>
        /// Gets/Sets university email.
        /// </summary>
        [Column("email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets/Sets region id of university.
        /// </summary>
        [Column("region_id")]
        public int RegionId { get; set; }

        #endregion

        #region Foreign keys

        public RegionEntity Region { get; set; }

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
                Id = Id,
                FullName = FullName,
                LevelOfAccreditation = LevelOfAccreditation, 
                Ownership = Ownership,
                Phone = Phone,
                Email = Email,
                Address = Address,
                Chief = Chief,
                PostIndex = PostIndex,
                Site = Site,
                Subordination = Subordination
            };
        }
    }
}