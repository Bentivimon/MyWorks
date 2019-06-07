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
    public class SelectEntrantPageViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private readonly EntrantsHttpClient _entrantsHttpClient;
        private readonly AccountHttpClient _accountHttpClient;
        private List<EntrantDto> _entrants;
        public ObservableCollection<EntrantListModel> Abiturients { get; set; }
        public Command FindAbiturientsCommand { get; set; }

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
                SetAbiturientAsync();
            }
        }

        private string _searchQuery;

        public string SearchQuery
        {
            get => _searchQuery;

            set
            {
                _searchQuery = value;
                OnPropertyChanged();
            }
        }

        private async Task SetAbiturientAsync()
        {
            await _entrantsHttpClient.CombineEntrantAndUserAsync(Application.Current.Properties["token"].ToString(),
                _selectedAbiturient.Id);
            
            var k = await _navigation.PopAsync(true);
            k.SendAppearing();
            SelectedAbiturient = null;
        }

        public SelectEntrantPageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _entrantsHttpClient = new EntrantsHttpClient();
            _accountHttpClient = new AccountHttpClient();
            Abiturients = new ObservableCollection<EntrantListModel>();

            FindAbiturientsCommand = new Command(async () => await FindAbiturientsAsync());
        }

        private async Task FindAbiturientsAsync()
        {
            _entrants = await _entrantsHttpClient.GetEntrantsByNameAsync(Application.Current.Properties["token"].ToString(),
                    _searchQuery);

            Abiturients.Clear();

            for (int i = 0; i < _entrants.Count; i++)
            {
                Abiturients.Add(new EntrantListModel
                {
                    Id = _entrants[i].Id,
                    Title = $"{i + 1}. {_entrants[i].Surname} {_entrants[i].Name}",
                    Score = $"Конкурсний бал: {_entrants[i].TotalScore}"
                });
            }
        }
    }
}
