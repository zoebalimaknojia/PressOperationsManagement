using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BusinessLogicLibrary
{
    class ReportOperations
    {
        public DataTable getMonthlyReport(string productName, int month, int year)
        {
            DatabaseOperations operations = new DatabaseOperations();
            int productCode = int.Parse(operations.executeSelectQuery("SELECT ProductID FROM Product WHERE ProductName='"+productName+"'").Rows[0][0].ToString());

            DataTable dt = null;
            return dt;
        }
    }
}
