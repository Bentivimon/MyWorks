using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GraduateWork.Server.Models.Request;
using GraduateWork.Server.Models.Response;

namespace GraduateWork.Server.Data.Entities
{
    /// <summary>
    /// Represents speciality entity.
    /// </summary>
    [Table("specialities")]
    public class SpecialityEntity
    {
        #region Properties

        /// <summary>
        /// Gets/Sets uniq speciality identifier.
        /// </summary>
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets/Sets speciality code.
        /// </summary>
        [Column("code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets/Sets speciality name.
        /// </summary>
        [Column("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets/Sets speciality additional factor.
        /// </summary>
        [Column("additional_factory")]
        public float AdditionalFactor { get; set; }

        /// <summary>
        /// Gets/Sets speciality count of state places.
        /// </summary>
        [Column("count_of_state_places")]
        public int CountOfStatePlaces { get; set; }

        #endregion

        #region Foreign keys

        /// <summary>
        /// Gets/Sets speciality associated with a university specialities.
        /// </summary>
        public IEnumerable<UniversitySpecialityEntity> UniversitySpecialities { get; set; }

        #endregion

        /// <summary>
        /// Basic constructor.
        /// </summary>
        public SpecialityEntity()
        { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="request"><see cref="SpecialityRequest"/> instance.</param>
        public SpecialityEntity(SpecialityRequest request)
        {
            Code = request.Code;
            Name = request.Name;
            AdditionalFactor = request.AdditionalFactor;
            CountOfStatePlaces = request.CountOfStatePlaces;
        }

        /// <summary>
        /// Method for update speciality.
        /// </summary>
        /// <param name="request"><see cref="SpecialityRequest"/> instance.</param>
        public void Update(SpecialityRequest request)
        {
            AdditionalFactor = request.AdditionalFactor;
            CountOfStatePlaces = request.CountOfStatePlaces;
            Name = request.Name;
        }

        /// <summary>
        /// Method for convert entity to dto.
        /// </summary>
        public SpecialityDto ToDto()
        {
            return new SpecialityDto
            {
                Code = Code,
                Name = Name,
                AdditionalFactor = AdditionalFactor,
                CountOfStatePlaces = CountOfStatePlaces
            };
        }
    }
}