using System;

namespace GraduateWork.Server.Models.Response
{
    /// <summary>
    /// Represent entrant response model.
    /// </summary>
    public class EntrantDto
    {
        /// <summary>
        /// Gets/Sets entrant id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets/Sets entrant name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets/Sets entrant surname.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Gets/Sets entrant birth day.
        /// </summary>
        public DateTime BDay { get; set; }
    }
}
