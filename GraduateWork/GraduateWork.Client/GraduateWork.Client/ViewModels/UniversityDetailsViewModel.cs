using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GraduateWork.Client.Models;
using GraduateWork.Client.Models.ResponseModels;
using GraduateWork.Client.Services;
using GraduateWork.Client.Views;
using Xamarin.Forms;

namespace GraduateWork.Client.ViewModels
{
    public class UniversityDetailsViewModel : BaseViewModel
    {
        private UniversityDto _university;
        private List<SpecialityDto> _specialities;
        private EntrantsHttpClient _httpClient;

        public ObservableCollection<UniversityInformation> UniversityData { get; set; }
        public ObservableCollection<SpecialityListModel> Specialities { get; set; }
        public Command ShowAlertCommand { get; set; }
        public Command ShowSpecialityInfoCommand { get; set; }
        public INavigation Navigation { get; set; }

        private SpecialityListModel _specialitySelectedItem;

        public SpecialityListModel SpecialitySelectedItem
        {
            get => _specialitySelectedItem;
            set
            {
                _specialitySelectedItem = value;
                OnPropertyChanged();
            }
        }

        string _universityName = string.Empty;
        public string UniversityName
        {
            get { return _universityName; }
            set { SetProperty(ref _universityName, value); }
        }

        public UniversityDetailsViewModel(INavigation navigation, UniversityDto university, List<SpecialityDto> specialities, string regionName)
        {
            _university = university;
            _specialities = specialities;
            UniversityName = university.FullName;
            Navigation = navigation;
            SpecialitySelectedItem = null;
            InitializeCollection(regionName);
            Specialities = new ObservableCollection<SpecialityListModel>(specialities.Select(x=> new SpecialityListModel{ Speciality = x.Name, Faculty = x.Faculty}));
            ShowAlertCommand = new Command<object>(async (arg) => await ShowAlertWithRequirementsAsync(arg));
            ShowSpecialityInfoCommand = new Command<object>(async (arg) => await ShowSpecialityInfoPageAsync(arg));
            _httpClient = new EntrantsHttpClient();
        }

        private async Task ShowSpecialityInfoPageAsync(object arg)
        {
            var specialityName = arg as string;
            var speciality = _specialities.First(x =>
                x.Name == specialityName);

            var entrants =
                await _httpClient.GetEntrantsBySpecialityIdAsync(Application.Current.Properties["token"].ToString(),
                    speciality.Id);

            await Navigation.PushAsync(new SpecialityInfoPage(_universityName, speciality, entrants), true);
        }

        private async Task ShowAlertWithRequirementsAsync(object arg)
        {
            var specialityName = arg as string;
            var speciality = _specialities.First(x =>
                x.Name == specialityName);
            await Application.Current.MainPage.DisplayAlert("Складові конкурсного балу",
                speciality.SubjectScores, "Закрити");
        }

        private void InitializeCollection(string regionName)
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
                    Value = _university.Ownership
                },
                new UniversityInformation()
                {
                    Title = "Керівник",
                    Value = _university.Chief
                },
                new UniversityInformation()
                {
                    Title = "Підпорядкування",
                    Value = _university.Subordination
                },
                new UniversityInformation()
                {
                    Title = "Поштовий індекс",
                    Value = _university.PostIndex
                },
                new UniversityInformation()
                {
                    Title = "Область, населений пункт",
                    Value = regionName
                },
                new UniversityInformation()
                {
                    Title = "Адреса",
                    Value = _university.Address
                },
                new UniversityInformation()
                {
                    Title = "Телефони",
                    Value = _university.Phone
                },
                new UniversityInformation()
                {
                    Title = "Веб-сайт",
                    Value = _university.Site
                },
                new UniversityInformation()
                {
                    Title = "Email",
                    Value = _university.Email
                },
            };
        }
    }
}
