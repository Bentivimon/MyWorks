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
            SelectedAbiturient = null;
        }


        public INavigation Navigation { get; set; }

        public SpecialityInfoViewModel(INavigation navigation, string universityName, SpecialityDto speciality, List<EntrantDto> entrants)
        {
            _entrants = entrants;
            Navigation = navigation;
            SelectedAbiturient = null;
            SpecialityInfo =
                    $"Бакалавр (на основі ПЗСНО 11кл.). Спеціальність: {speciality.Code} {speciality.Name}. Факультет: {speciality.Faculty}";
            UniversityName = universityName;
            Abiturients = new ObservableCollection<EntrantListModel>();
            InitializeEntrants(entrants.OrderByDescending(x=> x.TotalScore).ToList());
        }

        private void InitializeEntrants(List<EntrantDto> entrants)
        {
            for (int i = 0; i < entrants.Count; i++)
            {
                Abiturients.Add(new EntrantListModel
                {
                    Id = entrants[i].Id,
                    Title = $"{i + 1}. {entrants[i].Surname} {entrants[i].Name}",
                    Score = $"Конкурсний бал: {entrants[i].TotalScore}"
                });
            }
        }
    }
}
