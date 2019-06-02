using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public class LocationViewModel: BaseViewModel
    {
        private UniversitiesHttpClient _httpClient;
        private List<RegionDto> _regions;
        public ObservableCollection<LocationListModel> Locations { get; set; }

        public INavigation Navigation { get; set; }

        private LocationListModel _locationsSelectedItem;

        public LocationListModel LocationsSelectedItem
        {
            get
            {
                return _locationsSelectedItem;
            }

            set
            {
                _locationsSelectedItem = value;
                OnPropertyChanged();
                ShowDetailPage();
            }
        }

        public LocationViewModel(INavigation navigation, List<RegionDto> regions)
        {
            Navigation = navigation;
            _regions = regions;
            Locations = null;
            Locations = new ObservableCollection<LocationListModel>(regions.Select(x=> new LocationListModel{Region = x.Name}));
            _httpClient = new UniversitiesHttpClient();
        }

        private async Task ShowDetailPage()
        {
            var university = await _httpClient.GetUniversitiesByRegionIdAsync(
                Application.Current.Properties["token"].ToString(),
                _regions.First(x => x.Name == _locationsSelectedItem.Region).Id);

            await Navigation.PushAsync(new UniversityPage(university, _locationsSelectedItem.Region), true);
            LocationsSelectedItem = null;
        }
    }
}
