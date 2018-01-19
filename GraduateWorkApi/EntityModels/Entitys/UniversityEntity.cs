using System;
using System.Collections.Generic;
using System.Text;

namespace EntityModels.Entitys
{
    public class UniversityEntity
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string LevelOfAccreditation { get; set; }

        public List<StatementEntity> Statements { get; set; }
        public List<UniversitySpeciality> UniversitySpecialities { get; set; }
    }
}
