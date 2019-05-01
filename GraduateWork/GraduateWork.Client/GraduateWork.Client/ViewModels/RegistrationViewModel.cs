using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GraduateWork.Client.Views;
using Xamarin.Forms;

namespace GraduateWork.Client.ViewModels
{
    public class RegistrationViewModel: BaseViewModel
    {
        public Command RegistrationCommand { get; set; }
        public INavigation Navigation { get; set; }

        public RegistrationViewModel(INavigation navigation)
        {
            RegistrationCommand = new Command(async() => await RegistrationAsync());
            Navigation = navigation;
        }

        private async Task RegistrationAsync()
        {
            await Navigation.PushAsync(new LoginPage(), true);
        }
    }
}
