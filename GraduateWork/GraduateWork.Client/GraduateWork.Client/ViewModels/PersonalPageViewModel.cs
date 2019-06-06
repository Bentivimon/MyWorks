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
    public class PersonalPageViewModel : BaseViewModel
    {
        private readonly AccountHttpClient _accountHttpClient;
        private readonly EntrantsHttpClient _entrantsHttpClient;
        private readonly INavigation _navigation;

        public ObservableCollection<EntrantStatementListModel> Statements { get; set; }
        public Command ShowAbiturientCommand { get; set; }
        public Command ShowSelectEntrantCommand { get; set; }

        private bool _isShowAbiturientButtonEnabled;

        public bool IsShowAbiturientButtonEnabled
        {
            get => _isShowAbiturientButtonEnabled;

            set
            {
                _isShowAbiturientButtonEnabled = value;
                OnPropertyChanged();
            }
        }


        private string _firstName;
        
        public string FirstName
        {
            get => _firstName;

            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        private string _lastName;

        public string LastName
        {
            get => _lastName;

            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        private string _email;

        public string Email
        {
            get => _email;

            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _mobilePhone;

        public string MobilePhone
        {
            get => _mobilePhone;

            set
            {
                _mobilePhone = value;
                OnPropertyChanged();
            }
        }

        public PersonalPageViewModel(UserInfo userInfo, INavigation navigation)
        {
            _navigation = navigation;
            _accountHttpClient = new AccountHttpClient();
            _entrantsHttpClient = new EntrantsHttpClient();
            FirstName = userInfo.FirstName;
            LastName = userInfo.LastName;
            Email = userInfo.Email;
            MobilePhone = userInfo.MobileNumber;
            Statements = new ObservableCollection<EntrantStatementListModel>();
            IsShowAbiturientButtonEnabled = false;
            ShowAbiturientCommand = new Command(async ()=> await ShowAbiturientPageAsync(userInfo));
            ShowSelectEntrantCommand = new Command(async () => await ShowSelectEntrantPage());

            if (userInfo.Statements != null && userInfo.Statements.Any())
            {
                InitializeStatements(userInfo.Statements);
                IsShowAbiturientButtonEnabled = true;
            }
        }

        private async Task ShowSelectEntrantPage()
        {
            await _navigation.PushAsync(new SelectEntrantPage(), true);
        }

        private async Task ShowAbiturientPageAsync(UserInfo userInfo)
        {
            var entrant =
                await _entrantsHttpClient.GetEntrantByIdAsync(Application.Current.Properties["token"].ToString(),
                    userInfo.EntrantId);

            await _navigation.PushAsync(new AbiturientPage(entrant), true);
        }

        private void InitializeStatements(List<EntrantStatementDto> statements)
        {
            string color= null, status = null;
            foreach (var statement in statements)
            {
                switch (statement.StatementStatus)
                {
                    case StatementStatus.Accepted:
                    {
                        color = "Green";
                        status = "До наказу";
                        break;
                    }
                    case StatementStatus.Holding:
                    {
                        color = "Yellow";
                        status = "В очікуванні";
                        break;
                    }
                    case StatementStatus.Rejected:
                    {
                        color = "Red";
                        status = "Відмолено";
                        break;
                    }
                }
                var listModel = new EntrantStatementListModel
                {
                    UniversityName = statement.UniversityName,
                    SpecialityName = statement.SpecialityName,
                    Priority = statement.Priority,
                    EntrantScore = $"{statement.EntrantScore}",
                    Status = status,
                    Color = color
                };
                Statements.Add(listModel);
            }
        }
    }
}
