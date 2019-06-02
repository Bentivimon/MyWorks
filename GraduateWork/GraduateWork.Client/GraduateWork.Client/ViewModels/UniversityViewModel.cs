using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraduateWork.Client.Models;
using GraduateWork.Client.Models.ResponseModels;
using GraduateWork.Client.Services;
using GraduateWork.Client.Views;
using Xamarin.Forms;

namespace GraduateWork.Client.ViewModels
{
    public class UniversityViewModel : BaseViewModel
    {
        private string _regionName;
        private List<UniversityDto> _universities;
        public ObservableCollection<UniversityListModel> Universities { get; set; }

        private SpecialitiesHttpClient _httpClient;

        public INavigation Navigation { get; set; }

        private UniversityListModel _universitiesSelectedItem;

        public UniversityListModel UniversitiesSelectedItem
        {
            get
            {
                return _universitiesSelectedItem;
            }

            set
            {
                _universitiesSelectedItem = value;
                OnPropertyChanged();
                ShowUniversityDetailPage();
            }
        }

        private async Task ShowUniversityDetailPage()
        {
            var university = _universities.First(x => x.FullName == _universitiesSelectedItem.FullName);
          
            await Navigation.PushAsync(new UniversityDetailsPage(university, _regionName), true);

            UniversitiesSelectedItem = null;
        }


        public UniversityViewModel(INavigation navigation, List<UniversityDto> universities, string regionName)
        {
            _regionName = regionName;
            _universities = universities;
            Navigation = navigation;
            UniversitiesSelectedItem = null;
            Universities = new ObservableCollection<UniversityListModel>(universities.Select(x=> new UniversityListModel{ FullName = x.FullName }));
        }

    }
}
