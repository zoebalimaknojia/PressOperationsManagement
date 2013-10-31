using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogicLibrary
{
    public class Designer : Employee
    {
        public Designer()
        {
            this.orders = false;
            this.newOrder = false;
            this.existingOrders = false;
            this.payments = false;
            this.productsAndDesigns = true;
            this.addCategory = true;
            this.addProduct = true;
            this.addDesign = true;
            this.viewDesign = true;
            this.accountSettings = false;
            this.databaseOperations = false;
            this.reports = false;
            this.type = "designer";
        }
    }
}
