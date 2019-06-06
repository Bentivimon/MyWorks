using GraduateWork.Server.Models.Enums;

namespace GraduateWork.Server.Models.Response
{
    /// <summary>
    /// Model that show statement info.
    /// </summary>
    public class EntrantStatementDto
    {
        /// <summary>
        /// Gets/Sets university name.
        /// </summary>
        public string UniversityName { get; set; }

        /// <summary>
        /// Gets/Sets speciality name.
        /// </summary>
        public string SpecialityName { get; set; }

        /// <summary>
        /// Ges/Sets entrant score.
        /// </summary>
        public float EntrantScore { get; set; }
        
        /// <summary>
        /// Gets/Sets priority.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets/Sets statement status.
        /// </summary>
        public StatementStatus StatementStatus { get; set; }
        
    }
}
