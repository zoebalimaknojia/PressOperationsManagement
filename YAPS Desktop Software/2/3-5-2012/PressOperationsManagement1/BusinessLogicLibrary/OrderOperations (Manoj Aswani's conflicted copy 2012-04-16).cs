using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BusinessLogicLibrary
{
    public class OrderOperations
    {
        public int returnNextOrderID()
        {
            DatabaseOperations operations = new DatabaseOperations();
            DataTable dt = operations.getTableData("OrderDetails","OrderID");
            if (dt.Rows.Count == 0)
            {
                return 10001;
            }
            else
            {
                return ((int)dt.Rows[dt.Rows.Count - 1][0] + 1);
            }
        }
        public bool addNewOrder()
        {
            //DatabaseOperations operations = new DatabaseOperations();
            //DataTable dt = operations.getTableData("OrderDetails","OrderID");
            //if (dt.Rows.Count == 0)
            //{

            //}
            //else
            //{
            //    new
            //}
            return true;
        }

        public DataTable searchOrder(int orderID)
        {
            DatabaseOperations operations = new DatabaseOperations();
            DataTable orderDetails = operations.executeSelectQuery("SELECT * FROM OrderDetails WHERE OrderID="+orderID);
            if (orderDetails.Rows.Count > 0)
                return orderDetails;
            else
                return null;
        }

        public DataTable searchOrder(String customerName)
        {
            DatabaseOperations operations = new DatabaseOperations();
            DataTable customerDetails = operations.executeSelectQuery("SELECT CustomerID FROM CustomerDetails WHERE CustomerName=" + customerName);
            if (customerDetails.Rows.Count > 0)
            {
                DataTable orderDetails = operations.executeSelectQuery("SELECT * FROM OrderDetails WHERE CustomerID=" + customerDetails.Rows[0][0]);
                if (orderDetails.Rows.Count > 0)
                    return orderDetails;
                else
                    return null;
            }
            return null;
        }

        public DataTable searchOrder(DateTime deliveryDate)
        {
            DatabaseOperations operations = new DatabaseOperations();
            DataTable orderDetails = operations.executeSelectQuery("SELECT * FROM OrderDetails WHERE DeliverDate='" + deliveryDate + "'");
            if (orderDetails.Rows.Count > 0)
                return orderDetails;
            else
                return null;
        }

        public DataSet getAllOrders()
        {
            DatabaseOperations operations = new DatabaseOperations();
            DataTable orderDetails = operations.executeSelectQuery("SELECT OrderDetails.CustomerID,OrderDetails.OrderID,OrderDetails.DeliveryDate,OrderDetails.Quantity,OrderStatus.OrderStatusName,CustomerDetails.CustomerName FROM OrderStatus,OrderDetails,CustomerDetails WHERE OrderStatus.OrderStatusID=OrderDetails.OrderStatusID and CustomerDetails.CustomerID=OrderDetails.CustomerID").Copy();
            orderDetails.TableName = "order";
            DataTable status = operations.executeSelectQuery("Select OrderStatusName from OrderStatus").Copy();
            status.TableName = "status";
            DataSet ds = new DataSet();
            ds.Tables.Add(orderDetails);
            ds.Tables.Add(status);
            //CustomerDetails.CustomerName FROM OrderDetails,CustomerDetails where CustomerDetails.CustomerID=OrderDetails.CustomerID'");
            return ds;
        }
        public bool editOrder(int orderID)
        {
            return true;
        }
    }
}
