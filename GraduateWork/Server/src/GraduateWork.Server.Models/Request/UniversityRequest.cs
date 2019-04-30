using System.ComponentModel.DataAnnotations;

namespace GraduateWork.Server.Models.Request
{
    /// <summary>
    /// University request model.
    /// </summary>
    public class UniversityRequest
    {
        /// <summary>
        /// Gets/Sets full name.
        /// </summary>
        [Required]
        public string FullName { get; set; }

        /// <summary>
        /// Gets/Sets level of accreditation.
        /// </summary>
        [Required]
        public string LevelOfAccreditation { get; set; }
    }
}
