using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using GraduateWork.Client.Models;
using GraduateWork.Client.Views;
using Xamarin.Forms;

namespace GraduateWork.Client.ViewModels
{
    public class LocationViewModel: BaseViewModel
    {
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

        public LocationViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Locations = new ObservableCollection<LocationListModel>(){new LocationListModel(){Region = "Тернопільська обл."}, new LocationListModel() { Region = "Тернопільська обл." }, new LocationListModel() { Region = "Тернопільська обл." }, new LocationListModel() { Region = "Тернопільська обл." } , new LocationListModel() { Region = "Тернопільська обл." } , new LocationListModel() { Region = "Тернопільська обл." } , new LocationListModel() { Region = "Тернопільська обл." } };
        }

        private void ShowDetailPage()
        {
            Navigation.PushAsync(new UniversityPage(), true);
        }
    }
}
