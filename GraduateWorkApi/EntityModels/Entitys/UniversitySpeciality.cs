using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EntityModels.Entitys
{
    public class UniversitySpeciality
    {
        [ForeignKey("University")]
        public int UniversityId { get; set; }
        public UniversityEntity University { get; set; }

        [ForeignKey("Specialty")]
        public int SpecialtyId { get; set; }
        public SpecialityEntity Specialty { get; set; }
    }
}
