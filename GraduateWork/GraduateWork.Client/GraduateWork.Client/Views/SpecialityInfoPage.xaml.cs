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
	public partial class SpecialityInfoPage : ContentPage
    {
        private SpecialityInfoViewModel _viewModel;
        public SpecialityInfoPage ()
		{
			InitializeComponent ();
            BindingContext = _viewModel = new SpecialityInfoViewModel();
        }
	}
}