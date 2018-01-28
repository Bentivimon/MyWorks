using System;
using System.ComponentModel.DataAnnotations.Schema;
using EntityModels.Abstractions;

namespace EntityModels.Entitys
{
    public class CertificateOfSecondaryEducationEntity : ICertificateOfSecondaryEducation
    {
        public int Id { get; set; }
        public string SeriaNumber { get; set; }
        public float AverageMark { get; set; }
        public string FullNameOfTheEducationalInstitution { get; set; }
        public DateTime YearOfIssue { get; set; }

        [ForeignKey("Entrant")]
        public int? EntrantId { get; set; }
        public EntrantEntity Entrant { get; set; }
    }
}
