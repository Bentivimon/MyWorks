using System;

namespace GraduateWork.Server.Models.Response
{
    /// <summary>
    /// Represent user response model.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Gets/Sets user email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets/Sets user first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets/Sets user last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets/Sets user mobile number.
        /// </summary>
        public string MobileNumber { get; set; }

        /// <summary>
        /// Gets/Sets user birthday.
        /// </summary>
        public DateTime Birthday { get; set; }
    }
}
