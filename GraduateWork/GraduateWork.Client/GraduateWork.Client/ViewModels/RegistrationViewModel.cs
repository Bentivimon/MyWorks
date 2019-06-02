using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GraduateWork.Client.Models.RequestModels;
using GraduateWork.Client.Services;
using GraduateWork.Client.Views;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace GraduateWork.Client.ViewModels
{
    public class RegistrationViewModel: BaseViewModel
    {
        private readonly AccountHttpClient _httpClient;
        public Command RegistrationCommand { get; set; }
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

        private string _inputConfirmationPassword;

        public string InputConfirmationPassword
        {
            get => _inputConfirmationPassword;

            set
            {
                _inputConfirmationPassword = value;
                OnPropertyChanged();
            }
        }

        private string _inputName;

        public string InputName
        {
            get => _inputName;

            set
            {
                _inputName = value;
                OnPropertyChanged();
            }
        }

        private string _inputSurname;

        public string InputSurname
        {
            get => _inputSurname;

            set
            {
                _inputSurname = value;
                OnPropertyChanged();
            }
        }

        private string _inputNumber;

        public string InputNumber
        {
            get => _inputNumber;

            set
            {
                _inputNumber = value;
                OnPropertyChanged();
            }
        }
        
        public RegistrationViewModel(INavigation navigation)
        {
            _httpClient = new AccountHttpClient();
            RegistrationCommand = new Command(async() => await RegistrationAsync());
            Navigation = navigation;
        }
        
        private async Task RegistrationAsync()
        {
            if (!IsValidEmail(_inputEmail))
            {
                await Application.Current.MainPage.DisplayAlert("Помилка реєстрації", "Невалідна пошта", "Закрити");
                InputEmail = "";
                return;
            }

            var regex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,15}$");

            if (!regex.IsMatch(_inputPassword))
            {
                await Application.Current.MainPage.DisplayAlert("Помилка реєстрації", "Невалідний пароль", "Закрити");
                InputPassword = "";
                InputConfirmationPassword = "";
                return;
            }

            if (_inputPassword != _inputConfirmationPassword)
            {
                await Application.Current.MainPage.DisplayAlert("Помилка реєстрації", "Паролі не співпадають", "Закрити");
                InputPassword = "";
                InputConfirmationPassword = "";
                return;
            }

            var result = await _httpClient.RegistrationAsync(new RegistrationModel
            {
                FirstName = _inputName,
                LastName = _inputSurname,
                Email = _inputEmail,
                Password = _inputPassword,
                MobileNumber = _inputNumber
            });

            if (!result)
            {
                await Application.Current.MainPage.DisplayAlert("Помилка реєстрації", "Дана пошта вже використовується", "Закрити");
                InputEmail = "";
                InputPassword = "";
                InputConfirmationPassword = "";
                return;
            }

            await Navigation.PopAsync(true);
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
