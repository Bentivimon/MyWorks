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
            var result = await _httpClient.RegistrationAsync(new RegistrationModel
            {
                FirstName = _inputName,
                LastName = _inputSurname,
                Email = _inputEmail,
                Password = _inputPassword,
                MobileNumber = _inputNumber
            }).ConfigureAwait(false);

            await Navigation.PushAsync(new LoginPage(), true);
        }
    }
}
