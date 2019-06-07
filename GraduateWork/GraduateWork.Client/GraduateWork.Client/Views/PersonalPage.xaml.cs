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
        
        public PersonalPage ()
		{
			InitializeComponent ();
            BindingContext = _viewModel = new PersonalPageViewModel(Navigation);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = _viewModel = new PersonalPageViewModel(Navigation);
        }
    }
}