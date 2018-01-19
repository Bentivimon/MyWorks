using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityModels.Entitys
{
    public class CertificateOfTestingEntity
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public DateTime YearOfIssue { get; set; }
        public string FirstSubject { get; set; }
        public string SecondSubject { get; set; }
        public string ThirdSubject { get; set; }
        public string FourthSubject { get; set; }
        public float FirstMark { get; set; }
        public float SecondMark { get; set; }
        public float ThirdMark { get; set; }
        public float FourthMark { get; set; }

        [ForeignKey("Entrant")]
        public int? EntrantId { get; set; }
        public EntrantEntity Entrant { get; set; }
    }
}
