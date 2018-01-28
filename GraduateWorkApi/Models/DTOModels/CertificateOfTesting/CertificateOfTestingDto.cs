using System;
using EntityModels.Abstractions;

namespace Models.DTOModels.CertificateOfTesting
{
    public class CertificateOfTestingDto : ICertificateOfTesting
    {
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

        public CertificateOfTestingDto(ICertificateOfTesting certificateOfTesting)
        {
            SerialNumber = certificateOfTesting.SerialNumber;
            YearOfIssue = certificateOfTesting.YearOfIssue;
            FirstSubject = certificateOfTesting.FirstSubject;
            SecondSubject = certificateOfTesting.SecondSubject;
            ThirdSubject = certificateOfTesting.ThirdSubject;
            FourthSubject = certificateOfTesting.FourthSubject;
            FirstMark = certificateOfTesting.FirstMark;
            SecondMark = certificateOfTesting.SecondMark;
            ThirdMark = certificateOfTesting.ThirdMark;
            FourthMark = certificateOfTesting.FourthMark;
        }
    }
}
