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
            BindingContext = _viewModel = new PersonalPageViewModel(userInfo, Navigation);
        }
	}
}