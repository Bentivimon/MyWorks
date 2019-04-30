namespace GraduateWork.Server.Models.Response
{
    /// <summary>
    /// Represent extended entrant response model.
    /// </summary>
    public class EntrantExtendDto : EntrantDto
    {
        /// <summary>
        /// Gets/Sets certificate of testing.
        /// </summary>
        public CertificateOfTestingDto CertificateOfTesting { get; set; }

        /// <summary>
        /// Gets/Sets certificate of secondary education.
        /// </summary>
        public CertificateOfSecondaryEducationDto CertificateOfSecondaryEducation { get; set; }
    }
}
