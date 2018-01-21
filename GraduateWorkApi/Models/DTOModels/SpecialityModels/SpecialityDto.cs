using EntityModels.Abstractions;

namespace Models.DTOModels.SpecialityModels
{
    public class SpecialityDto : ISpeciality
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public float AdditionalFactor { get; set; }
        public int CountOfStatePlaces { get; set; }

        public SpecialityDto(ISpeciality speciality)
        {
            Code = speciality.Code;
            Name = speciality.Name;
            AdditionalFactor = speciality.AdditionalFactor;
            CountOfStatePlaces = speciality.CountOfStatePlaces;
        }
    }
}
