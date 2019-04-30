namespace GraduateWork.Server.Models.Response
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
        /// Gets/Sets additional factor.
        /// </summary>
        public float AdditionalFactor { get; set; }

        /// <summary>
        /// Gets/Sets count of state places.
        /// </summary>
        public int CountOfStatePlaces { get; set; }
    }
}
