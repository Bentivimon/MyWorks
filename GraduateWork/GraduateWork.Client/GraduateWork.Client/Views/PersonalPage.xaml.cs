using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraduateWork.Client.Models.ResponseModels;
using GraduateWork.Client.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GraduateWork.Client.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PersonalPage : ContentPage
	{
        private PersonalPageViewModel _viewModel;

        public PersonalPage()
        {
            InitializeComponent();
        }

        public PersonalPage (UserInfo userInfo)
		{
			InitializeComponent ();
            BindingContext = _viewModel = new PersonalPageViewModel(userInfo);
        }
	}
}