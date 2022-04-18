using System;
using System.Collections.Generic;
using System.Text;
//Ysel Rodriguez
//DEV2500
//TermC202204
//CE02:Passing Data and Multiple Forms

namespace Data_Collector
{
    public class Info
    {
        //declare class variables
        public String savedName;
        public String savedGender;
        public DateTime savedDate;
        public String savedMarried;
        public Info(){
        }
        public Info(String savedName, String savedGender, DateTime savedDate, String savedMarried) {
            //set instance variables in the constructor
            this.savedName = savedName;
            this.savedGender = savedGender;
            this.savedDate = savedDate;
            this.savedMarried = savedMarried;
        }
    }

}
