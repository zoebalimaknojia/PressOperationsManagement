using System;
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
        public bool addNewOrder(Order newOrder)
        {
            DatabaseOperations operations = new DatabaseOperations();
            try
            {
                int paperTypeID = int.Parse(operations.executeSelectQuery("SELECT PaperTypeID FROM PaperType WHERE PaperTypeName='" + newOrder.PaperType + "'").Rows[0][0].ToString());
                int statusTypeID = int.Parse(operations.executeSelectQuery("SELECT OrderStatusID FROM OrderStatus WHERE OrderStatusName='" + newOrder.OrderStatus + "'").Rows[0][0].ToString());
                DataTable customerDetails = operations.executeSelectQuery("SELECT * FROM CustomerDetails WHERE CustomerName='" + newOrder.Customer.Name + "' AND CustomerContactNumber=" + newOrder.Customer.ContactNumber);
                if (customerDetails.Rows.Count == 0)
                {
                    DataTable allCustomerDetails = operations.executeSelectQuery("SELECT * FROM CustomerDetails");
                    if (allCustomerDetails.Rows.Count == 0)
                    {
                        operations.executeInsUpdDelQuery("INSERT INTO CustomerDetails (CustomerID, CustomerName, CustomerAddress, CustomerContactNumber) VALUES (" + 1 + ",'" + newOrder.Customer.Name + "','" + newOrder.Customer.Address + "," + newOrder.Customer.ContactNumber + ")");
                        newOrder.Customer.ID = 1;
                    }
                    else
                    {
                        int lastCustomerID = (int)allCustomerDetails.Rows[allCustomerDetails.Rows.Count - 1][0];
                        operations.executeInsUpdDelQuery("INSERT INTO CustomerDetails (CustomerID, CustomerName, CustomerAddress, CustomerContactNumber) VALUES (" + (lastCustomerID + 1) + ",'" + newOrder.Customer.Name + "','" + newOrder.Customer.Address + "'," + newOrder.Customer.ContactNumber + ")");
                        newOrder.Customer.ID = lastCustomerID + 1;
                    }
                    int i = operations.executeInsUpdDelQuery("INSERT INTO OrderDetails (OrderID, CustomerID, DesignID, PaperTypeID, OrderStatusID, Quantity, Size, DeliveryDate, PerProductCost, AdvancePayment, OrderDelivered, Color) VALUES (" + newOrder.OrderID + "," + newOrder.Customer.ID + ",'" + newOrder.DesignID + "'," + paperTypeID + "," + statusTypeID + "," + newOrder.Quantity + ",'" + newOrder.Size + "','" + newOrder.DeliveryDate + "'," + newOrder.UnitPrice + "," + newOrder.AdvancePayment + ",0,'" + newOrder.Color + "')");
                    int j = operations.executeUpdImageQuery("OrderDetails", "FinalDesignToBePrinted", newOrder.FinalizedDesign, "OrderID", newOrder.OrderID.ToString());
                    if (i == 1 && j == 1)
                        return true;
                    else
                        return false;
                }
                else
                {
                    newOrder.Customer.ID = int.Parse(customerDetails.Rows[0][0].ToString());
                    int i = operations.executeInsUpdDelQuery("INSERT INTO OrderDetails (OrderID, CustomerID, DesignID, PaperTypeID, OrderStatusID, Quantity, Size, DeliveryDate, PerProductCost, AdvancePayment, OrderDelivered, Color) VALUES (" + newOrder.OrderID + "," + newOrder.Customer.ID + ",'" + newOrder.DesignID + "'," + paperTypeID + "," + statusTypeID + "," + newOrder.Quantity + ",'" + newOrder.Size + "','" + newOrder.DeliveryDate + "'," + newOrder.UnitPrice + "," + newOrder.AdvancePayment + ",0,'"+ newOrder.Color + "')");
                    int j = operations.executeUpdImageQuery("OrderDetails", "FinalDesignToBePrinted", newOrder.FinalizedDesign, "OrderID", newOrder.OrderID.ToString());
                    if (i == 1 && j == 1)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public DataTable searchOrder(int orderID)
        {
            DatabaseOperations operations = new DatabaseOperations();
            DataTable orderDetails = operations.executeSelectQuery("SELECT OrderDetails.OrderID, CustomerDetails.CustomerName, CustomerDetails.CustomerContactNumber, OrderDetails.DeliveryDate, OrderDetails.Quantity, Category.CategoryName, OrderStatus.OrderStatusName FROM OrderStatus,CustomerDetails,OrderDetails, Category, Product, Design WHERE OrderStatus.OrderStatusID=OrderDetails.OrderStatusID and OrderDetails.CustomerID=CustomerDetails.CustomerID AND Category.CategoryID = Product.CategoryID AND Product.ProductID = Design.ProductID AND Design.DesignID = OrderDetails.DesignID AND OrderDetails.OrderID=" + orderID);
            if (orderDetails.Rows.Count > 0)
                return orderDetails;
            else
                return null;
        }

        public DataTable searchOrder(String customerName)
        {
            DatabaseOperations operations = new DatabaseOperations();
            DataTable customerDetails = operations.executeSelectQuery("SELECT CustomerID FROM CustomerDetails WHERE CustomerName LIKE '" + customerName + "%'");
            if (customerDetails.Rows.Count > 0)
            {
                DataTable orderDetails = operations.executeSelectQuery("SELECT OrderDetails.OrderID, CustomerDetails.CustomerName, CustomerDetails.CustomerContactNumber, OrderDetails.DeliveryDate, OrderDetails.Quantity, Category.CategoryName, OrderStatus.OrderStatusName FROM OrderStatus,CustomerDetails,OrderDetails, Category, Product, Design WHERE OrderStatus.OrderStatusID=OrderDetails.OrderStatusID and OrderDetails.CustomerID=CustomerDetails.CustomerID AND Category.CategoryID = Product.CategoryID AND Product.ProductID = Design.ProductID AND Design.DesignID = OrderDetails.DesignID AND CustomerDetails.CustomerName LIKE '" + customerName + "%'");
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
            DataTable orderDetails = operations.executeSelectQuery("SELECT OrderDetails.OrderID, CustomerDetails.CustomerName, CustomerDetails.CustomerContactNumber, OrderDetails.DeliveryDate, OrderDetails.Quantity, Category.CategoryName, OrderStatus.OrderStatusName FROM OrderStatus,CustomerDetails,OrderDetails, Category, Product, Design WHERE OrderStatus.OrderStatusID=OrderDetails.OrderStatusID and OrderDetails.CustomerID=CustomerDetails.CustomerID AND Category.CategoryID = Product.CategoryID AND Product.ProductID = Design.ProductID AND Design.DesignID = OrderDetails.DesignID AND DeliveryDate='" + deliveryDate.Date + "'");
            if (orderDetails.Rows.Count > 0)
                return orderDetails;
            else
                return null;
        }

        public DataTable getAllOrders()
        {
            DatabaseOperations operations = new DatabaseOperations();
            DataTable orderDetails = operations.executeSelectQuery("SELECT OrderDetails.OrderID, CustomerDetails.CustomerName, CustomerDetails.CustomerContactNumber, OrderDetails.DeliveryDate,OrderDetails.Quantity, Category.CategoryName, OrderStatus.OrderStatusName FROM OrderStatus,CustomerDetails,OrderDetails, Category, Product, Design WHERE OrderStatus.OrderStatusID=OrderDetails.OrderStatusID and OrderDetails.CustomerID=CustomerDetails.CustomerID AND OrderStatus.OrderStatusName!='Delivered' AND Category.CategoryID = Product.CategoryID AND Product.ProductID = Design.ProductID AND Design.DesignID = OrderDetails.DesignID");
            return orderDetails;
        }

        public bool isOrderDelivered(int orderID)
        {
            DatabaseOperations operations = new DatabaseOperations();
            int del = int.Parse(operations.executeSelectQuery("SELECT OrderStatusID FROM OrderStatus WHERE OrderStatusName='Delivered'").Rows[0][0].ToString());
            if (int.Parse(operations.executeSelectQuery("SELECT OrderStatusID FROM OrderDetails WHERE OrderID="+orderID).Rows[0][0].ToString()) == del)
                return true;
            else
                return false;
        }

        public bool setOrderStatus(int orderID, string status)
        {
            DatabaseOperations operations = new DatabaseOperations();
            try
            {
                int statusID = int.Parse(operations.executeSelectQuery("SELECT OrderStatusID FROM OrderStatus WHERE OrderStatusName='" + status + "'").Rows[0][0].ToString());
                if (operations.executeInsUpdDelQuery("UPDATE OrderDetails SET OrderStatusID=" + statusID + " WHERE OrderID=" + orderID) == 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool orderExists(int orderID)
        {
            DatabaseOperations operations = new DatabaseOperations();
            if(operations.executeSelectQuery("SELECT * FROM OrderDetails WHERE OrderID="+orderID).Rows.Count ==1)
                return true;
            else
                return false;
        }

        public Order getOrderByID(int orderID)
        {
            DatabaseOperations operations = new DatabaseOperations();
            DataTable orderDetails = operations.executeSelectQuery("SELECT * FROM OrderDetails WHERE OrderID="+orderID);
            Order order = new Order();
            order.OrderID = int.Parse(orderDetails.Rows[0][0].ToString());
            order.DesignID = orderDetails.Rows[0][2].ToString();
            order.Quantity = int.Parse(orderDetails.Rows[0][3].ToString());
            order.Size = orderDetails.Rows[0][6].ToString();
            order.DesignFile = (byte[])orderDetails.Rows[0][7];
            order.DeliveryDate = ((DateTime) orderDetails.Rows[0][8]);
            order.UnitPrice = int.Parse(orderDetails.Rows[0][9].ToString());
            order.AdvancePayment = int.Parse(orderDetails.Rows[0][10].ToString());
            DataTable customerDetails = operations.executeSelectQuery("SELECT * FROM CustomerDetails WHERE CustomerID=" + orderDetails.Rows[0][1].ToString());
            order.Customer = new Customer(int.Parse(customerDetails.Rows[0][0].ToString()), customerDetails.Rows[0][1].ToString(), customerDetails.Rows[0][2].ToString(), long.Parse(customerDetails.Rows[0][3].ToString()));
            order.DesigName = operations.executeSelectQuery("Select DesignName from Design WHERE DesignID='" + order.DesignID + "'").Rows[0][0].ToString();
            order.PaperType = operations.executeSelectQuery("Select PaperTypeName from PaperType WHERE PaperTypeID=" + orderDetails.Rows[0][4].ToString()).Rows[0][0].ToString();
            order.OrderStatus = operations.executeSelectQuery("Select OrderStatusName from OrderStatus WHERE OrderStatusID=" + orderDetails.Rows[0][4].ToString()).Rows[0][0].ToString();
            return order;
        }

        public bool editOrder(Order order)
        {
            DatabaseOperations operations = new DatabaseOperations();
            int paperTypeID = int.Parse(operations.executeSelectQuery("SELECT PaperTypeID FROM PaperType WHERE PaperTypeName='" + order.PaperType + "'").Rows[0][0].ToString());
            int statusTypeID = int.Parse(operations.executeSelectQuery("SELECT OrderStatusID FROM OrderStatus WHERE OrderStatusName='" + order.OrderStatus + "'").Rows[0][0].ToString());
            int i = operations.executeInsUpdDelQuery("UPDATE OrderDetails SET DesignID='"+order.DesignID+"', PaperTypeID="+paperTypeID+", Size='"+order.Size+"', OrderStatusID="+statusTypeID+", Quantity="+order.Quantity+", DeliveryDate='"+order.DeliveryDate.ToString()+"', PerProductCost="+order.UnitPrice+", AdvancePayment="+order.AdvancePayment+" WHERE OrderID="+order.OrderID);
            int j = 1;
            if(order.FinalizedDesign != "Click Here")
                j = operations.executeUpdImageQuery("OrderDetails", "FinalDesignToBePrinted", order.FinalizedDesign, "OrderID", order.OrderID.ToString());
            if (i == 1 && j == 1)
                return true;
            else
                return false;
        }

        public int deleteOrder(int orderID)
        {
            DatabaseOperations operations = new DatabaseOperations();
            return operations.executeInsUpdDelQuery("DELETE FROM OrderDetails WHERE OrderID=" + orderID);
        }
    }
}
