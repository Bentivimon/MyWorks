using System.ComponentModel.DataAnnotations;

namespace GraduateWork.Server.Models.Request
{
    /// <summary>
    /// Represent model for change user email.
    /// </summary>
    public class ChangeUserEmailModel
    {
        /// <summary>
        /// Gets/Sets old user email.
        /// </summary>
        [Required]
        public string OldEmail { get; set; }

        /// <summary>
        /// Gets/Sets new user email.
        /// </summary>
        [Required]
        [DataType(DataType.EmailAddress)]
        public string NewEmail { get; set; }
    }
}
