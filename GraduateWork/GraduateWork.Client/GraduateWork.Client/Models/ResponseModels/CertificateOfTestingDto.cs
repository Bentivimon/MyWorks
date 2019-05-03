using System;

namespace GraduateWork.Client.Models.ResponseModels
{
    /// <summary>
    /// Represent certificate of testing response model.
    /// </summary>
    public class CertificateOfTestingDto
    {
        /// <summary>
        /// Gets/Sets serial number.
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// Gets/Sets year of issue.
        /// </summary>
        public DateTime YearOfIssue { get; set; }

        /// <summary>
        /// Gets/Sets first subject.
        /// </summary>
        public string FirstSubject { get; set; }

        /// <summary>
        /// Gets/Sets second subject.
        /// </summary>
        public string SecondSubject { get; set; }

        /// <summary>
        /// Gets/Sets third subject.
        /// </summary>
        public string ThirdSubject { get; set; }

        /// <summary>
        /// Gets/Sets fourth subject.
        /// </summary>
        public string FourthSubject { get; set; }

        /// <summary>
        /// Gets/Sets first mark.
        /// </summary>
        public float FirstMark { get; set; }

        /// <summary>
        /// Gets/Sets second mark.
        /// </summary>
        public float SecondMark { get; set; }

        /// <summary>
        /// Gets/Sets third mark.
        /// </summary>
        public float ThirdMark { get; set; }

        /// <summary>
        /// Gets/Sets fourth mark.
        /// </summary>
        public float FourthMark { get; set; }
    }
}