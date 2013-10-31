using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Data.Sql;
using System.Data.SqlClient;

namespace BusinessLogicLibrary
{
    public class DatabaseOperations
    {
        public DataTable getTableData(string tableName, string columnName)
        {
            try
            {
                SqlConnection connection = DatabaseConnection.getConnection();
                SqlCommand selectcmd = new SqlCommand();
                selectcmd.CommandType = CommandType.Text;
                if (columnName == null)
                    selectcmd.CommandText = "Select * from " + tableName;
                else
                    selectcmd.CommandText = "Select "+columnName+" from "+tableName;
                selectcmd.Connection = connection;
                SqlDataAdapter adpt = new SqlDataAdapter();
                adpt.SelectCommand = selectcmd;
                DataSet ds = new DataSet();
                adpt.Fill(ds, tableName);
                return ds.Tables[tableName];
            }
            catch(DatabaseConnectionException ex)
            {
                throw ex;
            }
        }

        public DataTable executeSelectQuery(string selectQuery)
        {
            try
            {
                SqlConnection connection = DatabaseConnection.getConnection();
                SqlCommand selectcmd = new SqlCommand();
                selectcmd.CommandType = CommandType.Text;
                selectcmd.CommandText = selectQuery;
                selectcmd.Connection = connection;
                SqlDataAdapter adpt = new SqlDataAdapter();
                adpt.SelectCommand = selectcmd;
                DataSet ds = new DataSet();
                adpt.Fill(ds, "temporaryTable");
                return ds.Tables["temporaryTable"];
            }
            catch (DatabaseConnectionException ex)
            {
                throw ex;
            }
        }

        public int executeInsUpdDelQuery(string query)
        {
            try
            {
                SqlConnection connection = DatabaseConnection.getConnection();
                SqlCommand selectcmd = new SqlCommand();
                selectcmd.CommandType = CommandType.Text;
                selectcmd.CommandText = query;
                selectcmd.Connection = connection;
                try
                {
                    connection.Open();
                    return selectcmd.ExecuteNonQuery();
                }
                finally
                {
                    connection.Close();
                }
            }
            catch (DatabaseConnectionException ex)
            {
                throw ex;
            }
        }

        public int executeUpdImageQuery(string tableName, string columnName, string imagePath, string primaryKey, string primaryKeyValue)
        {
            try
            {
                FileStream fs = new FileStream(imagePath,FileMode.Open,FileAccess.Read);
                byte[] imageData = new Byte[fs.Length];
                fs.Read(imageData, 0, (int)fs.Length);                               
                SqlConnection connection = DatabaseConnection.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                if(tableName  == "Design")
                    cmd.CommandText = "UPDATE "+tableName+" SET "+columnName+" = @ImageData WHERE "+primaryKey+" = '"+primaryKeyValue+"'";
                else
                    cmd.CommandText = "UPDATE " + tableName + " SET " + columnName + " = @ImageData WHERE " + primaryKey + "=" + primaryKeyValue;
                cmd.Connection = connection;
                SqlParameter img = new SqlParameter();
                img.ParameterName = "ImageData";
                img.SqlDbType = SqlDbType.Image;
                img.SqlValue = imageData;
                cmd.Parameters.Add(img);
                try
                {
                    connection.Open();
                    return cmd.ExecuteNonQuery();
                }
                finally
                {
                    connection.Close();
                }
            }
            catch (DatabaseConnectionException ex)
            {
                throw ex;
            }
        }

        public void executeDatabaseRestore(string query)
        {
            try
            {
                SqlConnection connection = DatabaseConnection.getConnection();
                connection.ConnectionString = connection.ConnectionString.Replace("PressOperationsManagement", "master");
                SqlCommand selectcmd = new SqlCommand();
                selectcmd.CommandType = CommandType.Text;
                selectcmd.CommandText = query;
                selectcmd.Connection = connection;
                try
                {
                    connection.Open();
                    selectcmd.ExecuteNonQuery();
                }
                finally
                {
                    connection.Close();
                }
            }
            catch (DatabaseConnectionException ex)
            {
                throw ex;
            }
        }

        public void setDatabaseOffline(string query)
        {
            try
            {
                SqlConnection connection = DatabaseConnection.getConnection();
                SqlCommand selectcmd = new SqlCommand();
                selectcmd.CommandType = CommandType.Text;
                selectcmd.CommandText = query;
                selectcmd.Connection = connection;
                try
                {
                    connection.Open();
                    selectcmd.ExecuteNonQuery();
                }
                finally
                {
                    connection.Close();
                }
            }
            catch (DatabaseConnectionException ex)
            {
                throw ex;
            }
        }

        public void setDatabaseOnline(string query)
        {
            try
            {
                SqlConnection connection = DatabaseConnection.getConnection();
                connection.ConnectionString = connection.ConnectionString.Replace("PressOperationsManagement", "master");
                SqlCommand selectcmd = new SqlCommand();
                selectcmd.CommandType = CommandType.Text;
                selectcmd.CommandText = query;
                selectcmd.Connection = connection;
                try
                {
                    connection.Open();
                    selectcmd.ExecuteNonQuery();
                }
                finally
                {
                    connection.Close();
                }
            }
            catch (DatabaseConnectionException ex)
            {
                throw ex;
            }
        }
    }
}
