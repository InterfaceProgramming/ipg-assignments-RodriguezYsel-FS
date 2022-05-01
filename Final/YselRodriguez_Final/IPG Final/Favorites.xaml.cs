using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
    public partial class Favorites : ContentPage
    {
        //declare variables and initialize as necessary
        public ObservableCollection<Stock> FavoriteStockList = new ObservableCollection<Stock>();
        public Favorites()
        {
            InitializeComponent();

            //set the imagelist to use the favorite stocks list data
            imageListView.ItemsSource = FavoriteStockList;

            //load favorites
            LoadFavorites();

            //listen for messages
            MessagingCenter.Subscribe<Settings>(this, "refresh", sender =>
            {
                // received message that data has changed
                //reload data
                LoadFavorites();

            });
        }

        public async void imageList_ItemTapped(object sender, EventArgs e)
        {
            //user has tapped an item in the image list 
            Stock selectedStock = (Stock)imageListView.SelectedItem;

            await Navigation.PushModalAsync(new Details(selectedStock), true);
        }

        public void LoadFavorites() {
            FavoriteStockList.Clear();

            //load values from the previously saved file
            try
            {
                StreamReader reader = new StreamReader(Constants.saveFile);

                String line;

                //parse the saved data line by line and update GUI
                //also update that data variable that holds the information
                Stock stock = new Stock(); 
                
                while ((line = reader.ReadLine()) != null)
                {
                    
                    if (line.StartsWith("Symbol="))
                    {
                        stock = new Stock(); 
                        String savedSymbol = line.Split('=')[1].Trim();
                        stock.Symbol = savedSymbol;
                    }
                    else if (line.StartsWith("Image="))
                    {
                        String savedImage = line.Split('=')[1].Trim();
                        stock.Image = savedImage;
                    }
                    else if (line.StartsWith("Open="))
                    {
                        String savedOpen = line.Split('=')[1].Trim();
                        stock.Open = savedOpen;
                    }
                    else if (line.StartsWith("Close="))
                    {
                        String savedClose = line.Split('=')[1].Trim();
                        stock.Close = savedClose;       
                    }
                    else if (line.StartsWith("High="))
                    {
                        String savedHigh = line.Split('=')[1].Trim();
                        stock.High = savedHigh;
                    }
                    else if (line.StartsWith("Low="))
                    {
                        String savedLow = line.Split('=')[1].Trim();
                        stock.Low = savedLow;
                    }
                    else if (line.StartsWith("Volume="))
                    {
                        String savedVolume = line.Split('=')[1].Trim();
                        stock.Volume = savedVolume;
                    }
                    else if (line.StartsWith("Exchange="))
                    {
                        String savedExchange = line.Split('=')[1].Trim();
                        stock.Exchange = savedExchange;
                    }
                    else if (line.StartsWith("Date="))
                    {
                        String savedDate = line.Split('=')[1].Trim();
                        stock.Date = savedDate;

                        //add the stock to favorites list
                        FavoriteStockList.Add(stock);
                    }
                    
                }

                //close the file
                reader.Close();

                if (FavoriteStockList.Count > 0)
                {
                    //hide the there are no favorites saved text
                    noFavorites.IsVisible = false;
                }
            }
            catch (Exception e)
            {
                //an error occured during loading the file
            }

        }
    }
}