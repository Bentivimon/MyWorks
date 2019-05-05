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
	public partial class AbiturientPage : ContentPage
    {
        private AbiturientViewModel _viewModel;
		public AbiturientPage (EntrantDto entrant)
		{
			InitializeComponent ();
            BindingContext = _viewModel = new AbiturientViewModel(entrant);
        }
	}
}