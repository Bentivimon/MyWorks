using System.ComponentModel.DataAnnotations;

namespace GraduateWork.Server.Models.Request
{
    /// <summary>
    /// Request model for user authorization.
    /// </summary>
    public class UserLoginModel
    {
        /// <summary>
        /// Gets/Sets user login.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Login { get; set; }

        /// <summary>
        /// Gets/Sets user password.
        /// </summary>
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
