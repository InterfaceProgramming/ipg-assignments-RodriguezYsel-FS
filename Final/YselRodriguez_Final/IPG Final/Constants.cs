using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
//Ysel Rodriguez
//DEV2500
//TermC202204
//4.2 Final Project: Stock App Project
namespace IPG_Final
{
    static public class Constants
    {
        //declare static variables to be reused in other classes
        public static String saveFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "info.txt");
        public static String API_KEY = "4d1fda74527f1ae3eae4069100dc058d";
        public static String API_URL = "http://api.marketstack.com/v1/eod";
        public static String EOD_API_URL = "https://eodhistoricaldata.com/img/logos/US/";
        public static String EOD_API_KEY = "626944d18eacf5.64670059";
    }
}
