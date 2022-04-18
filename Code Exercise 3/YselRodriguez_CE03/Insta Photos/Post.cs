using System;
using System.Collections.Generic;
using System.Text;
//Ysel Rodriguez
//DEV2500
//TermC202204
//CE03: ListView


namespace Insta_Photos
{
    //this class will be used to hold the main info
    public class Post
    {
        //constructor
        public Post()
        {

        }
        

        //define getters and setters
        
        public string photo
        {
            get;
            set;
        }
        public string title
        {
            get;
            set;
        }
        public string comments
        {
            get;
            set;
        }
    }
}
