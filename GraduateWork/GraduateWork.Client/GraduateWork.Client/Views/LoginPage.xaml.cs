using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraduateWork.Client.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GraduateWork.Client.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
	    private LoginViewModel _viewModel;
		public LoginPage ()
		{
			InitializeComponent ();
		    BindingContext = _viewModel = new LoginViewModel(Navigation);
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = _viewModel = new LoginViewModel(Navigation);
        }
    }
}