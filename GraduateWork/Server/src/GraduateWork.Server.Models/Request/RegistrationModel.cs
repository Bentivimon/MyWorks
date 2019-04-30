using System;
using System.ComponentModel.DataAnnotations;

namespace GraduateWork.Server.Models.Request
{
    /// <summary>
    /// Represent registration model for user.
    /// </summary>
    public class RegistrationModel
    {
        /// <summary>
        /// Gets/Sets user email.
        /// </summary>
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// Gets/Sets user password.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Gets/Sets user first name.
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets/Sets user last name.
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Gets/Sets user mobile phone.
        /// </summary>
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string MobileNumber { get; set; }

        /// <summary>
        /// Gets/Sets user birthday.
        /// </summary>
        [Required]
        public DateTime Birthday { get; set; }
    }
}
