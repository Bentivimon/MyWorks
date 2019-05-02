using System;
using System.Collections.Generic;
using System.Text;

namespace GraduateWork.Client.Models.RequestModels
{
    /// <summary>
    /// Request model for user authorization.
    /// </summary>
    public class UserLoginModel
    {
        /// <summary>
        /// Gets/Sets user login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets/Sets user password.
        /// </summary>
        public string Password { get; set; }
    }
}
