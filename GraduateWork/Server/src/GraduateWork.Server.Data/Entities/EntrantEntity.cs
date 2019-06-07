using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using GraduateWork.Server.Models.Enums;
using GraduateWork.Server.Models.Request;
using GraduateWork.Server.Models.Response;

namespace GraduateWork.Server.Data.Entities
{
    /// <summary>
    /// Represent entrant entity.
    /// </summary>
    [Table("entrants")]
    public class EntrantEntity
    {
        
        #region Properties

        /// <summary>
        /// Gets/Sets uniq entrant identifier.
        /// </summary>
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets/Sets entrant first name.
        /// </summary>
        [Column("firs_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets/Sets entrant last name.
        /// </summary>
        [Column("last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets/Sets entrant birthday.
        /// </summary>
        [Column("birthday")]
        public DateTimeOffset Birthday { get; set; }
        
        #endregion

        #region Foreign keys

        /// <summary>
        /// Gets/Sets entrant associated with user entity.
        /// </summary>
        public List<UserEntity> Users { get; set; }

        /// <summary>
        /// Gets/Sets entrant associated with certificate of testing.
        /// </summary>
        public CertificateOfTestingEntity CertificateOfTesting { get; set; }

        /// <summary>
        /// Gets/Sets entrant associated with certificate of secondary education.
        /// </summary>
        public CertificateOfSecondaryEducationEntity CertificateOfSecondaryEducation { get; set; }

        /// <summary>
        /// Gets/Sets entrant associated with statements.
        /// </summary>
        public List<StatementEntity> Statements { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Basic constructor.
        /// </summary>
        public EntrantEntity()
        { }

        /// <summary>
        /// Constructor with <see cref="EntrantModel"/> parameter.
        /// </summary>
        /// <param name="request"><see cref="EntrantModel"/> instance.</param>
        public EntrantEntity(EntrantModel request)
        {
            FirstName = request.Name;
            LastName = request.Surname;
            Birthday = request.BDay;
        }

        #endregion

        #region Converters

        /// <summary>
        /// Converter to <see cref="EntrantDto"/>.
        /// </summary>
        public EntrantDto ToDto()
        {
            return new EntrantDto
            {
                Id = Id,
                Name = FirstName,
                Surname = LastName,
                BDay = Birthday.UtcDateTime
            };
        }

        /// <summary>
        /// Converter to <see cref="EntrantExtendDto"/>.
        /// </summary>
        public EntrantExtendDto ToExtendedDto()
        {
            return new EntrantExtendDto()
            {
                Id = Id,
                Name = FirstName,
                Surname = LastName,
                BDay = Birthday.UtcDateTime,
                CertificateOfTesting = CertificateOfTesting?.ToDto(),
                CertificateOfSecondaryEducation = CertificateOfSecondaryEducation?.ToDto(),
                TotalScore = Statements?.First().TotalScore ?? 0f
            };
        }

        #endregion

        /// <summary>
        /// Method for update fields by <see cref="EntrantModel"/> instance.
        /// </summary>
        /// <param name="request"><see cref="EntrantModel"/> instance.</param>
        public void Update(EntrantModel request)
        {
            FirstName = request.Name;
            LastName = request.Surname;
            Birthday = request.BDay;
        }

    }
}