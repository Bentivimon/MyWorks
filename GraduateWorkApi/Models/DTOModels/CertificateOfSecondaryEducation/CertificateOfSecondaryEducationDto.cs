using System;
using EntityModels.Abstractions;

namespace Models.DTOModels.CertificateOfSecondaryEducation
{
    public class CertificateOfSecondaryEducationDto : ICertificateOfSecondaryEducation
    {
        public string SeriaNumber { get; set; }
        public float AverageMark { get; set; }
        public string FullNameOfTheEducationalInstitution { get; set; }
        public DateTime YearOfIssue { get; set; }

        public CertificateOfSecondaryEducationDto(ICertificateOfSecondaryEducation certificateOfSecondaryEducation)
        {
            SeriaNumber = certificateOfSecondaryEducation.SeriaNumber;
            AverageMark = certificateOfSecondaryEducation.AverageMark;
            FullNameOfTheEducationalInstitution = certificateOfSecondaryEducation.FullNameOfTheEducationalInstitution;
            YearOfIssue = certificateOfSecondaryEducation.YearOfIssue;

        }
    }
}
