using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraduateWork.Client.Models;
using GraduateWork.Client.Models.ResponseModels;
using GraduateWork.Client.Views;
using Xamarin.Forms;

namespace GraduateWork.Client.ViewModels
{
    public class SpecialityInfoViewModel : BaseViewModel
    {
        private readonly List<EntrantDto> _entrants;
        public ObservableCollection<EntrantListModel> Abiturients { get; set; }

        private EntrantListModel _selectedAbiturient;

        public EntrantListModel SelectedAbiturient
        {
            get
            {
                return _selectedAbiturient;
            }

            set
            {
                _selectedAbiturient = value;
                OnPropertyChanged();
                ShowAbiturientPage();
            }
        }

        public string SpecialityInfo { get; set; }
        public string UniversityName { get; set; }

        private async Task ShowAbiturientPage()
        {
            var entrant = _entrants.First(x => x.Id == _selectedAbiturient.Id);
            await Navigation.PushAsync(new AbiturientPage(entrant), true);
        }


        public INavigation Navigation { get; set; }

        public SpecialityInfoViewModel(INavigation navigation, string universityName, SpecialityDto speciality, List<EntrantDto> entrants)
        {
            _entrants = entrants;
            Navigation = navigation;
            SpecialityInfo =
                    $"Бакалавр (на основі ПЗСНО 11кл.). Спеціальність: {speciality.Code} {speciality.Name}. Факультет: {speciality.Faculty}";
            UniversityName = universityName;
            Abiturients = new ObservableCollection<EntrantListModel>();
            InitializeEntrants(entrants);
        }

        private void InitializeEntrants(List<EntrantDto> entrants)
        {
            for (int i = 0; i < entrants.Count; i++)
            {
                var score = entrants[i].CertificateOfTesting.FirstMark + entrants[i].CertificateOfTesting.SecondMark + entrants[i].CertificateOfTesting.ThirdMark;

                if (entrants[i].CertificateOfTesting.FourthMark.Equals(0))
                {
                    score += entrants[i].CertificateOfTesting.FourthMark;
                    score /= 4;
                }
                else
                {
                    score /= 3;
                }

                Abiturients.Add(new EntrantListModel
                {
                    Id = entrants[i].Id,
                    Title = $"{i + 1}. {entrants[i].Surname} {entrants[i].Name}.",
                    Score = $"Конкурсний бал: {Math.Round(score, 1)}"
                });
            }
        }
    }
}
