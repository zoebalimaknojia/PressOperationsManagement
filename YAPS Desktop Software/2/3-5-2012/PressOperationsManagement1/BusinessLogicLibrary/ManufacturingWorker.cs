using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogicLibrary
{
    public class ManufacturingWorker : Employee
    {
        public ManufacturingWorker()
        {
            this.orders = true;
            this.newOrder = false;
            this.existingOrders = true;
            this.payments = false;
            this.productsAndDesigns = false;
            this.addCategory = false;
            this.addProduct = false;
            this.addDesign = false;
            this.viewDesign = false;
            this.accountSettings = false;
            this.databaseOperations = false;
            this.reports = false;
            this.type = "manufacturingworker";
        }
    }
}
