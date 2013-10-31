using System;
using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;

namespace BusinessLogicLibrary
{
    public class DatabaseConnection
    {
        public static SqlConnection getConnection()
        {
            FileInfo connectionInfo = new FileInfo("conf\\server_info.txt");
            string s;
            string PRIMARY_SERVER = null;
            string SECONDARY_SERVER = null;
            if(connectionInfo.Exists)
            {
                StreamReader objReader = new StreamReader(connectionInfo.ToString());
                s = objReader.ReadLine();
                if (s.Contains("PRIMARY SERVER"))
                    PRIMARY_SERVER = s;
                if (s.Contains("SECONDARY SERVER"))
                    SECONDARY_SERVER = s;
                s = objReader.ReadLine();

                if (s.Contains("PRIMARY SERVER"))
                    PRIMARY_SERVER = s;
                if (s.Contains("SECONDARY SERVER"))
                    SECONDARY_SERVER = s;
                objReader.Close();
            }
            SqlConnection connectionInstance = new SqlConnection();
            try
            {
                connectionInstance.ConnectionString = @"Data Source="+ (PRIMARY_SERVER.Split('='))[1].Trim() + "\\SQLEXPRESS;Initial Catalog=PressOperationsManagement;User ID=yamuna";
                connectionInstance.Open();
                connectionInstance.Close();
                return connectionInstance;
            }
            catch(SqlException exception)
            {
                try
                {
                    connectionInstance.ConnectionString = @"Data Source=" + (SECONDARY_SERVER.Split('='))[1].Trim() + "\\SQLEXPRESS;Initial Catalog=PressOperationsManagement;User ID=yamuna";
                    connectionInstance.Open();
                    connectionInstance.Close();
                    string newConf = "PRIMARY SERVER=" + (SECONDARY_SERVER.Split('='))[1].Trim() + "\r\nSECONDARY SERVER=" + (PRIMARY_SERVER.Split('='))[1].Trim();
                    File.WriteAllText(connectionInfo.ToString(), newConf);
                    return connectionInstance;
                }
                catch (SqlException ex)
                {
                    throw new DatabaseConnectionException();
                }
            }
        }
    }
}
