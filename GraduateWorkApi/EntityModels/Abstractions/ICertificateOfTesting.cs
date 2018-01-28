using System;

namespace EntityModels.Abstractions
{
    public interface ICertificateOfTesting
    {
        string SerialNumber { get; set; }
        DateTime YearOfIssue { get; set; }
        string FirstSubject { get; set; }
        string SecondSubject { get; set; }
        string ThirdSubject { get; set; }
        string FourthSubject { get; set; }
        float FirstMark { get; set; }
        float SecondMark { get; set; }
        float ThirdMark { get; set; }
        float FourthMark { get; set; }
    }
}
