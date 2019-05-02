using System;
using System.Collections.Generic;
using System.Text;

namespace GraduateWork.Client.Models.RequestModels
{
    /// <summary>
    /// Represent registration model for user.
    /// </summary>
    public class RegistrationModel
    {
        /// <summary>
        /// Gets/Sets user email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets/Sets user password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets/Sets user first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets/Sets user last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets/Sets user mobile phone.
        /// </summary>
        public string MobileNumber { get; set; }

        /// <summary>
        /// Gets/Sets user birthday.
        /// </summary>
        public DateTime Birthday { get; set; }
    }
}
