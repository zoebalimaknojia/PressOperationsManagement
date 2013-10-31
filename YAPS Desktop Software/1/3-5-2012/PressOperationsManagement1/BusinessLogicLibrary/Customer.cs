using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogicLibrary
{
    public class Customer
    {
        private int id;
        private String name;
        private String address;
        private long contactNumber;
        public Customer()
        {
            
        }
        public Customer(int id, String name, String address, long contactNumber)
        {
            this.id = id;
            this.name = name;
            this.address = address;
            this.contactNumber = contactNumber;
        }
        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        public String Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public String Address
        {
            get
            {
                return address;
            }
            set
            {
                address = value;
            }
        }
        public long ContactNumber
        {
            get
            {
                return contactNumber;
            }
            set
            {
                contactNumber = value;
            }
        }
    }
}
