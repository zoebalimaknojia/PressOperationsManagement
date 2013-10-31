using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BusinessLogicLibrary
{
    public class Order
    {
        private int orderID;
        private String designID;
        private Customer customer;
        private String paperType;
        private String color;
        private String orderStatus;
        private int quantity;
        private String size;
        private String finalizedDesign;
        private byte[] designFile;
        private DateTime deliveryDate;
        private int unitPrice;
        private int advancePayment;
        private String designName;

        public int OrderID
        {
            get
            {
                return orderID;
            }
            set
            {
                orderID = value;
            }
        }

        public byte[] DesignFile
        {
            get
            {
                return designFile;
            }
            set
            {
                designFile = value;
            }
        }

        public String DesigName
        {
            get
            {
                return designName;
            }
            set
            {
                designName = value;
            }
        }


        public String DesignID
        {
            get
            {
                return designID;
            }
            set
            {
                designID = value;
            }
        }
        public Customer Customer
        {
            get
            {
                return customer;
            }
            set
            {
                customer = value;
            }
        }
        public String PaperType
        {
            get
            {
                return paperType;
            }
            set
            {
                paperType = value;
            }
        }
        public String Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }
        public string OrderStatus
        {
            get
            {
                return orderStatus;
            }
            set
            {
                orderStatus = value;
            }
        }
        public int Quantity
        {
            get
            {
                return quantity;
            }
            set
            {
                quantity = value;
            }
        }
        public String Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
            }
        }
        public string FinalizedDesign
        {
            get
            {
                return finalizedDesign;
            }
            set
            {
                finalizedDesign = value;
            }
        }
        public DateTime DeliveryDate
        {
            get
            {
                return deliveryDate;
            }
            set
            {
                deliveryDate = value;
            }
        }
        public int UnitPrice
        {
            get
            {
                return unitPrice;
            }
            set
            {
                unitPrice = value;
            }
        }
        public int AdvancePayment
        {
            get
            {
                return advancePayment;
            }
            set
            {
                advancePayment = value;
            }
        }
    }
}
