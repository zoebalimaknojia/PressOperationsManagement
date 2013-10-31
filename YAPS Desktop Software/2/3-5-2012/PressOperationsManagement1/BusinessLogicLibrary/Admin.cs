using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogicLibrary
{
    public class Admin : Employee
    {
        public Admin()
        {
            this.orders = true;
            this.newOrder = true;
            this.existingOrders = true;
            this.payments = true;
            this.productsAndDesigns = true;
            this.addCategory = true;
            this.addProduct = true;
            this.addDesign = true;
            this.viewDesign = true;
            this.accountSettings = true;
            this.databaseOperations = true;
            this.reports = true;
            this.type = "admin";
        }
    }
}
