using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BusinessLogicLibrary
{
    public class ProductsOperations
    {
        public DataTable getAllCategories()
        {
            DatabaseOperations operations = new DatabaseOperations();
            DataTable categories = operations.executeSelectQuery("SELECT * FROM Category");

            return categories;
        }
        public DataTable getAllProducts(int categoryID)
        {
            DatabaseOperations operations = new DatabaseOperations();
            DataTable products = operations.executeSelectQuery("SELECT * FROM Product WHERE Product.CategoryID="+categoryID);
            return products;
        }
        public List<Design> getAllDesigns(string productName)
        {
            DatabaseOperations operations = new DatabaseOperations();
            int productID = int.Parse(operations.executeSelectQuery("SELECT ProductID FROM Product WHERE Product.ProductName='" + productName + "'").Rows[0][0].ToString());
            DataTable designs = operations.executeSelectQuery("SELECT * FROM Design WHERE Design.ProductID=" + productID);
            List<Design> designList = new List<Design>(); 
            foreach(DataRow dr in designs.Rows)
            {
                Design d = new Design();
                d.DesignID = dr[0].ToString();
                d.DesignName = dr[1].ToString();
                d.DesignFile = (byte[]) dr[2];
                string paperTypeName = operations.executeSelectQuery("SELECT PaperTypeName FROM PaperType WHERE PaperType.PaperTypeID=" + dr[5].ToString()).Rows[0][0].ToString();
                d.DesignPaperType = paperTypeName;
                d.DesignSize = dr[6].ToString();
                designList.Add(d);
            }
            return designList;
        }

        public List<Design> getDesignByID(string designID)
        {
            DatabaseOperations operations = new DatabaseOperations();
            DataTable designs = operations.executeSelectQuery("SELECT * FROM Design WHERE Design.DesignID='" + designID + "'");
            //int productID = int.Parse(operations.executeSelectQuery("SELECT ProductID FROM Product WHERE Product.ProductName='" + productName + "'").Rows[0][0].ToString());
            List<Design> designList = new List<Design>();
            foreach (DataRow dr in designs.Rows)
            {
                Design d = new Design();
                d.DesignID = dr[0].ToString();
                d.DesignName = dr[1].ToString();
                d.DesignFile = (byte[])dr[2];
                string paperTypeName = operations.executeSelectQuery("SELECT PaperTypeName FROM PaperType WHERE PaperType.PaperTypeID=" + dr[5].ToString()).Rows[0][0].ToString();
                d.DesignPaperType = paperTypeName;
                d.DesignSize = dr[6].ToString();
                designList.Add(d);
            }
            return designList;
        }

        public List<Design> getDesignByName(string designName)
        {
            DatabaseOperations operations = new DatabaseOperations();
            DataTable designs = operations.executeSelectQuery("SELECT * FROM Design WHERE Design.DesignName LIKE '" + designName + "%'");
            List<Design> designList = new List<Design>();
            foreach (DataRow dr in designs.Rows)
            {
                Design d = new Design();
                d.DesignID = dr[0].ToString();
                d.DesignName = dr[1].ToString();
                d.DesignFile = (byte[])dr[2];
                string paperTypeName = operations.executeSelectQuery("SELECT PaperTypeName FROM PaperType WHERE PaperType.PaperTypeID=" + dr[5].ToString()).Rows[0][0].ToString();
                d.DesignPaperType = paperTypeName;
                d.DesignSize = dr[6].ToString();
                designList.Add(d);
            }
            return designList;
        }
        public bool saveDesign(Design d)
        {
            try
            {
                DatabaseOperations operations = new DatabaseOperations();
                int paperTypeID = int.Parse(operations.executeSelectQuery("SELECT PaperTypeID FROM PaperType WHERE PaperType.PaperTypeName='" + d.DesignPaperType + "'").Rows[0][0].ToString());
                int productID = int.Parse(operations.executeSelectQuery("SELECT ProductID FROM Product WHERE Product.ProductName='" + d.ProductName + "'").Rows[0][0].ToString());
                int i = operations.executeInsUpdDelQuery("INSERT INTO Design (DesignID, DesignName, ProductID, PaperTypeID, Size) VALUES('" + d.DesignID + "','" + d.DesignName + "'," + productID + "," + paperTypeID + ",'" + d.DesignSize + "')");
                int j = operations.executeUpdImageQuery("Design", "DesignFile", d.DesignFilePath, "DesignID", d.DesignID);
                if (i == 1 && j == 1)
                    return true;
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool editDesign(Design d)
        {
            DatabaseOperations operations = new DatabaseOperations();
            int paperTypeID = int.Parse(operations.executeSelectQuery("SELECT PaperTypeID FROM PaperType WHERE PaperType.PaperTypeName='" + d.DesignPaperType + "'").Rows[0][0].ToString());
            return (operations.executeInsUpdDelQuery("UPDATE Design SET DesignName='" + d.DesignName + "', Size='" + d.DesignSize + "', PaperTypeID=" + paperTypeID + " WHERE DesignID='" + d.DesignID + "'") == 1);
        }

        public bool addCategory(string categoryName)
        {
            DatabaseOperations operations = new DatabaseOperations();
            DataTable categoryIDs = operations.executeSelectQuery("SELECT CategoryID FROM Category");
            int lastcategoryID = 0;
            if(categoryIDs.Rows.Count > 0)
                lastcategoryID = int.Parse(categoryIDs.Rows[categoryIDs.Rows.Count-1][0].ToString());
            if (operations.executeInsUpdDelQuery("INSERT INTO Category VALUES("+(lastcategoryID+1)+",'"+categoryName+"')") == 1)
                return true;
            else
                return false;
        }

        public bool addProduct(string categoryName, string productName)
        {
            DatabaseOperations operations = new DatabaseOperations();
            int categoryID = int.Parse(operations.executeSelectQuery("SELECT CategoryID FROM Category WHERE CategoryName='"+categoryName+"'").Rows[0][0].ToString());
            DataTable productIDs = operations.executeSelectQuery("SELECT ProductID FROM Product");
            int lastProductID = 0;
            if (productIDs.Rows.Count > 0)
                lastProductID = int.Parse(productIDs.Rows[productIDs.Rows.Count - 1][0].ToString());
            if (operations.executeInsUpdDelQuery("INSERT INTO Product VALUES(" + (lastProductID + 1) + ","+categoryID+",'" + productName + "')") == 1)
                return true;
            else
                return false;
        }
    }
}
