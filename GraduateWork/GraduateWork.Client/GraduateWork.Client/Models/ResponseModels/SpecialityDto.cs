namespace GraduateWork.Client.Models.ResponseModels
{
    /// <summary>
    /// Speciality response model.
    /// </summary>
    public class SpecialityDto
    {
        /// <summary>
        /// Gets/Sets code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets/Sets name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets/Sets faculty factor.
        /// </summary>
        public string Faculty { get; set; }

        /// <summary>
        /// Gets/Sets count of state places.
        /// </summary>
        public string SubjectScores { get; set; }
    }
}
