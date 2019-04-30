using System;

namespace GraduateWork.Server.Models.Response
{
    /// <summary>
    /// Represent certificate of secondary education response model.
    /// </summary>
    public class CertificateOfSecondaryEducationDto
    {
        /// <summary>
        /// Gets/Sets serial number.
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// Gets/Sets average mark.
        /// </summary>
        public float AverageMark { get; set; }

        /// <summary>
        /// Gets/Sets full name of the educational institution.
        /// </summary>
        public string FullNameOfTheEducationalInstitution { get; set; }

        /// <summary>
        /// Gets/Sets year of issue
        /// </summary>
        public DateTime YearOfIssue { get; set; }
    }
}
