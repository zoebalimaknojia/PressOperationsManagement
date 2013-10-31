using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogicLibrary
{
    public class Design
    {
        private string designID;
        private string designFilePath;
        private byte[] designFile;
        private string designName;
        private string designSize;
        private string designPaperType;
        private string productName;

        public Design() { }

        public Design(string designID, string designFile, string designName, string designSize, string designPaperType, string productName)
        {
            this.designID = designID;
            this.designFilePath=designFile;
            this.designName=designName;
            this.designSize=designSize;
            this.designPaperType=designPaperType;
            this.productName = productName;
        }
        public Design(string designFilePath, string productName)
        {
            this.productName = productName;
            this.designFilePath = designFilePath;
        }
        public string DesignID { get { return designID; } set { this.designID = value; } }
        public string DesignFilePath { get { return designFilePath; } set { this.designFilePath = value; } }
        public byte[] DesignFile { get { return designFile; } set { this.designFile = value; } }
        public string DesignName { get { return designName; } set { this.designName = value; } }
        public string DesignSize { get { return designSize; } set { this.designSize = value; } }
        public string DesignPaperType { get { return designPaperType; } set { this.designPaperType = value; } }
        public string ProductName { get { return productName; } set { this.productName = value; } }   

    }
}
