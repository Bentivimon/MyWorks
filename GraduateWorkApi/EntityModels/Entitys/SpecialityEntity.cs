using System.Collections.Generic;
using EntityModels.Abstractions;

namespace EntityModels.Entitys
{
    public class SpecialityEntity : ISpeciality
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public float AdditionalFactor { get; set; }
        public int CountOfStatePlaces { get; set; }

        public List<UniversitySpeciality> UniversitySpecialities { get; set; }

        public SpecialityEntity(ISpeciality speciality)
        {
            Code = speciality.Code;
            Name = speciality.Name;
            AdditionalFactor = speciality.AdditionalFactor;
            CountOfStatePlaces = speciality.CountOfStatePlaces;
        }
    }
}
