using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GraduateWork.Client.Views;
using Xamarin.Forms;

namespace GraduateWork.Client.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command RegistrationCommand { get; set; }
        public Command LoginCommand { get; set; }
        public INavigation Navigation { get; set; }

        public LoginViewModel(INavigation navigation)
        {
            Navigation = navigation;
            RegistrationCommand = new Command(async () => await NavigateToRegistrationPage());
            LoginCommand = new Command(async () => await LoginAsync());
        }

        private async Task LoginAsync()
        {
            await Navigation.PushAsync(new LocationPage(), true);
        }

        private async Task NavigateToRegistrationPage()
        {
            await Navigation.PushAsync(new RegistrationPage(), true);
            
        }
    }
}
