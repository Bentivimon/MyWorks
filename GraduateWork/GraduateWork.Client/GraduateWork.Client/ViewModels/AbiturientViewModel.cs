using System;
using GraduateWork.Client.Models.ResponseModels;

namespace GraduateWork.Client.ViewModels
{
    public class AbiturientViewModel : BaseViewModel
    {
        public string NameSurname { get; set; }
        public string Score { get; set; }
        public string FirstSubject { get; set; }
        public string SecondSubject { get; set; }
        public string ThirdSubject { get; set; }
        public string SchoolAverageMark { get; set; }

        public AbiturientViewModel(EntrantDto entrant)
        {
            NameSurname = $"{entrant.Surname} {entrant.Name}";

            Score = $"Конкурсний бал: {entrant.TotalScore}";

            FirstSubject =
                $"- {entrant.CertificateOfTesting.FirstSubject}: {entrant.CertificateOfTesting.FirstMark}";

            SecondSubject =
                $"- {entrant.CertificateOfTesting.SecondSubject}: {entrant.CertificateOfTesting.SecondMark}";

            ThirdSubject =
                $"- {entrant.CertificateOfTesting.ThirdSubject}: {entrant.CertificateOfTesting.ThirdMark}";

            SchoolAverageMark =
                $"- Середній бал документа про освіту: {entrant.CertificateOfSecondaryEducation.AverageMark}";

        }
    }
}
