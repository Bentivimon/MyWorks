using System;

namespace GraduateWork.Server.Models.Response
{
    public class StatementDto
    {
         /// <summary>
        /// Gets/Sets uniq entrant identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets/Sets statement total score.
        /// </summary>
        public float TotalScore { get; set; }

        /// <summary>
        /// Gets/Sets statement extra score.
        /// </summary>
        public float ExtraScore { get; set; }

        /// <summary>
        /// Gets/Sets statement priority.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets/Sets is accepted
        /// </summary>
        public bool IsAccepted { get; set; }

        /// <summary>
        /// Gets/Sets statement associated with entrant by id.
        /// </summary>
        public Guid EntrantId { get; set; }

        /// <summary>
        /// Gets/Sets statement associated with speciality by id.
        /// </summary>
        public Guid SpecialityId { get; set; }
    }
}
