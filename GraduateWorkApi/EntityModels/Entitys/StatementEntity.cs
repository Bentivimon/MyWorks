using System.ComponentModel.DataAnnotations.Schema;

namespace EntityModels.Entitys
{
    public class StatementEntity
    {
        public int Id { get; set; }
        public float TotalScore { get; set; }
        public float ExtraScore { get; set; }

        [ForeignKey("Entrant")]
        public int EntrantId { get; set; }
        public EntrantEntity Entrant { get; set; }

        [ForeignKey("University")]
        public int UniversityId { get; set; }
        public UniversityEntity University { get; set; }
    }
}
