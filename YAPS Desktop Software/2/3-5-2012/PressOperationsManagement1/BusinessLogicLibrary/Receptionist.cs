using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogicLibrary
{
    public class Receptionist : Employee
    {
        public Receptionist()
        {
            this.orders = true;
            this.newOrder = true;
            this.existingOrders = true;
            this.payments = true;
            this.productsAndDesigns = true;
            this.addCategory = false;
            this.addProduct = false;
            this.addDesign = false;
            this.viewDesign = true;
            this.accountSettings = false;
            this.databaseOperations = true;
            this.reports = true;
            this.type = "receptionist";
        }
    }
}
