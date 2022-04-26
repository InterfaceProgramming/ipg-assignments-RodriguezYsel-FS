using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CE08
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class History : ContentPage
    {
        //this will hold history
        public ObservableCollection<HistoryData> EntireHistory = new ObservableCollection<HistoryData>();
        public History()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<Settings, HistoryData>(this, "history", (sender, data) =>
            {
                EntireHistory.Add(data);
            });

            //set the imagelistview to show history
            imageListView.ItemsSource = EntireHistory;
        }

        public void Clear_Button_Clicked(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                //clear history data
                EntireHistory.Clear();

            });
        }
    }
}