using System;

namespace GraduateWork.Server.Models.Request
{
    /// <summary>
    /// Represent entrant request model.
    /// </summary>
    public class EntrantModel
    {
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
