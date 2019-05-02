using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using GraduateWork.Client.Views;
using Xamarin.Forms;

namespace GraduateWork.Client.ViewModels
{
    public class SpecialityInfoViewModel : BaseViewModel
    {
        public ObservableCollection<object> Abiturients { get; set; }

        private object _selectedAbiturient;

        public object SelectedAbiturient
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

        private void ShowAbiturientPage()
        {
            Navigation.PushAsync(new AbiturientPage(), true);
        }


        public INavigation Navigation { get; set; }

        public SpecialityInfoViewModel(INavigation navigation)
        {
            Navigation = navigation;

            Abiturients = new ObservableCollection<object>(){new object(), new object(), new object(), new object(), new object(), new object(), new object(), new object(), new object(), new object(), new object()};
        }
    }
}
