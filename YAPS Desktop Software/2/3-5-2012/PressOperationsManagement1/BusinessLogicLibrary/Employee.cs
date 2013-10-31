using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogicLibrary
{
    public class Employee
    {
        protected bool orders;
        protected bool newOrder;
        protected bool existingOrders;
        protected bool payments;
        protected bool productsAndDesigns;
        protected bool accountSettings;
        protected bool databaseOperations;
        protected bool addDesign;
        protected bool addProduct;
        protected bool addCategory;
        protected bool viewDesign;
        protected bool reports;
        protected String type;
        protected String employeeName;

        public bool Orders
        {
            get
            {
                return orders;
            }
            set
            {
                orders = value;
            }
        }
        public bool NewOrder
        {
            get
            {
                return newOrder;
            }
            set
            {
                newOrder = value;
            }
        }
        public bool ExistingOrders
        {
            get
            {
                return existingOrders;
            }
            set
            {
                existingOrders = value;
            }
        }
        public bool Payments
        {
            get
            {
                return payments;
            }
            set
            {
                payments = value;
            }
        }
        public bool ProductsAndDesigns
        {
            get
            {
                return productsAndDesigns;
            }
            set
            {
                productsAndDesigns = value;
            }
        }
        public bool AccountSettings
        {
            get
            {
                return accountSettings;
            }
            set
            {
                accountSettings = value;
            }
        }
        public bool DatabaseOperations
        {
            get
            {
                return databaseOperations;
            }
            set
            {
                databaseOperations = value;
            }
        }

        public bool Reports
        {
            get
            {
                return reports;
            }
            set
            {
                reports = value;
            }
        }

        public bool AddCategory
        {
            get
            {
                return addCategory;
            }
            set
            {
                addCategory = value;
            }
        }

        public bool AddDesign
        {
            get
            {
                return addDesign;
            }
            set
            {
                addDesign = value;
            }
        }

        public bool AddProduct
        {
            get
            {
                return addProduct;
            }
            set
            {
                addProduct = value;
            }
        }

        public bool ViewDesign
        {
            get
            {
                return viewDesign;
            }
            set
            {
                viewDesign = value;
            }
        }

        public String Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }
        public String EmployeeName
        {
            get
            {
                return employeeName;
            }
            set
            {
                employeeName = value;
            }
        }
    }
}
