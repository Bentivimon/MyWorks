using System.ComponentModel.DataAnnotations;

namespace GraduateWork.Server.Models.Request
{
    /// <summary>
    /// Represent model for change user password.
    /// </summary>
    public class ChangeUserPasswordModel
    {
        /// <summary>
        /// Gets/Sets old user password.
        /// </summary>
        [Required]
        public string OldPassword { get; set; }

        /// <summary>
        /// Gets/Sets new user password.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
