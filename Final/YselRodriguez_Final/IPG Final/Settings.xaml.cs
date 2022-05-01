using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
    public partial class Settings : ContentPage
    {
        //declare variables and initialize as necessary
        public ObservableCollection<Stock> FavoriteStockList = new ObservableCollection<Stock>();
        public Settings()
        {
            InitializeComponent();
            favoriteStocksListView.ItemsSource = FavoriteStockList;

            //load favorites from file
            LoadFavorites();
        }

        public async void Add_Favorite_Stock_Button_Clicked(object sender, EventArgs e)
        {
            String symbolValue = "";


            if (StockInput.Text.Length == 0)
            {
                await DisplayAlert("Validation Failed", "Type a stock symbol", "OK");
            }
            else
            {
                symbolValue = StockInput.Text.ToLower();

                if (FavoriteStockList.Where(stock => String.Equals(stock.Symbol, symbolValue, StringComparison.CurrentCultureIgnoreCase)).ToList().Count > 0)
                {
                    //stock already exists
                    await DisplayAlert("Duplicate!", "This stock is already a favorite", "OK");
                }
                else
                {

                    Stock stock = new Stock();
                    stock.Symbol = symbolValue;

                    //URL parameters
                    var parameters = $"?symbols={stock.Symbol}&access_key={Constants.API_KEY}&limit=1";

                    try
                    {
                        //instantiate api client
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri(Constants.API_URL);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        //make request asyncroniously
                        HttpResponseMessage response = await client.GetAsync(parameters).ConfigureAwait(false);

                        JObject json = JObject.Parse(response.Content.ReadAsStringAsync().Result);

                        //dynamic dynJson = JsonConvert.DeserializeObject(json["Data"]);

                        if (response.IsSuccessStatusCode)
                        {
                            //parse the data into a dictionary
                            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json.ToString());
                            Console.WriteLine(json.ToString());

                            if (dictionary.ContainsKey("error"))
                            {
                                //there was an API error
                                JObject error_object = JObject.Parse(dictionary["error"].ToString());
                                String error_value = error_object.GetValue("code").ToString();

                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    DisplayAlert("API Failed", error_value, "OK");
                                    DisplayStatus("error", "Failed to get stock data");
                                });
                            }
                            else
                            {
                                //successfully fetched API result for symbol
                                //parse the useful data into a json array
                                JArray jsonArray = JArray.Parse(dictionary["data"].ToString());

                                foreach (JObject element in jsonArray)
                                {
                                    //set the data values
                                    stock.Symbol = element.GetValue("symbol").ToString();
                                    
                                    stock.Open = element.GetValue("open").ToString();
                                    stock.Close = element.GetValue("close").ToString();
                                    stock.Close = element.GetValue("close").ToString();
                                    stock.High = element.GetValue("high").ToString();
                                    stock.Low = element.GetValue("low").ToString();
                                    stock.Volume = element.GetValue("volume").ToString();
                                    stock.Exchange = element.GetValue("exchange").ToString();
                                    stock.Image = Constants.EOD_API_URL + stock.Symbol.ToLower() + ".png" + "?api_token=" + Constants.EOD_API_KEY;
                                    stock.Date = DateTime.Parse(element.GetValue("date").ToString()).ToLongDateString();

                                    //add to data list
                                    FavoriteStockList.Add(stock);

                                    //save to file
                                    SaveStocks(FavoriteStockList);
                                }

                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    DisplayAlert("Success!", "The new stock has been added to your favorites", "OK");
                                    DisplayStatus("success", "successfully added stock to favorites");
                                });
                            }
                        }
                        else
                        {
                            //API error
                            //parse the data into a dictionary
                            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json.ToString());
                            Console.WriteLine(json.ToString());

                            if (dictionary.ContainsKey("error"))
                            {
                                //there was an API error
                                JObject error_object = JObject.Parse(dictionary["error"].ToString());
                                String error_value = error_object.GetValue("code").ToString();

                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    DisplayAlert("API Failed", error_value, "OK");
                                    DisplayStatus("error", "Failed to get stock data");
                                });
                            }
                        }
                    }

                    catch (WebException ex)
                    {
                        await DisplayAlert("API Failed", ex.Message, "OK");
                        DisplayStatus("error", "Failed to get stock data");
                    }


                }
            }
        }


        public void SaveStocks(ObservableCollection<Stock> stocks)
        {
            //save the data to a file
            StreamWriter writer = new StreamWriter(Constants.saveFile); 

            foreach (Stock stock in stocks)
            {
                try
                {
                    writer.WriteLine("Symbol=" + stock.Symbol);
                    writer.WriteLine("Image=" + stock.Image);
                    writer.WriteLine("Open=" + stock.Open);
                    writer.WriteLine("Close=" + stock.Close);
                    writer.WriteLine("High=" + stock.High);
                    writer.WriteLine("Low=" + stock.Low);
                    writer.WriteLine("Volume=" + stock.Volume);
                    writer.WriteLine("Exchange=" + stock.Exchange);
                    writer.WriteLine("Date=" + stock.Date);
                }
                catch (Exception e)
                {
                    //an error occured during saving, do nothing
                }
            }
            //close the file handler
            writer.Close();

            //notify other pages that data has changed
            MessagingCenter.Send<Settings>(this, "refresh");
        }



        public void DisplayStatus(String type, String message)
        {
            //this displays the status of the most recent operation conveniently
            if (type == "success")
            {
                status.TextColor = Color.Green;
                status.Text = message;
            }
            else if (type == "error")
            {
                status.TextColor = Color.Red;
                status.Text = message;
            }
        }

        public void Save(Stock stock)
        {
            try
            {
                //save the data to a file
                StreamWriter writer = new StreamWriter(Constants.saveFile);

                writer.WriteLine("Symbol=" + stock.Symbol);
                writer.WriteLine("Image=" + stock.Image);
                writer.Close();
            }
            catch (Exception e)
            {
                //an error occured during saving, do nothing
            }
        }

        public void LoadFavorites()
        {
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
            }
            catch (Exception e)
            {
                //an error occured during loading the file
            }

        }

        public async void imageList_ItemTapped(object sender, EventArgs e)
        {
            //user has tapped an item in the image list 
            Stock selectedStock = (Stock)favoriteStocksListView.SelectedItem;

            //confirm if user wants to delete stock
            bool decision = await DisplayAlert("Delete", "Are you sure you wish to delete this favorite?", "YES", "NO");

            if (decision == true)
            {
                //delete the stock
                FavoriteStockList.Remove(selectedStock);

                //save the list
                SaveStocks(FavoriteStockList);
            }
        }


    }
}
