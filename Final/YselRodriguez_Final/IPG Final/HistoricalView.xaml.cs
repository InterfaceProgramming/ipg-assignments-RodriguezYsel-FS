using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//Ysel Rodriguez
//DEV2500
//TermC202204
//4.2 Final Project: Stock App Project
namespace IPG_Final
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoricalView : ContentPage
    {
        public HistoricalView()
        {
            InitializeComponent();
        }

        public HistoricalView(ObservableCollection<Stock> HistoricalData)
        {
            InitializeComponent();

            Device.BeginInvokeOnMainThread(() =>
            {
                carouselView.ItemsSource = HistoricalData;
            });
        }
    }
}