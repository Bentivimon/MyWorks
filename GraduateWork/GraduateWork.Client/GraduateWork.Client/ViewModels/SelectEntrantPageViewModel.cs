using System;
using System.Collections.Generic;
using System.Text;
using GraduateWork.Client.Services;
using Xamarin.Forms;

namespace GraduateWork.Client.ViewModels
{
    public class SelectEntrantPageViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private readonly EntrantsHttpClient _entrantsHttpClient;

        public SelectEntrantPageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _entrantsHttpClient = new EntrantsHttpClient();
        }
    }
}
