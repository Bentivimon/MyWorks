using System;
using System.Collections.Generic;

namespace GraduateWork.Client.Models.ResponseModels
{
    public class UserInfo
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

        /// <summary>
        /// Gets/Sets entrant id.
        /// </summary>
        public Guid EntrantId { get; set; }

        /// <summary>
        /// Gets/Sets entrant first name.
        /// </summary>
        public string EntrantFistName { get; set; }

        /// <summary>
        /// Gets/Sets entrant last name.
        /// </summary>
        public string EntrantLastName { get; set; }

        /// <summary>
        /// Gets/Sets attached entrant.
        /// </summary>
        public List<EntrantStatementDto> Statements { get; set; }
    }
}
