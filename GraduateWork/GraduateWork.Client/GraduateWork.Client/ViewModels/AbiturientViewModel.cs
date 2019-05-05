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
            var score = entrant.CertificateOfTesting.FirstMark + entrant.CertificateOfTesting.SecondMark + entrant.CertificateOfTesting.ThirdMark;

            if (entrant.CertificateOfTesting.FourthMark.Equals(0))
            {
                score += entrant.CertificateOfTesting.FourthMark;
                score /= 4;
            }
            else
            {
                score /= 3;
            }

            Score = $"Конкурсний бал: {Math.Round(score, 1)}";

            FirstSubject =
                $"- {entrant.CertificateOfTesting.FirstSubject} (ЗНО): {entrant.CertificateOfTesting.FirstMark}";

            SecondSubject =
                $"- {entrant.CertificateOfTesting.SecondSubject} (ЗНО): {entrant.CertificateOfTesting.SecondMark}";

            ThirdSubject =
                $"- {entrant.CertificateOfTesting.ThirdSubject} (ЗНО): {entrant.CertificateOfTesting.ThirdMark}";

            SchoolAverageMark =
                $"- Середній бал документа про освіту: {entrant.CertificateOfSecondaryEducation.AverageMark}";

        }
    }
}
