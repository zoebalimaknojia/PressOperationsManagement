using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BusinessLogicLibrary
{
    public class PaymentOperations
    {
        public bool applyPayment(int orderID, int paymentAmount)
        {
            DatabaseOperations operations = new DatabaseOperations();
            if (operations.executeInsUpdDelQuery("UPDATE OrderDetails SET AdvancePayment=AdvancePayment+" + paymentAmount + "WHERE OrderID=" + orderID) == 1)
                return true;
            else
                return false;
        }
        public DataTable remainingPayments()
        {
            DatabaseOperations operations = new DatabaseOperations();
            return operations.executeSelectQuery("Select OrderDetails.OrderID, CustomerDetails.CustomerName, CustomerDetails.CustomerContactNumber, OrderDetails.DeliveryDate, OrderDetails.AdvancePayment, ((OrderDetails.Quantity * OrderDetails.PerProductCost)- OrderDetails.AdvancePayment) AS RemainingPayment, (OrderDetails.Quantity*OrderDetails.PerProductCost) AS TotalCost FROM OrderDetails, CustomerDetails WHERE OrderDetails.CustomerID=CustomerDetails.CustomerID AND ((OrderDetails.Quantity * OrderDetails.PerProductCost)- OrderDetails.AdvancePayment)>0");
        }
        public DataTable remainingPayments(int orderID)
        
        {
            DatabaseOperations operations = new DatabaseOperations();
            return operations.executeSelectQuery("Select OrderDetails.OrderID, CustomerDetails.CustomerName, CustomerDetails.CustomerContactNumber, OrderDetails.DeliveryDate, OrderDetails.AdvancePayment, ((OrderDetails.Quantity * OrderDetails.PerProductCost)- OrderDetails.AdvancePayment) AS RemainingPayment, (OrderDetails.Quantity*OrderDetails.PerProductCost) AS TotalCost, OrderStatus.OrderStatusName FROM OrderDetails, CustomerDetails, OrderStatus WHERE OrderDetails.CustomerID=CustomerDetails.CustomerID AND OrderStatus.OrderStatusID=OrderDetails.OrderStatusID AND OrderDetails.OrderID=" + orderID);
        }
    }
}
