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
	public partial class SpecialityInfoPage : ContentPage
    {
        private SpecialityInfoViewModel _viewModel;
        public SpecialityInfoPage (string universityName, SpecialityDto speciality, List<EntrantDto> entrants)
		{
			InitializeComponent ();
            BindingContext = _viewModel = new SpecialityInfoViewModel(Navigation, universityName, speciality, entrants);
        }
	}
}