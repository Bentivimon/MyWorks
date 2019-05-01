using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using GraduateWork.Client.Models;
using GraduateWork.Client.Views;
using Xamarin.Forms;

namespace GraduateWork.Client.ViewModels
{
    public class UniversityViewModel : BaseViewModel
    {
        public ObservableCollection<UniversityListModel> Universities { get; set; }

        public INavigation Navigation { get; set; }

        private UniversityListModel _universitiesSelectedItem;

        public UniversityListModel UniversitiesSelectedItem
        {
            get
            {
                return _universitiesSelectedItem;
            }

            set
            {
                _universitiesSelectedItem = value;
                OnPropertyChanged();
                ShowUniversityDetailPage();
            }
        }

        private void ShowUniversityDetailPage()
        {
            Navigation.PushAsync(new UniversityDetailsPage(_universitiesSelectedItem.FullName), true);
        }


        public UniversityViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Universities = new ObservableCollection<UniversityListModel>()
            {
                new UniversityListModel(){FullName = "Тернопільський національний педагогічний університет імені Володимира Гнатюка"},
                new UniversityListModel(){FullName = "ТЕРНОПІЛЬСЬКИЙ НАЦІОНАЛЬНИЙ ЕКОНОМІЧНИЙ УНІВЕРСИТЕТ"},
                new UniversityListModel(){FullName = "ТЕРНОПІЛЬСЬКИЙ НАЦІОНАЛЬНИЙ МЕДИЧНИЙ УНІВЕРСИТЕТ  імені І.Я.Горбачевського"},
                new UniversityListModel(){FullName = "Національний університет «Львівська політехніка»"},
                new UniversityListModel(){FullName = "Національний технічний університет україни «Київський політехнічний штститут імені Ігоря Сікорського»"},
                new UniversityListModel(){FullName = "Тернопільський національний педагогічний університет імені Володимира Гнатюка"},
                new UniversityListModel(){FullName = "ТЕРНОПІЛЬСЬКИЙ НАЦІОНАЛЬНИЙ ЕКОНОМІЧНИЙ УНІВЕРСИТЕТ"},
                new UniversityListModel(){FullName = "ТЕРНОПІЛЬСЬКИЙ НАЦІОНАЛЬНИЙ МЕДИЧНИЙ УНІВЕРСИТЕТ  імені І.Я.Горбачевського"},
                new UniversityListModel(){FullName = "Національний університет «Львівська політехніка»"},
                new UniversityListModel(){FullName = "Національний технічний університет україни «Київський політехнічний штститут імені Ігоря Сікорського»"},
                new UniversityListModel(){FullName = "Тернопільський національний педагогічний університет імені Володимира Гнатюка"},
                new UniversityListModel(){FullName = "ТЕРНОПІЛЬСЬКИЙ НАЦІОНАЛЬНИЙ ЕКОНОМІЧНИЙ УНІВЕРСИТЕТ"},
                new UniversityListModel(){FullName = "ТЕРНОПІЛЬСЬКИЙ НАЦІОНАЛЬНИЙ МЕДИЧНИЙ УНІВЕРСИТЕТ  імені І.Я.Горбачевського"},
                new UniversityListModel(){FullName = "Національний університет «Львівська політехніка»"},
                new UniversityListModel(){FullName = "Національний технічний університет україни «Київський політехнічний штститут імені Ігоря Сікорського»"},
                new UniversityListModel(){FullName = "Тернопільський національний педагогічний університет імені Володимира Гнатюка"},
                new UniversityListModel(){FullName = "ТЕРНОПІЛЬСЬКИЙ НАЦІОНАЛЬНИЙ ЕКОНОМІЧНИЙ УНІВЕРСИТЕТ"},
                new UniversityListModel(){FullName = "ТЕРНОПІЛЬСЬКИЙ НАЦІОНАЛЬНИЙ МЕДИЧНИЙ УНІВЕРСИТЕТ  імені І.Я.Горбачевського"},
                new UniversityListModel(){FullName = "Національний університет «Львівська політехніка»"},
                new UniversityListModel(){FullName = "Національний технічний університет україни «Київський політехнічний штститут імені Ігоря Сікорського»"},
                new UniversityListModel(){FullName = "Тернопільський національний педагогічний університет імені Володимира Гнатюка"},
                new UniversityListModel(){FullName = "ТЕРНОПІЛЬСЬКИЙ НАЦІОНАЛЬНИЙ ЕКОНОМІЧНИЙ УНІВЕРСИТЕТ"},
                new UniversityListModel(){FullName = "ТЕРНОПІЛЬСЬКИЙ НАЦІОНАЛЬНИЙ МЕДИЧНИЙ УНІВЕРСИТЕТ  імені І.Я.Горбачевського"},
                new UniversityListModel(){FullName = "Національний університет «Львівська політехніка»"},
                new UniversityListModel(){FullName = "Національний технічний університет україни «Київський політехнічний штститут імені Ігоря Сікорського»"},
                new UniversityListModel(){FullName = "Тернопільський національний педагогічний університет імені Володимира Гнатюка"},
                new UniversityListModel(){FullName = "ТЕРНОПІЛЬСЬКИЙ НАЦІОНАЛЬНИЙ ЕКОНОМІЧНИЙ УНІВЕРСИТЕТ"},
                new UniversityListModel(){FullName = "ТЕРНОПІЛЬСЬКИЙ НАЦІОНАЛЬНИЙ МЕДИЧНИЙ УНІВЕРСИТЕТ  імені І.Я.Горбачевського"},
                new UniversityListModel(){FullName = "Національний університет «Львівська політехніка»"},
                new UniversityListModel(){FullName = "Національний технічний університет україни «Київський політехнічний штститут імені Ігоря Сікорського»"},
                new UniversityListModel(){FullName = "Тернопільський національний педагогічний університет імені Володимира Гнатюка"},
                new UniversityListModel(){FullName = "ТЕРНОПІЛЬСЬКИЙ НАЦІОНАЛЬНИЙ ЕКОНОМІЧНИЙ УНІВЕРСИТЕТ"},
                new UniversityListModel(){FullName = "ТЕРНОПІЛЬСЬКИЙ НАЦІОНАЛЬНИЙ МЕДИЧНИЙ УНІВЕРСИТЕТ  імені І.Я.Горбачевського"},
                new UniversityListModel(){FullName = "Національний університет «Львівська політехніка»"},
                new UniversityListModel(){FullName = "Національний технічний університет україни «Київський політехнічний штститут імені Ігоря Сікорського»"},
                new UniversityListModel(){FullName = "Тернопільський національний педагогічний університет імені Володимира Гнатюка"},
                new UniversityListModel(){FullName = "ТЕРНОПІЛЬСЬКИЙ НАЦІОНАЛЬНИЙ ЕКОНОМІЧНИЙ УНІВЕРСИТЕТ"},
                new UniversityListModel(){FullName = "ТЕРНОПІЛЬСЬКИЙ НАЦІОНАЛЬНИЙ МЕДИЧНИЙ УНІВЕРСИТЕТ  імені І.Я.Горбачевського"},
                new UniversityListModel(){FullName = "Національний університет «Львівська політехніка»"},
                new UniversityListModel(){FullName = "Національний технічний університет україни «Київський політехнічний штститут імені Ігоря Сікорського»"},
            };
        }

    }
}
