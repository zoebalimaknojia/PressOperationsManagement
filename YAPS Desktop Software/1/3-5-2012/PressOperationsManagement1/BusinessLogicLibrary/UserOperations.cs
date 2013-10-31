using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BusinessLogicLibrary
{
    public class UserOperations
    {
        public LoginStatus isValidUser(String username, String password)
        {

            Employee employee = null;
            DatabaseOperations operations = new DatabaseOperations();
            DataTable users = new DataTable();
            users = operations.executeSelectQuery("Select * from UserDetails where UserName='" + username + "' AND Password='" + password + "'");
            if (users.Rows.Count == 1)
            {
                DataTable userType = new DataTable();
                userType = operations.executeSelectQuery("Select DesignationName FROM Designation where DesignationID =(Select DesignationID from EmployeeDetails where UserName='" + username + "')");

                if(userType.Rows[0][0].ToString().Equals("Admin"))
                    employee = new Admin();
                else if(userType.Rows[0][0].ToString().Equals("Receptionist"))
                    employee = new Receptionist();
                else if(userType.Rows[0][0].ToString().Equals("Designer"))
                    employee = new Designer();
                else if(userType.Rows[0][0].ToString().Equals("Manufacturing Worker"))
                    employee = new ManufacturingWorker();

                employee.EmployeeName = username;
                if(username.Equals(password))
                    return new LoginStatus(true, true,employee);
                else
                    return new LoginStatus(false, true, employee);
            }
            else
                return new LoginStatus(false, false, employee);
        }

        public bool changePassword(String userName, String oldPassword, string newPassword)
        {
            try
            {
                DatabaseOperations operations = new DatabaseOperations();
                DataTable users = new DataTable();
                users = operations.executeSelectQuery("Select * from UserDetails where UserName='" + userName + "' AND Password='" + oldPassword + "'");
                if (users.Rows.Count == 1)
                {
                    if (operations.executeInsUpdDelQuery("UPDATE UserDetails SET Password='"+newPassword+"' WHERE UserName='"+userName+"'") == 1)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool addUser(String employeeName, String userName, String password, String designation)
        {
            DatabaseOperations operations = new DatabaseOperations();
            String addUserQuery = "INSERT INTO UserDetails (UserName, Password) VALUES ('"+userName+"','"+password+"')";
            int lastEmployeeID;
            if(operations.executeSelectQuery("SELECT TOP 1 EmployeeID FROM EmployeeDetails ORDER BY EmployeeID DESC").Rows.Count>0)
                lastEmployeeID = int.Parse(operations.executeSelectQuery("SELECT TOP 1 EmployeeID FROM EmployeeDetails ORDER BY EmployeeID DESC").Rows[0][0].ToString());
            else
                lastEmployeeID = 101;
            int designationID = int.Parse(operations.executeSelectQuery("SELECT DesignationID FROM Designation WHERE DesignationName='"+designation.ToLower()+"'").Rows[0][0].ToString());
            String addEmployeeQuery = "INSERT INTO EmployeeDetails VALUES("+(lastEmployeeID+1)+","+designationID+",'"+employeeName+"','"+userName+"')";

            try
            {
                if (operations.executeInsUpdDelQuery(addUserQuery) == 1 && operations.executeInsUpdDelQuery(addEmployeeQuery) == 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool resetPassword(String userName)
        {
            DatabaseOperations operations = new DatabaseOperations();
            String resetPasswordQuery = "UPDATE UserDetails SET Password='"+userName+"' WHERE UserName='" + userName + "'";

            try
            {
                if (operations.executeInsUpdDelQuery(resetPasswordQuery) == 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool deleteUser(String userName)
        {
            DatabaseOperations operations = new DatabaseOperations();
            String deleteUserQuery = "DELETE FROM UserDetails WHERE UserName='" + userName + "'";
            String deleteEmployeeQuery = "DELETE FROM EmployeeDetails WHERE UserName='" + userName + "'";


            try
            {
                if (operations.executeInsUpdDelQuery(deleteUserQuery) == 1 && operations.executeInsUpdDelQuery(deleteEmployeeQuery) == 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
