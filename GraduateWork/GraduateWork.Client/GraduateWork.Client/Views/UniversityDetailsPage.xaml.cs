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
	public partial class UniversityDetailsPage : TabbedPage
    {
        private readonly UniversityDetailsViewModel _viewModel;

        public UniversityDetailsPage() 
        {
            InitializeComponent();
        }

        public UniversityDetailsPage (string universityName)
		{
			InitializeComponent ();
            BindingContext = _viewModel = new UniversityDetailsViewModel(universityName, Navigation);
        }
	}
}