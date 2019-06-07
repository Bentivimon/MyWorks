using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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

        private bool _isLoginButtonEnabled;

        public bool IsLoginButtonEnabled
        {
            get => _isLoginButtonEnabled;
            set
            {
                _isLoginButtonEnabled = value;
                OnPropertyChanged();
            }
        }

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
            IsLoginButtonEnabled = true;
        }

        private async Task LoginAsync()
        {
            IsLoginButtonEnabled = false;
            if (!IsValidEmail(_inputEmail))
            {
                await Application.Current.MainPage.DisplayAlert("Помилка авторизації", "Невалідна пошта", "Закрити");
                InputEmail = "";
                IsLoginButtonEnabled = true;
                return;
            }

            var regex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,15}$");

            if (!regex.IsMatch(_inputPassword))
            {
                await Application.Current.MainPage.DisplayAlert("Помилка авторизації", "Невалідний пароль", "Закрити");
                InputPassword = "";
                IsLoginButtonEnabled = true;
                return;
            }
            
            var token = await _httpClient.LoginAsync(new UserLoginModel
            {
                Login = _inputEmail,
                Password = _inputPassword
            });

            if (string.IsNullOrEmpty(token))
            {
                await Application.Current.MainPage.DisplayAlert("Помилка авторизації", "Невірна пошта або пароль.", "Закрити");
                InputPassword = "";
                IsLoginButtonEnabled = true;
                return;
            }

            Application.Current.Properties["token"] = token;
            var regions =
                await _regionsHttpClient.GetAllRegionsAsync(Application.Current.Properties["token"].ToString());
            await Navigation.PushAsync(new LocationPage(regions), true);

            IsLoginButtonEnabled = true;
        }

        private async Task NavigateToRegistrationPage()
        {
            await Navigation.PushAsync(new RegistrationPage(), true);
            
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var address = new System.Net.Mail.MailAddress(email);
                return address.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
