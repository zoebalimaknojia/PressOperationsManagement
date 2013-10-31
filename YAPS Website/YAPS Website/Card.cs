using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YAPS_Website
{
    public class Card
    {
        private string cardid;
        private string imagesource;
        private string imagecontent;

        public Card() { }

        public Card(string imagesource, string imagecontent)
        {
            this.imagecontent = imagecontent;
            this.imagesource = imagesource;
        }

        public Card(string cardid, string imagesource, string imagecontent)
        {
            this.cardid = cardid;
            this.imagesource = imagesource;
            this.imagecontent = imagecontent;
        }

        public string ImageSource { get { return imagesource; } set { this.imagesource = value; } }
        public string ImageName { get { return imagecontent; } set { this.imagecontent = value; } }
        public string CardID { get { return cardid; } set { this.cardid = value; } }

    }
}