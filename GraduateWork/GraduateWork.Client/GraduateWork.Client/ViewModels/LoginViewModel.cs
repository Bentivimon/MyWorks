using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GraduateWork.Client.Models.RequestModels;
using GraduateWork.Client.Services;
using GraduateWork.Client.Views;
using Xamarin.Forms;

namespace GraduateWork.Client.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly AccountHttpClient _httpClient;
        private readonly RegionsHttpClient _regionsHttpClient;
        public Command RegistrationCommand { get; set; }
        public Command LoginCommand { get; set; }
        public INavigation Navigation { get; set; }

        private string _inputEmail;

        public string InputEmail
        {
            get => _inputEmail; 

            set
            {
                _inputEmail = value;
                OnPropertyChanged();
            }
        }

        private string _inputPassword;

        public string InputPassword
        {
            get => _inputPassword;

            set
            {
                _inputPassword = value;
                OnPropertyChanged();
            }
        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get => _errorMessage;

            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public LoginViewModel(INavigation navigation)
        {
            Navigation = navigation;
            RegistrationCommand = new Command(async () => await NavigateToRegistrationPage());
            LoginCommand = new Command(async () => await LoginAsync());
            _httpClient = new AccountHttpClient();
            _regionsHttpClient = new RegionsHttpClient();
        }

        private async Task LoginAsync()
        {
            var token = await _httpClient.LoginAsync(new UserLoginModel
            {
                Login = _inputEmail,
                Password = _inputPassword
            });

            if (string.IsNullOrEmpty(token))
            {
                ErrorMessage = "Невірний Email або пароль.";
                InputPassword = "";
                return;
            }

            Application.Current.Properties["token"] = token;
            var regions =
                await _regionsHttpClient.GetAllRegionsAsync(Application.Current.Properties["token"].ToString());
            await Navigation.PushAsync(new LocationPage(regions), true);
        }

        private async Task NavigateToRegistrationPage()
        {
            await Navigation.PushAsync(new RegistrationPage(), true);
            
        }
    }
}
