using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraduateWork.Client.Models.ResponseModels;
using GraduateWork.Client.Services;
using GraduateWork.Client.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GraduateWork.Client.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UniversityDetailsPage : TabbedPage
    {
        private readonly UniversityDetailsViewModel _viewModel;
        private readonly SpecialitiesHttpClient _httpClient;
        public UniversityDetailsPage() 
        {
            InitializeComponent();
        }

        public UniversityDetailsPage (UniversityDto university, string regionName)
		{
            _httpClient = new SpecialitiesHttpClient();
			InitializeComponent ();
		    var specialities =
		        _httpClient.GetSpecialitiesByUniversityIdAsync(Application.Current.Properties["token"].ToString(),
		            university.Id).GetAwaiter().GetResult();
            BindingContext = _viewModel = new UniversityDetailsViewModel(Navigation, university, specialities, regionName);
        }
	}
}