using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GraduateWork.Server.Models.Response;

namespace GraduateWork.Server.Data.Entities
{
    /// <summary>
    /// Represent certificate of testing entity.
    /// </summary>
    [Table("certificates_of_testing")]
    public class CertificateOfTestingEntity
    {
        #region Properties
        /// <summary>
        /// Gets/Sets uniq identifier.
        /// </summary>
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets/Sets certificate of testing serial number.
        /// </summary>
        [Column("serial_number")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Gets/Sets certificate of testing year of issue.
        /// </summary>
        [Column("year_of_issue")]
        public DateTime YearOfIssue { get; set; }

        /// <summary>
        /// Gets/Sets certificate of testing first subject.
        /// </summary>
        [Column("first_subject")]
        public string FirstSubject { get; set; }

        /// <summary>
        /// Gets/Sets certificate of testing second subject.
        /// </summary>
        [Column("second_subject")]
        public string SecondSubject { get; set; }

        /// <summary>
        /// Gets/Sets certificate of testing  third subject.
        /// </summary>
        [Column("third_subject")]
        public string ThirdSubject { get; set; }

        /// <summary>
        /// Gets/Sets certificate of testing fourth subject
        /// </summary>
        [Column("fourth_subject")]
        public string FourthSubject { get; set; }

        /// <summary>
        /// Gets/Sets certificate of testing first mark
        /// </summary>
        [Column("first_mark")]
        public float FirstMark { get; set; }

        /// <summary>
        /// Gets/Sets certificate of testing second mark
        /// </summary>
        [Column("second_mark")]
        public float SecondMark { get; set; }

        /// <summary>
        /// Gets/Sets certificate of testing third mak.
        /// </summary>
        [Column("third_mark")]
        public float ThirdMark { get; set; }

        /// <summary>
        /// Gets/Sets certificate of testing fourth mark.
        /// </summary>
        [Column("fourth_mark")]
        public float FourthMark { get; set; }

        /// <summary>
        /// Gets/Sets certificate of testing associated with entrant by id.
        /// </summary>
        [Column("entrant_id")]
        public Guid? EntrantId { get; set; }

        #endregion

        #region Foreign keys

        /// <summary>
        /// Gets/Sets certificate of testing associated with entrant.
        /// </summary>
        public EntrantEntity Entrant { get; set; }

        #endregion

        #region Converters

        /// <summary>
        /// Converter to <see cref="CertificateOfTestingDto"/>.
        /// </summary>
        public CertificateOfTestingDto ToDto()
        {
            return new CertificateOfTestingDto
            {
                SerialNumber = SerialNumber,
                YearOfIssue = YearOfIssue,
                FirstSubject = FirstSubject,
                SecondSubject = SecondSubject,
                ThirdSubject = ThirdSubject,
                FourthSubject = FourthSubject,
                FirstMark = FirstMark,
                SecondMark = SecondMark,
                ThirdMark = ThirdMark,
                FourthMark = FourthMark
            };
        }

        #endregion

    }
}