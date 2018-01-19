using System.Collections.Generic;

namespace EntityModels.Entitys
{
    public class SpecialtyEntity
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public float AdditionalFactor { get; set; }
        public int CountOfStatePlaces { get; set; }

        public List<UniversitySpeciality> UniversitySpecialities { get; set; }   
    }
}
