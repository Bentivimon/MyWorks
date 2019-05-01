using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using GraduateWork.Client.Models;
using GraduateWork.Client.Views;
using Xamarin.Forms;

namespace GraduateWork.Client.ViewModels
{
    public class UniversityDetailsViewModel : BaseViewModel
    {
        public ObservableCollection<UniversityInformation> UniversityData { get; set; }
        public ObservableCollection<object> Specialities { get; set; }
        public Command ShowAlertCommand { get; set; }
        public Command ShowSpecialityInfoCommand { get; set; }
        public INavigation Navigation { get; set; }


        string _universityName = string.Empty;
        public string UniversityName
        {
            get { return _universityName; }
            set { SetProperty(ref _universityName, value); }
        }

        public UniversityDetailsViewModel(string universityName, INavigation navigation)
        {
            UniversityName = universityName;
            Navigation = navigation;
            InitializeCollection();
            Specialities = new ObservableCollection<object>(){ new object(), new object(), new object()};
            ShowAlertCommand = new Command(async () => await ShowAlertWithRequirementsAsync());
            ShowSpecialityInfoCommand = new Command(async () => await ShowSpecialityInfoPageAsync());
        }

        private async Task ShowSpecialityInfoPageAsync()
        {
            await Navigation.PushAsync(new SpecialityInfoPage(), true);
        }

        private async Task ShowAlertWithRequirementsAsync()
        {
            await Application.Current.MainPage.DisplayAlert("Test",
                "Many test Data, Many test Data, Many test Data, Many test Data,", "Закрити");
        }

        private void InitializeCollection()
        {
            UniversityData = new ObservableCollection<UniversityInformation>()
            {
                new UniversityInformation()
                {
                    Title = "Тип ВНЗ",
                    Value = "Університет"
                },
                new UniversityInformation()
                {
                    Title = "Форма Власності",
                    Value = "Державна"
                },
                new UniversityInformation()
                {
                    Title = "Керівник",
                    Value = "Ректор блаблаблабалабалабалабалабалабалабл"
                },
                new UniversityInformation()
                {
                    Title = "Підпорядкування",
                    Value = "Міністерство охорони здоров*я"
                },
                new UniversityInformation()
                {
                    Title = "Поштовий індекс",
                    Value = "46001"
                },
                new UniversityInformation()
                {
                    Title = "Область, населений пункт",
                    Value = "Тернопільська область, Тернопіль"
                },
                new UniversityInformation()
                {
                    Title = "Адреса",
                    Value = "Блабалаблааблабалабалабала"
                },
                new UniversityInformation()
                {
                    Title = "Телефони",
                    Value = "2020293948483932"
                },
                new UniversityInformation()
                {
                    Title = "Веб-сайт",
                    Value = "www.tneu.edu.te.ua"
                },
                new UniversityInformation()
                {
                    Title = "Email",
                    Value = "university@tneu.edu.ua"
                },
            };
        }
    }
}
