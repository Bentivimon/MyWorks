using System.ComponentModel.DataAnnotations;
using EntityModels.Abstractions;

namespace Models.RequestModels.SpecialityModels
{
    public class SpecialityRequest : ISpeciality
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public float AdditionalFactor { get; set; }

        [Required]
        public int CountOfStatePlaces { get; set; }
    }
}
