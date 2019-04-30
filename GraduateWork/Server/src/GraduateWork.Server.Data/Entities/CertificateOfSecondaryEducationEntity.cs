using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GraduateWork.Server.Models.Response;

namespace GraduateWork.Server.Data.Entities
{
    /// <summary>
    /// Represent certificate of secondary education entity. 
    /// </summary>
    [Table("certificate_of_secondary_education")]
    public class CertificateOfSecondaryEducationEntity
    {
        #region Propertis

        /// <summary>
        /// Gets/Sets uniq certificate of secondary education identifier.
        /// </summary>
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets/Sets certificate of secondary education serial number.
        /// </summary>
        [Column("serial_number")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Gets/Sets certificate of secondary education average mark.
        /// </summary>
        [Column("average_mark")]
        public float AverageMark { get; set; }

        /// <summary>
        /// Gets/Sets certificate of secondary education full name of the educational institution. 
        /// </summary>
        [Column("full_name_of_the_educational_institution")]
        public string FullNameOfTheEducationalInstitution { get; set; }

        /// <summary>
        /// Gets/Sets certificate of secondary education year of issue.
        /// </summary>
        [Column("year_of_issue")]
        public DateTime YearOfIssue { get; set; }

        /// <summary>
        /// Gets/Sets certificate of secondary education associated with entrant by id.
        /// </summary>
        [Column("entrant_id")]
        public Guid? EntrantId { get; set; }

        #endregion

        #region Foreign keys

        /// <summary>
        /// Gets/Sets certificate of secondary education associated with entrant.
        /// </summary>
        public EntrantEntity Entrant { get; set; }

        #endregion

        #region Convertors

        /// <summary>
        /// Converter to <see cref="CertificateOfSecondaryEducationDto"/>.
        /// </summary>
        public CertificateOfSecondaryEducationDto ToDto()
        {
            return new CertificateOfSecondaryEducationDto
            {
                SerialNumber = SerialNumber,
                AverageMark = AverageMark,
                FullNameOfTheEducationalInstitution = FullNameOfTheEducationalInstitution,
                YearOfIssue = YearOfIssue
            };
        }

        #endregion

    }
}