using System.ComponentModel.DataAnnotations;
using EntityModels.Abstractions;

namespace Models.RequestModels.UniversityModels
{
    public class UnivesityModelRequest : IUniversity
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string LevelOfAccreditation { get; set; }
    }
}
