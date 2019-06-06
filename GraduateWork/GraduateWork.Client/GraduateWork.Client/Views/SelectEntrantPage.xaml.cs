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
	public partial class SelectEntrantPage : ContentPage
	{
	    private SelectEntrantPageViewModel _viewModel;

		public SelectEntrantPage ()
		{
			InitializeComponent ();
		    BindingContext = _viewModel = new SelectEntrantPageViewModel(Navigation);
		}
	}
}