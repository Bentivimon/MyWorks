using System.Collections.Generic;
using EntityModels.Abstractions;

namespace EntityModels.Entitys
{
    public class UniversityEntity : IUniversity
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string LevelOfAccreditation { get; set; }

        public List<StatementEntity> Statements { get; set; }
        public List<UniversitySpeciality> UniversitySpecialities { get; set; }
    }
}
