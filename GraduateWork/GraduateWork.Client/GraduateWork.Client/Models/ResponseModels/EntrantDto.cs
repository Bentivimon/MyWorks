using System;

namespace GraduateWork.Client.Models.ResponseModels
{
    /// <summary>
    /// Represent entrant response model.
    /// </summary>
    public class EntrantDto
    {
        /// <summary>
        /// Gets/Sets entrant id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets/Sets entrant name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets/Sets entrant surname.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Gets/Sets entrant birth day.
        /// </summary>
        public DateTime BDay { get; set; }

        /// <summary>
        /// Gets/Sets certificate of testing.
        /// </summary>
        public CertificateOfTestingDto CertificateOfTesting { get; set; }

        /// <summary>
        /// Gets/Sets certificate of secondary education.
        /// </summary>
        public CertificateOfSecondaryEducationDto CertificateOfSecondaryEducation { get; set; }

        /// <summary>
        /// Gets/Sets total score by speciality.
        /// </summary>
        public float TotalScore { get; set; }
    }
}
