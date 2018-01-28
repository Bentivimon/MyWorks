using System;

namespace EntityModels.Abstractions
{
    public interface ICertificateOfSecondaryEducation
    {
        string SeriaNumber { get; set; }
        float AverageMark { get; set; }
        string FullNameOfTheEducationalInstitution { get; set; }
        DateTime YearOfIssue { get; set; }
    }
}
