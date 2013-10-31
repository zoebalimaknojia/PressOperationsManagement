#region namespaces
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CloseableTabItemDemo;
using System.Timers;
using System.IO;
using System.Windows.Forms;
using BusinessLogicLibrary;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Security.AccessControl;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Text.RegularExpressions;
using System.Diagnostics;

#endregion
namespace PresentationLayer
{
    public partial class HomeWindow : Window
    {

        #region variables

        int NewOrderCount = 0;
        int desctriptionCount = 0;
        int ExistingOrderCount = 0;
        int AddUsersCount = 0;
        int ResetPasswordCount = 0;
        int DeleteUsersCount = 0;
        int Report1Count = 0;
        int Report2Count = 0;
        int BackupDatabaseCount = 0;
        int RestoreDatabaseCount = 0;
        int NewCategoryCount = 0;
        int NewProductCount = 0;
        int NewDesignCount = 0;
        int ViewDesignCount = 0;
        int RemainingPaymentCount = 0;
        int ApplyPaymentCount = 0;
        String userName;
        String userPassword;
        Employee emp;


        BindingDataset nds = new BindingDataset();

        BindingDatasetTableAdapters.CategoryTableAdapter cat = new BindingDatasetTableAdapters.CategoryTableAdapter();
        BindingDatasetTableAdapters.ProductTableAdapter pro = new BindingDatasetTableAdapters.ProductTableAdapter();
        BindingDatasetTableAdapters.OrderDetailsTableAdapter ord = new BindingDatasetTableAdapters.OrderDetailsTableAdapter();
        BindingDatasetTableAdapters.OrderStatusTableAdapter ords = new BindingDatasetTableAdapters.OrderStatusTableAdapter();
        BindingDatasetTableAdapters.CustomerDetailsTableAdapter cust = new BindingDatasetTableAdapters.CustomerDetailsTableAdapter();
        BindingDatasetTableAdapters.DesignTableAdapter desig = new BindingDatasetTableAdapters.DesignTableAdapter();
        BindingDatasetTableAdapters.DesignationTableAdapter designation = new BindingDatasetTableAdapters.DesignationTableAdapter();
        BindingDatasetTableAdapters.EmployeeDetailsTableAdapter empTab = new BindingDatasetTableAdapters.EmployeeDetailsTableAdapter();
        BindingDatasetTableAdapters.PaperTypeTableAdapter pap = new BindingDatasetTableAdapters.PaperTypeTableAdapter();
        BindingDatasetTableAdapters.UserDetailsTableAdapter usr = new BindingDatasetTableAdapters.UserDetailsTableAdapter();

        #endregion

        #region Constructor

        public HomeWindow(String userName, String password, Employee emp)
        {
            this.emp = emp;
            this.userName = userName;
            this.userPassword = password;
            InitializeComponent();

            label10.Content += emp.EmployeeName;

            if (!emp.Orders)
                OrdersExpander.Visibility = Visibility.Collapsed;
            if (!emp.NewOrder)
                NewOrderslabel.Visibility = Visibility.Collapsed;
            if (!emp.ExistingOrders)
                ExistingOrderslabel.Visibility = Visibility.Collapsed;

            if (!emp.AccountSettings)
                AccountSettingsExpander.Visibility = Visibility.Collapsed;

            if (!emp.Reports)
                ReportsExpander.Visibility = Visibility.Collapsed;

            if (!emp.DatabaseOperations)
                DatabaseOperationsExpander.Visibility = Visibility.Collapsed;

            if (!emp.ProductsAndDesigns)
                ProductsAndDesignsExpander.Visibility = Visibility.Collapsed;

            if (!emp.Payments)
                PaymentsExpander.Visibility = Visibility.Collapsed;

            if (!emp.AddCategory)
                NewCategorylabel.Visibility = Visibility.Collapsed;

            if (!emp.AddProduct)
                NewProductlabel.Visibility = Visibility.Collapsed;

            if (!emp.AddDesign)
                NewDesignlabel.Visibility = Visibility.Collapsed;

            if (!emp.ViewDesign)
                ViewDesignlabel.Visibility = Visibility.Collapsed;

            datePicker1.DisplayDateStart = DateTime.Today;

            tabControl1.Height = 600;

            tabControl1.Items.Remove(NewOrder);
            tabControl1.Items.Remove(ExistingOrders);
            tabControl1.Items.Remove(AddUsers);
            tabControl1.Items.Remove(ResetPassword);
            tabControl1.Items.Remove(DeleteUsers);
            tabControl1.Items.Remove(Report1);
            tabControl1.Items.Remove(Report2);
            tabControl1.Items.Remove(BackupDatabase);
            tabControl1.Items.Remove(RestoreDatabase);
            tabControl1.Items.Remove(NewCategory);
            tabControl1.Items.Remove(NewProduct);
            tabControl1.Items.Remove(NewDesign);
            tabControl1.Items.Remove(ViewDesign);
            tabControl1.Items.Remove(RemainingPayment);
            tabControl1.Items.Remove(ApplyPayment);
            tabControl1.Items.Remove(description);

            this.WindowState = System.Windows.WindowState.Maximized;//this will open the window in maximized mode    
            this.WindowStyle = System.Windows.WindowStyle.ThreeDBorderWindow;//it will create a border
            this.ResizeMode = System.Windows.ResizeMode.CanMinimize;//window can be minimized

        }

        #endregion

        #region NewOrder

        private void NewOrderslabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //on click of New Order in Expander 
            if (NewOrderCount == 0)
            {
                tabControl1.Items.Add(NewOrder);
                OrderOperations operations = new OrderOperations();
                label35.Content = operations.returnNextOrderID();
                comboBox4.SelectedIndex = -1;
                comboBox3.SelectedIndex = -1;
                DatabaseOperations dboperations = new DatabaseOperations();
                comboBox3.DataContext = dboperations.getTableData("PaperType", "PaperTypeName");
                datePicker1.SelectedDate = DateTime.Now;
                textBox2.TextChanged += new TextChangedEventHandler(txtBox2_textChanged);
                NewOrder.Focus();
                NewOrderCount++;
            }
            else
            {
                NewOrder.Focus();
            }
        }

        private void NewOrder_CloseTab(object sender, RoutedEventArgs e)
        {
            button4_Click(sender, e);
            tabControl1.Items.Remove(NewOrder);
            NewOrderCount--;
        }

        //for selecting design
        private void button10_Click(object sender, RoutedEventArgs e)
        {
            FileDialog file = new OpenFileDialog();
            file.Filter = "jpeg files (*.jpeg)|*.jpg|jpg files (*.jpg)|*.jpg|Corel files (*.cdr)|*.cdr|Photoshop files (*.psd)|*.psd";
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox3.Text = file.FileName;
                datePicker1.Focus();
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Order newOrder = new Order();
            try
            {
                newOrder.OrderID = int.Parse(label35.Content.ToString());
                newOrder.Customer = new Customer(0, textBox2.Text, txtadd.Text, long.Parse(textBox4.Text));
                newOrder.DesignID = textBox5.Text;
                newOrder.OrderStatus = "Order Taken";
                newOrder.PaperType = comboBox3.Text;
                newOrder.Size = textBox10.Text + "X" + textBox7.Text + " " + comboBox4.SelectionBoxItem.ToString();
                label52.Visibility = Visibility.Hidden;
            }
            catch (FormatException ex)
            {
                label52.Content = "Some Information not entered";
                label52.Visibility = Visibility.Visible;
                return;
            }
            try
            {
                newOrder.Quantity = int.Parse(textBox8.Text);
                label52.Visibility = Visibility.Hidden;
            }
            catch (FormatException ex)
            {
                label52.Content = "Enter quantity in number format";
                label52.Visibility = Visibility.Visible;
                return;
            }
            catch (ArgumentNullException ex)
            {
                label52.Content = "Please Enter Quantity";
                label52.Visibility = Visibility.Visible;
                return;
            }
            if (textBox3.Text != "")
            {
                label52.Visibility = Visibility.Hidden;
                newOrder.FinalizedDesign = textBox3.Text;
                FileInfo f = new FileInfo(textBox3.Text);
                if (f.Exists)
                    label52.Visibility = Visibility.Hidden;
                else
                {
                    label52.Content = "Design File Invalid";
                    label52.Visibility = Visibility.Visible;
                    return;
                }
            }
            else
            {
                label52.Content = "Select design file";
                label52.Visibility = Visibility.Visible;
                return;
            }
            
            newOrder.DeliveryDate = (DateTime)datePicker1.SelectedDate;
            try
            {
                newOrder.UnitPrice = int.Parse(textBox14.Text);
                label52.Visibility = Visibility.Hidden;
            }
            catch (FormatException ex)
            {
                label52.Content = "Enter Unit Price in number format";
                label52.Visibility = Visibility.Visible;
                return;
            }
            catch (ArgumentNullException ex)
            {
                label52.Content = "Please Enter Per Unit Price";
                label52.Visibility = Visibility.Visible;
                return;
            }

            try
            {
                newOrder.AdvancePayment = int.Parse(textBox15.Text);
                label52.Visibility = Visibility.Hidden;
            }
            catch (FormatException ex)
            {
                label52.Content = "Enter Advance Payment in number format";
                label52.Visibility = Visibility.Visible;
                return;
            }
            catch (ArgumentNullException ex)
            {
                label52.Content = "Please Enter Advance Payment";
                label52.Visibility = Visibility.Visible;
                return;
            }

            try
            {
                int.Parse(textBox15.Text);
                if (int.Parse(textBox15.Text) > (int.Parse(textBox8.Text) * int.Parse(textBox14.Text)))
                {
                    label52.Content = "Advance Payment greater than cost!";
                    label52.Visibility = Visibility.Visible;
                    return;
                }
                label52.Visibility = Visibility.Hidden;
            }
            catch (FormatException ex)
            {
                label52.Content = "Advance Payment should be a number";
                label52.Visibility = Visibility.Visible;
                return;
            }
            catch (ArgumentNullException ex)
            {
                label52.Content = "Please Enter Advance Payment";
                label52.Visibility = Visibility.Visible;
                return;
            }
            
            OrderOperations operations = new OrderOperations();
            if (label52.Visibility == Visibility.Hidden)
            {
                if (operations.addNewOrder(newOrder))
                {
                    button4_Click(sender, e);
                    label52.Visibility = Visibility.Hidden;
                    OrderOperations operations1 = new OrderOperations();
                    if (System.Windows.MessageBox.Show("Order Taken Successfully") == MessageBoxResult.OK)
                    {
                        label35.Content = operations1.returnNextOrderID();
                        generateAdvanceReceipt(newOrder);
                        if (ExistingOrderCount == 1)
                        {
                            ExistingOrders_CloseTab(sender, e);
                            ExistingOrderslabel_MouseDown(sender, null);
                            label83_MouseUp(sender, null);
                            NewOrder.Focus();
                        }
                        if (RemainingPaymentCount == 1)
                        {
                            RemainingPayment_CloseTab(sender, e);
                            RemainingPaymentslabel_MouseDown(sender, null);
                            NewOrder.Focus();
                        }
                    }
                }
                else
                {
                    label52.Content = "Order Not Taken Successfully";
                    label52.Visibility = Visibility.Visible;
                    return;
                }
            }
            else
            {
                System.Windows.MessageBox.Show("See the error in right bottom corner");
                return;
            }
        }

        public void generateAdvanceReceipt(Order newOrder)
        {
            ReportDocument advancePaymentReceipt = new ReportDocument();
            FileInfo file = new FileInfo("Reports/Advance_Payment/AdvancePayment.rpt");
            if (file.Exists)
            {
                advancePaymentReceipt.Load(file.FullName);

                ParameterFieldDefinitions crParameterFieldDefinitions;
                ParameterFieldDefinition crParameterFieldDefinition;
                ParameterValues crParameterValues = new ParameterValues();
                ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();

                crParameterFieldDefinitions = advancePaymentReceipt.DataDefinition.ParameterFields;

                crParameterDiscreteValue.Value = newOrder.OrderID;
                crParameterFieldDefinition = crParameterFieldDefinitions["orderID"];
                crParameterValues = crParameterFieldDefinition.CurrentValues;
                crParameterValues.Clear();
                crParameterValues.Add(crParameterDiscreteValue);
                crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

                crParameterDiscreteValue.Value = newOrder.Customer.Name;
                crParameterFieldDefinition = crParameterFieldDefinitions["customerName"];
                crParameterValues = crParameterFieldDefinition.CurrentValues;
                crParameterValues.Clear();
                crParameterValues.Add(crParameterDiscreteValue);
                crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

                crParameterDiscreteValue.Value = newOrder.DesignID;
                crParameterFieldDefinition = crParameterFieldDefinitions["designCode"];
                crParameterValues = crParameterFieldDefinition.CurrentValues;
                crParameterValues.Clear();
                crParameterValues.Add(crParameterDiscreteValue);
                crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

                crParameterDiscreteValue.Value = newOrder.PaperType;
                crParameterFieldDefinition = crParameterFieldDefinitions["paperType"];
                crParameterValues = crParameterFieldDefinition.CurrentValues;
                crParameterValues.Clear();
                crParameterValues.Add(crParameterDiscreteValue);
                crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

                crParameterDiscreteValue.Value = newOrder.Size;
                crParameterFieldDefinition = crParameterFieldDefinitions["size"];
                crParameterValues = crParameterFieldDefinition.CurrentValues;
                crParameterValues.Clear();
                crParameterValues.Add(crParameterDiscreteValue);
                crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

                crParameterDiscreteValue.Value = newOrder.DeliveryDate;
                crParameterFieldDefinition = crParameterFieldDefinitions["deliveryDate"];
                crParameterValues = crParameterFieldDefinition.CurrentValues;
                crParameterValues.Clear();
                crParameterValues.Add(crParameterDiscreteValue);
                crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

                crParameterDiscreteValue.Value = newOrder.Quantity;
                crParameterFieldDefinition = crParameterFieldDefinitions["quantity"];
                crParameterValues = crParameterFieldDefinition.CurrentValues;
                crParameterValues.Clear();
                crParameterValues.Add(crParameterDiscreteValue);
                crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

                crParameterDiscreteValue.Value = newOrder.UnitPrice;
                crParameterFieldDefinition = crParameterFieldDefinitions["unitCost"];
                crParameterValues = crParameterFieldDefinition.CurrentValues;
                crParameterValues.Clear();
                crParameterValues.Add(crParameterDiscreteValue);
                crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

                crParameterDiscreteValue.Value = ((newOrder.Quantity * newOrder.UnitPrice) - newOrder.AdvancePayment);
                crParameterFieldDefinition = crParameterFieldDefinitions["remainingPayment"];
                crParameterValues = crParameterFieldDefinition.CurrentValues;
                crParameterValues.Clear();
                crParameterValues.Add(crParameterDiscreteValue);
                crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

                crParameterDiscreteValue.Value = (newOrder.Quantity * newOrder.UnitPrice);
                crParameterFieldDefinition = crParameterFieldDefinitions["totalCost"];
                crParameterValues = crParameterFieldDefinition.CurrentValues;
                crParameterValues.Clear();
                crParameterValues.Add(crParameterDiscreteValue);
                crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

                crParameterDiscreteValue.Value = newOrder.AdvancePayment;
                crParameterFieldDefinition = crParameterFieldDefinitions["advancePayment"];
                crParameterValues = crParameterFieldDefinition.CurrentValues;
                crParameterValues.Clear();
                crParameterValues.Add(crParameterDiscreteValue);
                crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

                advancePaymentReceipt.ExportToDisk(ExportFormatType.WordForWindows, newOrder.OrderID + ".doc");
                if(File.Exists(Environment.CurrentDirectory + "\\Reports\\Advance_Payment\\" + newOrder.OrderID + ".doc"))
                    File.Delete(Environment.CurrentDirectory + "\\Reports\\Advance_Payment\\" + newOrder.OrderID + ".doc");
                File.Move(newOrder.OrderID + ".doc", Environment.CurrentDirectory + "\\Reports\\Advance_Payment\\" + newOrder.OrderID + ".doc");
                advancePaymentReceipt.PrintToPrinter(1, false, 1, 1);
                advancePaymentReceipt.Dispose();
            }
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            textBox2.Text = "";
            textBox4.Text = "";
            txtadd.Text = "";
            textBox5.Text = "";
            label54.Content = "";
            label22.Content = "";
            comboBox3.SelectedIndex = -1;
            textBox10.Text = "";
            textBox7.Text = "";
            comboBox4.SelectedIndex = 0;
            textBox3.Text = "";
            textBox8.Text = "";
            datePicker1.SelectedDate = DateTime.Now;
            textBox14.Text = "";
            textBox15.Text = "";
            label52.Visibility = Visibility.Hidden;
        }

        private void textBox14_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                label22.Content = "Total Cost : " + (int.Parse(textBox8.Text) * int.Parse(textBox14.Text));
                label52.Visibility = Visibility.Hidden;
            }
            catch (FormatException ex)
            {
                label52.Content = "Per Unit Cost should be a number";
                label52.Visibility = Visibility.Visible;
                return;
            }
            catch (ArgumentNullException ex)
            {
                label52.Content = "Please Enter Per Unit Price";
                label52.Visibility = Visibility.Visible;
                return;
            }
        }

        private void textBox8_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                int.Parse(textBox8.Text);
                label52.Visibility = Visibility.Hidden;
            }
            catch (FormatException ex)
            {
                label52.Content = "Enter quantity in number format";
                label52.Visibility = Visibility.Visible;
                return;
            }
            catch (ArgumentNullException ex)
            {
                label52.Content = "Please Enter Quantity";
                label52.Visibility = Visibility.Visible;
                return;
            }
        }

        private void textBox15_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                int.Parse(textBox15.Text);
                if(int.Parse(textBox15.Text) > (int.Parse(textBox8.Text) * int.Parse(textBox14.Text)))
                {
                    label52.Content = "Advance Payment greater than cost!";
                    label52.Visibility = Visibility.Visible;
                    return;
                }
                label52.Visibility = Visibility.Hidden;
            }
            catch (FormatException ex)
            {
                label52.Content = "Advance Payment should be a number";
                label52.Visibility = Visibility.Visible;
                return;
            }
            catch (ArgumentNullException ex)
            {
                label52.Content = "Please Enter Advance Payment";
                label52.Visibility = Visibility.Visible;
                return;
            }
        }



        private void textBox5_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            ProductsOperations operations = new ProductsOperations();
            List<Design> design = operations.getDesignByID(textBox5.Text);
            if (design.Count == 1)
            {
                try
                {
                    label52.Visibility = Visibility.Hidden;
                    label54.Content = design[0].DesignName;
                    comboBox3.Text = design[0].DesignPaperType;
                    String size = design[0].DesignSize;
                    size.Trim();
                    String[] a = size.Split('X');
                    textBox10.Text = a[0];
                    String[] b = a[1].Trim().Split(' ');
                    textBox7.Text = b[0];
                    if (b[1].StartsWith("C") || b[1].StartsWith("c"))
                        comboBox4.SelectedIndex = 0;
                    else if (b[1].StartsWith("I") || b[1].StartsWith("i"))
                        comboBox4.SelectedIndex = 1;
                    else if (b[1].StartsWith("F") || b[1].StartsWith("f"))
                        comboBox4.SelectedIndex = 2;
                    else if (b[1].StartsWith("M") || b[1].StartsWith("m"))
                        comboBox4.SelectedIndex = 3;
                    else
                        comboBox4.SelectedIndex = 1;
                }
                catch (Exception ex)
                {
                    return;
                }
            }
            else
            {
                label52.Content = "Invalid Design Code";
                label52.Visibility = Visibility.Visible;
                return;
            }
        }

        private void txtBox2_textChanged(object sender, TextChangedEventArgs e)
        {
            listBox1.Items.Clear();
            if (textBox2.Text != "")
            {
                String name = textBox2.Text;
                DatabaseOperations operations = new DatabaseOperations();
                DataTable dt = operations.executeSelectQuery("SELECT * FROM CustomerDetails WHERE CustomerName LIKE '" + name + "%'");
                foreach (DataRow dr in dt.Rows)
                {
                    listBox1.Items.Add(dr[1].ToString() + "," + dr[3].ToString());
                }

                if (listBox1.Items.Count > 0)
                {
                    listBox1.Visibility = Visibility.Visible;
                }
            }
            else
            {
                listBox1.Visibility = Visibility.Collapsed;
                listBox1.Items.Clear();
            }
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                String name = listBox1.SelectedValue.ToString();
                String[] details = name.Split(',');
                DatabaseOperations operations = new DatabaseOperations();
                DataTable dt = operations.executeSelectQuery("SELECT CustomerAddress FROM CustomerDetails WHERE CustomerName='" + details[0] + "' AND CustomerContactNumber=" + long.Parse(details[1]));
                textBox2.Text = details[0];
                textBox4.Text = details[1];
                txtadd.Text = dt.Rows[0][0].ToString();
                listBox1.Visibility = Visibility.Collapsed;
                listBox1.Items.Clear();
            }
        }

        private void textBox2_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            listBox1.Visibility = Visibility.Collapsed;
            Regex regex = new Regex("[A-Za-z. ]$");
            if (!regex.IsMatch(textBox2.Text))
            {
                label52.Content = "Invalid Name Entered";
                label52.Visibility = Visibility.Visible;
            }
            else
                label52.Visibility = Visibility.Hidden;
            try{
                int i = listBox1.Items.IndexOf(textBox2.Text + "," + textBox4.Text);
                if (i <=-1) { textBox4.Text = ""; txtadd.Text = ""; }
            }
            catch (Exception) { textBox4.Text = ""; txtadd.Text = ""; }

        }

        private void textBox4_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Regex regex = new Regex("^[0-9]{8,12}$");
            if (!regex.IsMatch(textBox4.Text))
            {
                label52.Content = "Invalid Contact Number Entered";
                label52.Visibility = Visibility.Visible;
            }
            else
                label52.Visibility = Visibility.Hidden;
        }

        private void textBox10_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Regex regex = new Regex("^[0-9]{1,4}$");
            if (!regex.IsMatch(textBox10.Text))
            {
                label52.Content = "Invalid Size Entered";
                label52.Visibility = Visibility.Visible;
            }
            else
                label52.Visibility = Visibility.Hidden;
        }

        private void textBox7_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Regex regex = new Regex("^[0-9]{1,4}$");
            if (!regex.IsMatch(textBox7.Text))
            {
                label52.Content = "Invalid Size Entered";
                label52.Visibility = Visibility.Visible;
            }
            else
                label52.Visibility = Visibility.Hidden;
        }
        #endregion 

        #region ExistingOrders

        private void ExistingOrderslabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //on click of Existing Order in Expander
            if (ExistingOrderCount == 0)
            {
                datePicker3.SelectedDate = DateTime.Now.Date;
                tabControl1.Items.Add(ExistingOrders);
                ExistingOrders.Focus();
                ExistingOrderCount++;
                dataGrid1.Visibility = Visibility.Hidden;
            }
            else
            {
                ExistingOrders.Focus();
            }
        }

        private void ExistingOrders_CloseTab(object sender, RoutedEventArgs e)
        {
            label52.Visibility = Visibility.Hidden;
            tabControl1.Items.Remove(ExistingOrders);
            ExistingOrderCount--;
            dataGrid1.DataContext = null;
            dataGrid1.Visibility = Visibility.Hidden;
        }

        private void button25_Click(object sender, RoutedEventArgs e)
        {
            OrderOperations operations = new OrderOperations();
            DataTable dt = null; ;

            if (comboBox14.Text == "ID")
            {
                try
                {
                    label79.Visibility = Visibility.Hidden;
                    dt = operations.searchOrder(int.Parse(textBox9.Text));
                }
                catch (Exception ex)
                {
                    label79.Content = "Invalid Order ID Entered";
                    label79.Visibility = Visibility.Visible;
                    return;
                }
            }
            else if (comboBox14.Text == "Name")
            {
                dt = operations.searchOrder(textBox9.Text);
            }
            else if (comboBox14.Text == "Delivery Date")
            {
                dt = operations.searchOrder(((DateTime)datePicker3.SelectedDate).Date);
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                DatabaseOperations operation = new DatabaseOperations();
                DataSet ds = new DataSet();
                DataTable dt1 = dt.Copy();
                dt1.TableName = "OrderDetails";
                ds.Tables.Add(dt1);
                dt1 = operation.getTableData("OrderStatus", null).Copy();
                dt1.TableName = "OrderStatus";
                ds.Tables.Add(dt1);
                dt1 = operation.getTableData("PaperType", null).Copy();
                dt1.TableName = "PaperType";
                ds.Tables.Add(dt1);
                dataGrid1.DataContext = ds;
                label79.Visibility = Visibility.Hidden;
                dataGrid1.Visibility = Visibility.Visible;
            }
            else
            {
                label79.Content = "No Results Found";
                label79.Visibility = Visibility.Visible;
            }
        }

        private void comboBox14_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (comboBox14.Text == "ID")
            {
                datePicker3.Visibility = Visibility.Hidden;
                textBox9.Visibility = Visibility.Visible;
            }
            if (comboBox14.Text == "Name")
            {
                datePicker3.Visibility = Visibility.Hidden;
                textBox9.Visibility = Visibility.Visible;
            }
            if (comboBox14.Text == "Delivery Date")
            {
                datePicker3.Visibility = Visibility.Visible;
                textBox9.Visibility = Visibility.Hidden;
            }
        }

        private void deleteImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Do you really want to delete this order?", "Delete Order", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DataRowView d = (DataRowView)dataGrid1.SelectedItems[0];
                int orderID = int.Parse(d.Row[0].ToString());
                OrderOperations operations = new OrderOperations();
                if (operations.deleteOrder(orderID) == 1)
                {
                    ExistingOrders_CloseTab(sender, e);
                    ExistingOrderslabel_MouseDown(sender, e);
                    label83_MouseUp(sender, e);
                }
            }
        }

        private void label83_MouseUp(object sender, MouseButtonEventArgs e)
        {

            OrderOperations operations = new OrderOperations();
            DatabaseOperations operation = new DatabaseOperations();
            DataSet ds = new DataSet();
            DataTable dt = operations.getAllOrders().Copy();
            dt.TableName = "OrderDetails";
            ds.Tables.Add(dt);
            dt = operation.getTableData("OrderStatus", null).Copy();
            dt.TableName = "OrderStatus";
            ds.Tables.Add(dt);
            dt = operation.getTableData("PaperType", null).Copy();
            dt.TableName = "PaperType";
            ds.Tables.Add(dt);
            dataGrid1.DataContext = ds;
            dataGrid1.Visibility = Visibility.Visible;
        }


        private void comboBox14_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textBox9.Text = "";
        }

        #endregion

        #region Description

        Order eorderDetails;
        private void Description_Click(object sender, RoutedEventArgs e)
        {
            if (desctriptionCount == 0)
            {
                tabControl1.Items.Add(description);
                desctriptionCount++;
            }
            description.Focus();
            DatabaseOperations doperations = new DatabaseOperations();
            comboBox11.DataContext = doperations.getTableData("PaperType", null);
            comboBox13.DataContext = doperations.getTableData("OrderStatus", null);
            int index = dataGrid1.SelectedIndex;
            DataRowView d = (DataRowView)dataGrid1.SelectedItems[0];
            int orderID = int.Parse(d.Row[0].ToString());
            OrderOperations operations = new OrderOperations();
            eorderDetails = operations.getOrderByID(orderID);
            label58.Content = eorderDetails.OrderID;
            label60.Content = eorderDetails.Customer.Name;
            label78.Content = eorderDetails.Customer.ContactNumber;
            label63.Content = eorderDetails.Customer.Address;
            textBox11.Text = eorderDetails.DesignID;
            label66.Content = eorderDetails.DesigName;
            comboBox11.Text = eorderDetails.PaperType;
            
            String size = eorderDetails.Size;
            size.Trim();
            String[] a = size.Split('X');
            textBox16.Text = a[0].Trim();
            String[] b = a[1].Trim().Split(' ');
            textBox23.Text = b[0];
            if (b[1].StartsWith("C") || b[1].StartsWith("c"))
                comboBox12.SelectedIndex = 0;
            else if (b[1].StartsWith("I") || b[1].StartsWith("i"))
                comboBox12.SelectedIndex = 1;
            else if (b[1].StartsWith("F") || b[1].StartsWith("f"))
                comboBox12.SelectedIndex = 2;
            else if (b[1].StartsWith("M") || b[1].StartsWith("m"))
                comboBox12.SelectedIndex = 3;
            else
                comboBox4.SelectedIndex = 1;

            comboBox13.Text = eorderDetails.OrderStatus;
            
            textBox12.Text = eorderDetails.Quantity.ToString();
            
            MemoryStream ms = new MemoryStream(eorderDetails.DesignFile);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            img.Save("temp.jpeg");

            comboBox13.Text = eorderDetails.OrderStatus;
            
            datePicker2.Text = eorderDetails.DeliveryDate.Date.ToShortDateString();

            textBox20.Text = eorderDetails.UnitPrice.ToString();

            textBox21.Text = eorderDetails.AdvancePayment.ToString();
            
            label74.Content = "Total Cost : " + (eorderDetails.UnitPrice * eorderDetails.Quantity).ToString();
         
        }

        public void displayAppropriateControls()
        {
            if (emp.Type == "manufacturingworker")
            {
                textBox11.IsReadOnly = textBox16.IsReadOnly = textBox23.IsReadOnly = textBox12.IsReadOnly = true;
                comboBox11.IsEnabled = comboBox12.IsEnabled = false;
                datePicker2.IsEnabled = false;
                button24.Visibility = button26.Visibility = label72.Visibility = label73.Visibility = label74.Visibility = textBox20.Visibility = textBox21.Visibility = Visibility.Hidden;
            }
        }
        private void label70_Click(object sender, RoutedEventArgs e)
        {
            FileInfo imgFile = new FileInfo("temp.jpeg");
            System.Diagnostics.Process.Start(imgFile.ToString());
        }

        private void Description_CloseTab(object sender, RoutedEventArgs e)
        {
            tabControl1.Items.Remove(description);
            desctriptionCount--;
        }

        private void textBox11_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            ProductsOperations operations = new ProductsOperations();
            List<Design> design = operations.getDesignByID(textBox11.Text);
            if (design.Count == 1)
            {
                try
                {
                    label52.Visibility = Visibility.Hidden;
                    label66.Content = design[0].DesignName;
                    comboBox11.Text = design[0].DesignPaperType;
                    String size = design[0].DesignSize;
                    size.Trim();
                    String[] a = size.Split('X');
                    textBox16.Text = a[0];
                    String[] b = a[1].Trim().Split(' ');
                    textBox23.Text = b[0];
                    if (b[1].StartsWith("C") || b[1].StartsWith("c"))
                        comboBox12.SelectedIndex = 0;
                    else if (b[1].StartsWith("I") || b[1].StartsWith("i"))
                        comboBox12.SelectedIndex = 1;
                    else if (b[1].StartsWith("F") || b[1].StartsWith("f"))
                        comboBox12.SelectedIndex = 2;
                    else if (b[1].StartsWith("M") || b[1].StartsWith("m"))
                        comboBox12.SelectedIndex = 3;
                    else
                        comboBox4.SelectedIndex = 1;
                }
                catch (Exception ex)
                {
                    return;
                }
            }
            else
            {
                label52.Content = "Invalid Design Code";
                label52.Visibility = Visibility.Visible;
                return;
            }
        }

        private void textBox11_TextChanged(object sender, TextChangedEventArgs e)
        {
            button24.IsEnabled = true;
        }

        private void textBox16_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex regex = new Regex("^[0-9.]{0,4}$");
            if (!regex.IsMatch(textBox16.Text))
            {
                label52.Content = "Invalid Size Entered";
                label52.Visibility = Visibility.Visible;
            }
            else
                label52.Visibility = Visibility.Hidden;
        }

        private void textBox23_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex regex = new Regex("^[0-9.]{0,4}$");
            if (!regex.IsMatch(textBox23.Text))
            {
                label52.Content = "Invalid Size Entered";
                label52.Visibility = Visibility.Visible;
            }
            else
                label52.Visibility = Visibility.Hidden;
        }

        private void button26_Click(object sender, RoutedEventArgs e)
        {
            FileDialog file = new OpenFileDialog();
            file.Filter = "jpeg files (*.jpeg)|*.jpg|jpg files (*.jpg)|*.jpg|Corel files (*.cdr)|*.cdr|Photoshop files (*.psd)|*.psd";
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                label170.Content = file.FileName;
            }
        }

        private void textBox12_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Regex regex = new Regex("^[0-9]{1,10}$");
            if (!regex.IsMatch(textBox12.Text))
            {
                label52.Content = "Invalid Quantity Entered";
                label52.Visibility = Visibility.Visible;
            }
            else
            {
                label74.Content = "Total Cost : " + (int.Parse(textBox12.Text) * int.Parse(textBox20.Text)).ToString();
                label52.Visibility = Visibility.Hidden;
            }
        }

        private void textBox20_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Regex regex = new Regex("^[0-9]{1,10}$");
            if (!regex.IsMatch(textBox20.Text))
            {
                label52.Content = "Invalid Quantity Entered";
                label52.Visibility = Visibility.Visible;
            }
            else
            {
                label74.Content = "Total Cost : " + (int.Parse(textBox12.Text) * int.Parse(textBox20.Text)).ToString();
                label52.Visibility = Visibility.Hidden;
            }
        }

        private void textBox21_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Regex regex = new Regex("^[0-9]{1,10}$");
            if (!regex.IsMatch(textBox20.Text))
            {
                label52.Content = "Invalid Advance Payment";
                label52.Visibility = Visibility.Visible;
            }
            else
            {
                label52.Visibility = Visibility.Hidden;
            }
        }

        private void button24_Click(object sender, RoutedEventArgs e)
        {
            if (label52.Visibility == Visibility.Hidden)
            {
                Order order = new Order();
                order.OrderID = int.Parse(label58.Content.ToString());
                order.Customer = new Customer();
                order.Customer.Name = label60.Content.ToString();
                order.DesignID = textBox11.Text;
                order.PaperType = comboBox11.Text;
                order.Size = textBox16.Text + " X " + textBox23.Text + " " + comboBox12.Text;
                order.OrderStatus = comboBox13.Text;
                order.Quantity = int.Parse(textBox12.Text);
                order.FinalizedDesign = label170.Content.ToString();
                order.DeliveryDate = (DateTime)datePicker2.SelectedDate;
                order.UnitPrice = int.Parse(textBox20.Text);
                order.AdvancePayment = int.Parse(textBox21.Text);
                OrderOperations operations = new OrderOperations();
                if (operations.editOrder(order))
                {
                    if (comboBox13.Text == "Delivered")
                    {
                        generateInvoiceReceipt(int.Parse(label58.Content.ToString()));
                    }
                    else
                        generateAdvanceReceipt(order);
                    Description_CloseTab(sender, e);
                }
                else
                    System.Windows.MessageBox.Show("Could Not Update Order Details");
            }
            else
                System.Windows.MessageBox.Show("Some Invalid Details Entered");
        }

        #endregion

        #region AddUser

        private void AddUserslabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Add users in expander
            if (AddUsersCount == 0)
            {
                tabControl1.Items.Add(AddUsers);
                AddUsers.Focus();
                AddUsersCount++;
                DatabaseOperations operations = new DatabaseOperations();
                comboBox1.DataContext = operations.getTableData("Designation", "DesignationName");
            }
            else
            {
                AddUsers.Focus();
            }
        }

        private void AddUsers_CloseTab(object sender, RoutedEventArgs e)
        {
            tabControl1.Items.Remove(AddUsers);
            AddUsersCount--;
            label52.Visibility = Visibility.Hidden;
        }

        private void textBox1_LostFocus(object sender, RoutedEventArgs e)
        {
            Regex regex = new Regex("[A-Za-z. ]$");
            if (!regex.IsMatch(textBox1.Text))
            {
                label52.Content = "Invalid Name Entered";
                label52.Visibility = Visibility.Visible;
            }
            else
                label52.Visibility = Visibility.Hidden;
        }

        private void textBox13_LostFocus(object sender, RoutedEventArgs e)
        {
            Regex regex = new Regex("[A-Za-z.]$");
            if (!regex.IsMatch(textBox13.Text))
            {
                label52.Content = "Invalid User Name Entered";
                label52.Visibility = Visibility.Visible;
            }
            else
                label52.Visibility = Visibility.Hidden;
        }

        private void password_LostFocus(object sender, RoutedEventArgs e)
        {
            if (password.Password == "")
            {
                label52.Content = "Enter password for new user";
                label52.Visibility = Visibility.Visible;
            }
            else
                label52.Visibility = Visibility.Hidden;
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Text = "";
            comboBox1.SelectedIndex = -1;
            textBox13.Text = "";
            password.Password = "";
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                label52.Content = "Select Designation";
                label52.Visibility = Visibility.Visible;
                return;
            }
            else
                label52.Visibility = Visibility.Hidden;

            if (label52.Visibility == Visibility.Hidden)
            {
                UserOperations operations = new UserOperations();
                if (operations.addUser(textBox1.Text, textBox13.Text, password.Password, comboBox1.Text))
                {
                    textBox1.Text = textBox13.Text = password.Password = "";
                    comboBox1.SelectedIndex = -1;
                    System.Windows.MessageBox.Show("User Added Successfully");
                }
                else
                    System.Windows.MessageBox.Show("System could not enter user");
            }
        }

        #endregion

        #region ResetPassword

        private void ResetPasswordlabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Reset Password in expander
            if (ResetPasswordCount == 0)
            {
                tabControl1.Items.Add(ResetPassword);
                ResetPassword.Focus();
                ResetPasswordCount++;
                DatabaseOperations operations = new DatabaseOperations();
                DataTable users = operations.getTableData("UserDetails","UserName");
                foreach(DataRow dr in users.Rows)
                {
                    if (dr[0].ToString() == emp.EmployeeName)
                    {
                        users.Rows.Remove(dr);
                        break;
                    }
                }
                comboBox2.DataContext = users;
            }
            else
            {
                
                ResetPassword.Focus();
            }
        }

        private void ResetPassword_CloseTab(object sender, RoutedEventArgs e)
        {
            tabControl1.Items.Remove(ResetPassword);
            ResetPasswordCount--;
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            UserOperations operations = new UserOperations();
            if (operations.resetPassword(comboBox2.Text))
            {
                comboBox2.SelectedIndex = -1;
                System.Windows.MessageBox.Show("Password for User " + comboBox2.Text + "resetted successfully");
            }
            else
                System.Windows.MessageBox.Show("System could not reset password");
        }
        #endregion

        #region DeleteUser

        private void DeleteUserlabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Delete User in expander
            if (DeleteUsersCount == 0)
            {
                tabControl1.Items.Add(DeleteUsers);
                DeleteUsers.Focus();
                DeleteUsersCount++;
                DatabaseOperations operations = new DatabaseOperations();
                DataTable users = operations.getTableData("UserDetails", "UserName");
                foreach (DataRow dr in users.Rows)
                {
                    if (dr[0].ToString() == emp.EmployeeName)
                    {
                        users.Rows.Remove(dr);
                        break;
                    }
                }
                comboBox6.DataContext = users;
            }
            else
            {
                DeleteUsers.Focus();
            }
        }

        private void DeleteUsers_CloseTab(object sender, RoutedEventArgs e)
        {
            tabControl1.Items.Remove(DeleteUsers);
            DeleteUsersCount--;
        }

        private void button8_Click(object sender, RoutedEventArgs e)
        {
            UserOperations operations = new UserOperations();
            if (operations.deleteUser(comboBox6.Text))
            {
                comboBox6.SelectedIndex = -1;
                System.Windows.MessageBox.Show("User " + comboBox6.Text + " deleted successfully");
                DeleteUsers_CloseTab(sender, e);
                DeleteUserlabel_MouseDown(sender, null);
                if(ResetPasswordCount == 1)
                {
                    ResetPassword_CloseTab(sender, e);
                    ResetPasswordlabel_MouseDown(sender, null);
                }
            }
            else
                System.Windows.MessageBox.Show("System could not delete password");
        }
        #endregion

        #region Monthly Report

        private void Report1label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Report 1 in expander
            if (Report1Count == 0)
            {
                tabControl1.Items.Add(Report1);
                Report1.Focus();
                Report1Count++;
                documentViewer.ViewerCore.ZoomFactor = 87;
            }
            else
            {                
                Report1.Focus();
            }
        }

        private void Report1_CloseTab(object sender, RoutedEventArgs e)
        {
            tabControl1.Items.Remove(Report1);
            Report1Count--;
            selectYrComboR.SelectedIndex = -1;
            selectMonComboR.SelectedIndex = -1;
            label36.Visibility = Visibility.Hidden;
        }


        private void selectMonComboR_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (selectYrComboR.SelectedIndex == -1)
            {
                label36.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                label36.Visibility = Visibility.Hidden;
            }
            int month = (selectMonComboR.SelectedIndex + 1);
            int year = int.Parse(selectYrComboR.Text.ToString());
            string smonth;
            if (month > 0 && month < 10)
                smonth = 0 + month.ToString();
            else
                smonth = year.ToString();
            DatabaseOperations operations = new DatabaseOperations();
            DataTable dt = operations.executeSelectQuery("SELECT Product.ProductName, SUM(OrderDetails.Quantity) AS TOTAL_QUANTITY, SUM(OrderDetails.Quantity*OrderDetails.PerProductCost) AS TOTAL_COST FROM OrderDetails,Design,Product WHERE Year(OrderDetails.DeliveryDate)=" + year + " AND Month(OrderDetails.DeliveryDate)=" + smonth + " AND OrderDetails.DesignID = Design.DesignID AND Design.ProductID =Product.ProductID GROUP BY Product.ProductName");
            ReportDocument document = generateSalesReport(dt);
            if (document != null)
            {
                documentViewer.ViewerCore.ReportSource = document;
            }

        }

        public ReportDocument generateSalesReport(DataTable dt)
        {
            ReportDocument salesReport = new ReportDocument();
            FileInfo file = new FileInfo("Reports/Sales/SalesReport.rpt");
            if (file.Exists)
            {
                salesReport.Load(file.FullName);

                ParameterFieldDefinitions crParameterFieldDefinitions1;
                crParameterFieldDefinitions1 = salesReport.DataDefinition.ParameterFields;
                ParameterFieldDefinition crParameterFieldDefinition1 = crParameterFieldDefinitions1["product"];
                ParameterValues crParameterValues1 = new ParameterValues();
                ParameterDiscreteValue crParameterDiscreteValue1 = new ParameterDiscreteValue();

                ParameterFieldDefinitions crParameterFieldDefinitions2;
                crParameterFieldDefinitions2 = salesReport.DataDefinition.ParameterFields;
                ParameterFieldDefinition crParameterFieldDefinition2 = crParameterFieldDefinitions2["quantity"];
                ParameterValues crParameterValues2 = new ParameterValues();
                ParameterDiscreteValue crParameterDiscreteValue2 = new ParameterDiscreteValue();

                ParameterFieldDefinitions crParameterFieldDefinitions3;
                crParameterFieldDefinitions3 = salesReport.DataDefinition.ParameterFields;
                ParameterFieldDefinition crParameterFieldDefinition3 = crParameterFieldDefinitions3["cost"];
                ParameterValues crParameterValues3 = new ParameterValues();
                ParameterDiscreteValue crParameterDiscreteValue3 = new ParameterDiscreteValue();

                string name = "";
                string quantity = "";
                string cost = "";
                foreach (DataRow dr in dt.Rows)
                {
                    name = name + dr[0].ToString() + "\n";
                    quantity = quantity + dr[1].ToString() + "\n";
                    cost = cost + dr[2].ToString() + "\n";
                }

                crParameterDiscreteValue1.Value = name;
                crParameterValues1 = crParameterFieldDefinition1.CurrentValues;
                crParameterValues1.Add(crParameterDiscreteValue1);
                crParameterFieldDefinition1.ApplyCurrentValues(crParameterValues1);

                crParameterDiscreteValue2.Value = quantity;
                crParameterValues2 = crParameterFieldDefinition2.CurrentValues;
                crParameterValues2.Add(crParameterDiscreteValue2);
                crParameterFieldDefinition2.ApplyCurrentValues(crParameterValues2);

                crParameterDiscreteValue3.Value = cost;
                crParameterValues3 = crParameterFieldDefinition3.CurrentValues;
                crParameterValues3.Add(crParameterDiscreteValue3);
                crParameterFieldDefinition3.ApplyCurrentValues(crParameterValues3);
            }
            else
            {
                salesReport = null;
            }
            return salesReport;
        }

        #endregion

        #region Yearly Report

        private void Report2label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Report 2 in expander
            if (Report2Count == 0)
            {
                tabControl1.Items.Add(Report2);
                Report2.Focus();
                Report2Count++;
                documentViewer.ViewerCore.ZoomFactor = 87;
            }
            else
            {
                Report2.Focus();
            }
        }

        private void Report2_CloseTab(object sender, RoutedEventArgs e)
        {
            tabControl1.Items.Remove(Report2);
            Report2Count--;
            selectYrComboR2.SelectedIndex = -1;
        }


        private void selectYrComboR2_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (selectYrComboR2.SelectedIndex != -1)
            {
                int year = int.Parse(selectYrComboR2.Text.ToString());
                DatabaseOperations operations = new DatabaseOperations();
                DataTable dt = operations.executeSelectQuery("SELECT Product.ProductName, SUM(OrderDetails.Quantity) AS TOTAL_QUANTITY, SUM(OrderDetails.Quantity*OrderDetails.PerProductCost) AS TOTAL_COST FROM OrderDetails,Design,Product WHERE Year(OrderDetails.DeliveryDate)=" + year + " AND OrderDetails.DesignID = Design.DesignID AND Design.ProductID =Product.ProductID GROUP BY Product.ProductName");
                ReportDocument document = generateSalesReport(dt);
                if (document != null)
                {
                    documentViewerY.ViewerCore.ReportSource = document;
                }
            }
        }

        #endregion

        #region BackupDB

        private void BackupDatabaselabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Backup Database in expander
            if (BackupDatabaseCount == 0)
            {
                tabControl1.Items.Add(BackupDatabase);
                BackupDatabase.Focus();
                BackupDatabaseCount++;
            }
            else
            {
                
                BackupDatabase.Focus();
            }
        }

        private void BackupDatabase_CloseTab(object sender, RoutedEventArgs e)
        {
            textBox17.Text = "";
            tabControl1.Items.Remove(BackupDatabase);
            BackupDatabaseCount--;
        }

        private void button9_Click(object sender, RoutedEventArgs e)
        {
            //Browse button for selecting database
            FolderBrowserDialog browse = new FolderBrowserDialog();
            browse.ShowNewFolderButton = true;
            browse.RootFolder = System.Environment.SpecialFolder.MyComputer;
            DialogResult result = browse.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                textBox17.Text = browse.SelectedPath;
            }
        }


        private void button12_Click(object sender, RoutedEventArgs e)
        {
            BackupDatabase_CloseTab(sender, e);
        }

        private void button11_Click(object sender, RoutedEventArgs e)
        {
            if (textBox17.Text != "")
            {
                DirectoryInfo destinationPath = new DirectoryInfo(textBox17.Text);
                if (!destinationPath.Exists)
                {
                    destinationPath.Create();
                }

                DatabaseOperations op = new DatabaseOperations();
                op.executeInsUpdDelQuery("BACKUP DATABASE [PressOperationsManagement] TO  DISK = N'C:\\Program Files (x86)\\Microsoft SQL Server\\MSSQL.1\\MSSQL\\Backup\\PressOperationsManagement.bak'WITH  DIFFERENTIAL , NOFORMAT, NOINIT,  NAME = N'PressOperationsManagement-Differential Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10");
                if(File.Exists(destinationPath+"\\PressOperationsManagement.bak"))
                    File.Delete(destinationPath+"\\PressOperationsManagement.bak");
                File.Move("C:\\Program Files (x86)\\Microsoft SQL Server\\MSSQL.1\\MSSQL\\Backup\\PressOperationsManagement.bak",destinationPath+"\\PressOperationsManagement.bak");
                textBox17.Text = "";
                System.Windows.MessageBox.Show("Backup Taken Successfully");
            }
            else
                System.Windows.MessageBox.Show("Path to save database not selected");
        }

        #endregion

        #region RestoreDB

        private void RestoreDatabaselabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Restore Database in expander
            if (RestoreDatabaseCount == 0)
            {
                tabControl1.Items.Add(RestoreDatabase);
                RestoreDatabase.Focus();
                RestoreDatabaseCount++;
            }
            else
            {
                
                RestoreDatabase.Focus();
            }
        }

        private void RestoreDatabase_CloseTab(object sender, RoutedEventArgs e)
        {
            textBox18.Text = "";
            tabControl1.Items.Remove(RestoreDatabase);
            RestoreDatabaseCount--;
        }

        private void button13_Click(object sender, RoutedEventArgs e)
        {
            //Browse button for restoring database
            FileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox18.Text = file.FileName;
            }
        }


        private void button15_Click(object sender, RoutedEventArgs e)
        {
            RestoreDatabase_CloseTab(sender, e);
        }


        private void button14_Click(object sender, RoutedEventArgs e)
        {
            if (textBox18.Text == "")
                System.Windows.MessageBox.Show("Path to restore database not selected");
            else
            {
                if (textBox18.Text.EndsWith(".bak") && new FileInfo(textBox18.Text).Exists)
                {
                    DatabaseOperations op = new DatabaseOperations();
                    op.setDatabaseOffline("ALTER DATABASE PressOperationsManagement SET OFFLINE");
                    op.setDatabaseOnline("ALTER DATABASE PressOperationsManagement SET ONLINE");
                    op.executeDatabaseRestore("RESTORE DATABASE [PressOperationsManagement] FROM  DISK = N'" + textBox18.Text + "' WITH  FILE = 1,  NOUNLOAD,  STATS = 10");
                    textBox18.Text = "";
                    System.Windows.MessageBox.Show("Database Restored Successfully");
                }
                else
                {
                    System.Windows.MessageBox.Show("Invalid File Selected");
                }
            }
        }
        #endregion

        #region NewCategory

        private void NewCategorylabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //New Category in expander
            if (NewCategoryCount == 0)
            {
                tabControl1.Items.Add(NewCategory);
                NewCategory.Focus();
                NewCategoryCount++;
            }
            else
            {
                
                NewCategory.Focus();
            }
        }

        private void NewCategory_CloseTab(object sender, RoutedEventArgs e)
        {
            tabControl1.Items.Remove(NewCategory);
            NewCategoryCount--;
            label52.Visibility = Visibility.Hidden;
            textBox19.Text = "";
        }


        private void button16_Click(object sender, RoutedEventArgs e)
        {
            if (textBox19.Text == "")
            {
                label52.Content="Please Enter Category Name";
                label52.Visibility = Visibility.Visible;
                return;
            }
            else
                label52.Visibility = Visibility.Hidden;

            ProductsOperations operations = new ProductsOperations();
            if (operations.addCategory(textBox19.Text))
            {
                textBox19.Text = "";
                System.Windows.MessageBox.Show("New Category Successfully Added");
            }
            else
                System.Windows.MessageBox.Show("System Unable to add new category");
        }

        private void button17_Click(object sender, RoutedEventArgs e)
        {
            NewCategory_CloseTab(sender, e);
            label52.Visibility = Visibility.Hidden;
        }

        #endregion

        #region NewProduct

        private void NewProductlabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //New Product in expander
            if (NewProductCount == 0)
            {
                tabControl1.Items.Add(NewProduct);
                NewProduct.Focus();
                NewProductCount++;

                ProductsOperations operations = new ProductsOperations();
                comboBox5.DataContext = operations.getAllCategories();
            }
            else
            {
                NewProduct.Focus();
            }
        }

        private void NewProduct_CloseTab(object sender, RoutedEventArgs e)
        {
            tabControl1.Items.Remove(NewProduct);
            NewProductCount--;
            label52.Visibility = Visibility.Hidden;
            textBox25.Text = "";
        }


        private void button19_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox5.SelectedIndex == -1)
            {
                label52.Content = "Please Select Category";
                label52.Visibility = Visibility.Visible;
                return;
            }
            else
                label52.Visibility = Visibility.Hidden;

            if (textBox25.Text == "")
            {
                label52.Content = "Please Enter Product Name";
                label52.Visibility = Visibility.Visible;
                return;
            }
            else
                label52.Visibility = Visibility.Hidden;

            ProductsOperations operations = new ProductsOperations();
            if (operations.addProduct(comboBox5.Text, textBox25.Text))
            {
                textBox25.Text = "";
                System.Windows.MessageBox.Show("New Product Successfully Added");
            }
            else
                System.Windows.MessageBox.Show("System Unable to add new product","Error",MessageBoxButton.OK,MessageBoxImage.Error);
        }

        private void label82_MouseUp(object sender, MouseButtonEventArgs e)
        {
            NewCategorylabel_MouseDown(sender, e);
        }

        #endregion

        #region NewDesign

        private void NewDesignlabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //New Design in expander
            if (NewDesignCount == 0)
            {
                tabControl1.Items.Add(NewDesign);
                NewDesign.Focus();
                NewDesignCount++;

                cat.Fill(nds.Category);
                pro.Fill(nds.Product);

                NewDesignGrid.DataContext = nds.Category;
            }
            else
            {
                NewDesign.Focus();
            }
        }

        private void NewDesign_CloseTab(object sender, RoutedEventArgs e)
        {
            tabControl1.Items.Remove(NewDesign);
            NewDesignCount--;
            label52.Visibility = Visibility.Hidden;
            grid3.DataContext = null;
            grid3.Visibility = Visibility.Hidden;
            addDesignButton.Visibility = Visibility.Hidden;
            savebutton.Visibility = Visibility.Hidden;
        }

        List<Design> listofDesign = new List<Design>();
        private void addDesignButton_Click(object sender, RoutedEventArgs e)
        {
            listofDesign.Clear();
            grid3.DataContext = null;

            String[] designFiles = null;
            OpenFileDialog file = new OpenFileDialog();
            file.Multiselect = true;
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                designFiles = file.FileNames;
            }
            if (designFiles != null)
            {
                foreach (string design in designFiles)
                {
                    listofDesign.Add(new Design(design, comboBox9.Text));
                }
                grid3.DataContext = listofDesign;
                savebutton.Visibility = Visibility.Visible;
                grid3.Visibility = Visibility.Visible;
            }
        }

        private void savebutton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Design s in ListViewDesign.Items)
            {
                s.DesignID = s.DesignFilePath.Substring(s.DesignFilePath.LastIndexOf('\\')).Split('.')[0].Substring(1);
                ProductsOperations operations = new ProductsOperations();
                bool saveOutput = operations.saveDesign(s);
                if (saveOutput)
                    continue;
                else
                {
                    System.Windows.Forms.MessageBox.Show("Designs Not successfully added!!!");
                    return;
                }
            }
            grid3.DataContext = null;
            grid3.Visibility = Visibility.Hidden;
            System.Windows.Forms.MessageBox.Show("All The Designs Successfully Added");
        }

        private void comboBox9_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            addDesignButton.Visibility = Visibility.Visible;
        }

        //Add Product Label
        private void label80_MouseUp(object sender, MouseButtonEventArgs e)
        {
            NewProductlabel_MouseDown(sender, e);
        }


        private void designsize_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            System.Windows.Controls.TextBox t = sender as System.Windows.Controls.TextBox;
            Regex regex = new Regex("[0-9]{1,3}[Xx][0-9]{1,3}[ ][CIFMcifm]");
            if (!regex.IsMatch(t.Text))
            {
                label52.Content = "Format for size : \"aXb Unit\"";
                label52.Visibility = Visibility.Visible;
                t.Focus();
                return;
            }
            else
                label52.Visibility = Visibility.Hidden;
        }

        #endregion

        #region ViewDesign

        private void ViewDesignlabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //View Design in expander
            if (ViewDesignCount == 0)
            {
                tabControl1.Items.Add(ViewDesign);
                ViewDesign.Focus();
                ViewDesignCount++;

                cat.Fill(nds.Category);
                pro.Fill(nds.Product);
                desig.Fill(nds.Design);
                pap.Fill(nds.PaperType);
                ViewDesignGrid.DataContext = nds.Category;
            }
            else
            {
                ViewDesign.Focus();
            }
        }

        private void ViewDesign_CloseTab(object sender, RoutedEventArgs e)
        {
            tabControl1.Items.Remove(ViewDesign);
            ViewDesignCount--;
            comboBox20.SelectedIndex = -1;
            grid4.DataContext = null;
            grid4.Visibility = Visibility.Hidden;
        }


        private void comboBox21_DropDownClosed(object sender, EventArgs e)
        {
            if (comboBox21.SelectedIndex != -1)
            {
                ProductsOperations operations = new ProductsOperations();
                List<Design> viewDesignList = operations.getAllDesigns(comboBox21.Text);
                grid4.DataContext = viewDesignList;
                grid4.Visibility = Visibility.Visible;
            }
        }

        private void image1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MemoryStream ms = new MemoryStream(((Design)viewDesignList.SelectedItem).DesignFile);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            img.Save("temp.jpeg");
            FileInfo imgFile = new FileInfo("temp.jpeg");
            System.Diagnostics.Process.Start(imgFile.ToString());
        }

        private void btnsave_Click(object sender, RoutedEventArgs e)
        {
            Design d = (Design)viewDesignList.SelectedItem;
            ProductsOperations operations = new ProductsOperations();
            if (d != null && operations.editDesign(d))
                System.Windows.Forms.MessageBox.Show("Design Info Edited Successfully");
            else
                System.Windows.Forms.MessageBox.Show("Could not edit design info");
        }

        private void button21_Click_2(object sender, RoutedEventArgs e)
        {
            grid4.DataContext = null;
            label51.Visibility = Visibility.Hidden;
            if (textBox6.Text == "")
            {
                label51.Content = "Enter Search Query";
                label51.Visibility = Visibility.Visible;
                return;
            }
            else
                label51.Visibility = Visibility.Hidden;

            ProductsOperations operations = new ProductsOperations();
            List<Design> viewDesignList;
            if (comboBox10.Text == "ID")
                viewDesignList = operations.getDesignByID(textBox6.Text);
            else
                viewDesignList = operations.getDesignByName(textBox6.Text);
            if (viewDesignList.Count == 0)
            {
                label51.Content = "No Results Found";
                label51.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                label51.Visibility = Visibility.Hidden;
                grid4.DataContext = viewDesignList;
                grid4.Visibility = Visibility.Visible;
            }
        }


        private void designsize_LostKeyboardFocus_1(object sender, KeyboardFocusChangedEventArgs e)
        {
            System.Windows.Controls.TextBox t = sender as System.Windows.Controls.TextBox;
            Regex regex = new Regex("[0-9]{1,3}[Xx][0-9]{1,3}[ ][CIFMcifm]");
            if (!regex.IsMatch(t.Text))
            {
                label52.Content = "Format for size : \"aXb Unit\"";
                label52.Visibility = Visibility.Visible;
                t.Focus();
                return;
            }
            else
                label52.Visibility = Visibility.Hidden;
        }
        #endregion

        #region RemainingPayment

        private void RemainingPaymentslabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Remaining Payments in expander
            if (RemainingPaymentCount == 0)
            {
                tabControl1.Items.Add(RemainingPayment);
                RemainingPayment.Focus();
                RemainingPaymentCount++;

                PaymentOperations operations = new PaymentOperations();
                DataTable remainingPaymentsTable = operations.remainingPayments();
                int totalRemainingPayment = 0;
                foreach(DataRow dr in remainingPaymentsTable.Rows)
                {
                    totalRemainingPayment += int.Parse(dr["RemainingPayment"].ToString());
                }
                label123.Content = "Total Remaning Payment : "+totalRemainingPayment.ToString();
                gridremainingpayments.DataContext = remainingPaymentsTable;
            }
            else
            {
                RemainingPayment.Focus();
            }
            
        }

        private void RemainingPayment_CloseTab(object sender, RoutedEventArgs e)
        {
            tabControl1.Items.Remove(RemainingPayment);
            RemainingPaymentCount--;
            gridremainingpayments.DataContext = null;
        }

        #endregion

        #region ApplyPayment

        private void ApplyPaymentlabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Apply Payments in expander
            if (ApplyPaymentCount == 0)
            {
                tabControl1.Items.Add(ApplyPayment);
                ApplyPayment.Focus();
                ApplyPaymentCount++;
                comboBox7.DataContext = null;
                label39.Content = "";
                label41.Content = "";
                label43.Content = "";
                label14.Content = "";
                label45.Content = "";
                comboBox7.SelectedIndex = -1;
                textBox28.Text = "";
                textBox29.Text = "";
            }
            else
            {
                ApplyPayment.Focus();
            }
        }

        private void ApplyPayment_CloseTab(object sender, RoutedEventArgs e)
        {
            tabControl1.Items.Remove(ApplyPayment);
            ApplyPaymentCount--;
        }

        private void button23_Click(object sender, RoutedEventArgs e)
        {
            if (textBox29.Text == "" && int.Parse(label45.Content.ToString()) != 0)
            {
                label50.Content = "Enter Amount Received";
                label50.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                label50.Visibility = Visibility.Hidden;
            }
            try
            {
                if (int.Parse(label45.Content.ToString()) == 0 && int.Parse(textBox29.Text) > 0)
                {
                    label50.Content = "No Remaining Payment To Be Applied";
                    label50.Visibility = Visibility.Visible;
                    return;
                }
                else
                {
                    label50.Visibility = Visibility.Hidden;
                }
                if ((int.Parse(label14.Content.ToString()) + int.Parse(textBox29.Text)) <= int.Parse(label43.Content.ToString()))
                {
                    label50.Visibility = Visibility.Hidden;
                    int amountReceived = int.Parse(textBox29.Text);
                    if (comboBox7.Text == "Delivered")
                    {
                        if ((int.Parse(label14.Content.ToString()) + int.Parse(textBox29.Text)) == int.Parse(label43.Content.ToString()))
                        {
                            generateInvoiceReceipt(int.Parse(textBox28.Text));
                        }
                        else
                        {
                            label50.Content = "Full payment not done for order to be delivered";
                            label50.Visibility = Visibility.Visible;
                            return;
                        }
                    }
                    PaymentOperations operations = new PaymentOperations();                      
                    if (operations.applyPayment(int.Parse(textBox28.Text), amountReceived))
                    {               
                        comboBox7.DataContext = null;
                        label39.Content = "";
                        label41.Content = "";
                        label43.Content = "";
                        label14.Content = "";
                        label45.Content = "";
                        comboBox7.SelectedIndex = -1;
                        textBox28.Text = "";
                        textBox29.Text = "";
                        System.Windows.MessageBox.Show("Amount Received Applied");
                        if (ExistingOrderCount == 1)
                        {
                            ExistingOrders_CloseTab(sender, e);
                            ExistingOrderslabel_MouseDown(sender, null);
                            label83_MouseUp(sender, null);
                            ApplyPayment.Focus();
                        }
                        if (RemainingPaymentCount == 1)
                        {
                            RemainingPayment_CloseTab(sender, e);
                            RemainingPaymentslabel_MouseDown(sender, null);
                            ApplyPayment.Focus();
                        }
                    }
                }
                else
                {
                    label50.Content = "Amount Received Exceeds Total Amount";
                    label50.Visibility = Visibility.Visible;
                    return;
                }
            }
            catch (FormatException ex)
            {
                label50.Content = "Invalid Amount Entered";
                label50.Visibility = Visibility.Visible;
                return;
            }
        }

        private void button22_Click(object sender, RoutedEventArgs e)
        {
            comboBox7.DataContext = null;
            label39.Content = "";
            label41.Content = "";
            label43.Content = "";
            label14.Content = "";
            label45.Content = "";
            comboBox7.SelectedIndex = -1;
            textBox29.Text = "";

            if (textBox28.Text == "")
            {
                label50.Content = "OrderID Not Entered";
                label50.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                label50.Visibility = Visibility.Hidden;
            }
            try
            {
                label50.Visibility = Visibility.Hidden;
                OrderOperations oper = new OrderOperations();
                int orderID = int.Parse(textBox28.Text);
                if (!oper.orderExists(orderID))
                {
                    label50.Content = "Entered OrderID does not exists";
                    label50.Visibility = Visibility.Visible;
                    return;
                }
                else
                {
                    label50.Visibility = Visibility.Hidden;
                }
                if (oper.isOrderDelivered(orderID))
                {
                    label50.Content = "Order Already Delivered";
                    label50.Visibility = Visibility.Visible;
                    return;
                }
                else
                {
                    label50.Visibility = Visibility.Hidden;
                    ords.Fill(nds.OrderStatus);
                    PaymentOperations operations = new PaymentOperations();
                    DataTable remainingPayment = operations.remainingPayments(orderID);
                    if (remainingPayment.Rows.Count == 1)
                    {
                        comboBox7.DataContext = nds.OrderStatus;
                        label39.Content = remainingPayment.Rows[0][1].ToString();
                        String s = remainingPayment.Rows[0][3].ToString().Split(' ')[0];
                        label41.Content = s.Split('/')[1] + "-"+ s.Split('/')[0] + "-"+ s.Split('/')[2];
                        label43.Content = remainingPayment.Rows[0][6].ToString();
                        label14.Content = remainingPayment.Rows[0][4].ToString();
                        label45.Content = remainingPayment.Rows[0][5].ToString();
                        comboBox7.Text = remainingPayment.Rows[0][7].ToString();
                    }
                }
            }
            catch (FormatException ex)
            {
                label50.Content = "Invalid OrderID Entered";
                label50.Visibility = Visibility.Visible;
            }
        }

        public void generateInvoiceReceipt(int orderID)
        {
            OrderOperations operations = new OrderOperations();
            Order newOrder = operations.getOrderByID(orderID);

            ReportDocument advancePaymentReceipt = new ReportDocument();
            FileInfo file = new FileInfo("Reports/Invoice/FinalInvoice.rpt");
            if (file.Exists)
            {
                advancePaymentReceipt.Load(file.FullName);

                ParameterFieldDefinitions crParameterFieldDefinitions;
                ParameterFieldDefinition crParameterFieldDefinition;
                ParameterValues crParameterValues = new ParameterValues();
                ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();

                crParameterFieldDefinitions = advancePaymentReceipt.DataDefinition.ParameterFields;

                crParameterDiscreteValue.Value = newOrder.Customer.Name;
                crParameterFieldDefinition = crParameterFieldDefinitions["name"];
                crParameterValues = crParameterFieldDefinition.CurrentValues;
                crParameterValues.Clear();
                crParameterValues.Add(crParameterDiscreteValue);
                crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);


                crParameterDiscreteValue.Value = newOrder.Customer.Address;
                crParameterFieldDefinition = crParameterFieldDefinitions["address"];
                crParameterValues = crParameterFieldDefinition.CurrentValues;
                crParameterValues.Clear();
                crParameterValues.Add(crParameterDiscreteValue);
                crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);


                crParameterDiscreteValue.Value = newOrder.Customer.ContactNumber;
                crParameterFieldDefinition = crParameterFieldDefinitions["contactNo"];
                crParameterValues = crParameterFieldDefinition.CurrentValues;
                crParameterValues.Clear();
                crParameterValues.Add(crParameterDiscreteValue);
                crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

                crParameterDiscreteValue.Value = newOrder.OrderID;
                crParameterFieldDefinition = crParameterFieldDefinitions["orderID"];
                crParameterValues = crParameterFieldDefinition.CurrentValues;
                crParameterValues.Clear();
                crParameterValues.Add(crParameterDiscreteValue);
                crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

                crParameterDiscreteValue.Value = newOrder.DesignID;
                crParameterFieldDefinition = crParameterFieldDefinitions["designID"];
                crParameterValues = crParameterFieldDefinition.CurrentValues;
                crParameterValues.Clear();
                crParameterValues.Add(crParameterDiscreteValue);
                crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);


                crParameterDiscreteValue.Value = newOrder.DesigName;
                crParameterFieldDefinition = crParameterFieldDefinitions["designName"];
                crParameterValues = crParameterFieldDefinition.CurrentValues;
                crParameterValues.Clear();
                crParameterValues.Add(crParameterDiscreteValue);
                crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

                crParameterDiscreteValue.Value = newOrder.PaperType;
                crParameterFieldDefinition = crParameterFieldDefinitions["paperType"];
                crParameterValues = crParameterFieldDefinition.CurrentValues;
                crParameterValues.Clear();
                crParameterValues.Add(crParameterDiscreteValue);
                crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

                crParameterDiscreteValue.Value = newOrder.Size;
                crParameterFieldDefinition = crParameterFieldDefinitions["size"];
                crParameterValues = crParameterFieldDefinition.CurrentValues;
                crParameterValues.Clear();
                crParameterValues.Add(crParameterDiscreteValue);
                crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

                crParameterDiscreteValue.Value = newOrder.Quantity;
                crParameterFieldDefinition = crParameterFieldDefinitions["quantity"];
                crParameterValues = crParameterFieldDefinition.CurrentValues;
                crParameterValues.Clear();
                crParameterValues.Add(crParameterDiscreteValue);
                crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

                crParameterDiscreteValue.Value = newOrder.UnitPrice;
                crParameterFieldDefinition = crParameterFieldDefinitions["unitPrice"];
                crParameterValues = crParameterFieldDefinition.CurrentValues;
                crParameterValues.Clear();
                crParameterValues.Add(crParameterDiscreteValue);
                crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

                crParameterDiscreteValue.Value = (newOrder.Quantity * newOrder.UnitPrice);
                crParameterFieldDefinition = crParameterFieldDefinitions["totalCost"];
                crParameterValues = crParameterFieldDefinition.CurrentValues;
                crParameterValues.Clear();
                crParameterValues.Add(crParameterDiscreteValue);
                crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

                crParameterDiscreteValue.Value = newOrder.DeliveryDate;
                crParameterFieldDefinition = crParameterFieldDefinitions["scheduledDate"];
                crParameterValues = crParameterFieldDefinition.CurrentValues;
                crParameterValues.Clear();
                crParameterValues.Add(crParameterDiscreteValue);
                crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

                crParameterDiscreteValue.Value = DateTime.Now; ;
                crParameterFieldDefinition = crParameterFieldDefinitions["deliveryDate"];
                crParameterValues = crParameterFieldDefinition.CurrentValues;
                crParameterValues.Clear();
                crParameterValues.Add(crParameterDiscreteValue);
                crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

                advancePaymentReceipt.ExportToDisk(ExportFormatType.WordForWindows, "i" + newOrder.OrderID + ".doc");
                if (File.Exists(Environment.CurrentDirectory + "\\Reports\\Invoice\\i" + newOrder.OrderID + ".doc"))
                    File.Delete(Environment.CurrentDirectory + "\\Reports\\Invoice\\i" + newOrder.OrderID + ".doc");
                File.Move("i"+newOrder.OrderID + ".doc", Environment.CurrentDirectory + "\\Reports\\Invoice\\i" + newOrder.OrderID + ".doc");
                //advancePaymentReceipt.PrintToPrinter(1, false, 1, 1);
                advancePaymentReceipt.Dispose();
            }
        }

        private void comboBox7_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox7.Text != "" && textBox28.Text != "")
            {
                OrderOperations operations = new OrderOperations();
                operations.setOrderStatus(int.Parse(textBox28.Text), comboBox7.Text);
            }
        }

        #endregion

        #region Logout
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //Logout
            Window4 login = new Window4();
            login.Show();
            this.Close();
        }
        #endregion

        #region ChangePassword

        //Change Passowrd
        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            ChangePassword change = new ChangePassword(userName, userPassword, emp);
            change.ShowDialog();
        }

        #endregion

        #region extraWidgets

        private void image1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (mediaElement1.LoadedBehavior == MediaState.Play)
            {
                mediaElement1.LoadedBehavior = MediaState.Pause;
            }
            else if (mediaElement1.LoadedBehavior == MediaState.Pause)
            {
                mediaElement1.LoadedBehavior = MediaState.Play;
            }
        }
        

        private void label23_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.facebook.com");
        }

        private void label27_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                mediaElement1.Source = new Uri(file.FileName);
            }
        }

        private void label33_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.gmail.com");
        }

        private void label34_MouseDown(object sender, MouseButtonEventArgs e)
        {
           System.Diagnostics.Process.Start("C:\\Users\\" + Environment.UserName +"\\AppData\\Local\\Google\\Chrome\\Application\\chrome.exe");
        }

        //Help button
        private void label81_MouseUp(object sender, MouseButtonEventArgs e)
        {
            FileInfo f = null;
            if (emp.Type == "admin")
            {
                f = new FileInfo("Help/admin_guide.docx");
            }
            if (emp.Type == "designer")
            {
                f = new FileInfo("Help/designer_guide.docx");
            }
            if (emp.Type == "manufacturingworker")
            {
                f = new FileInfo("Help/mworker_guide.docx");
            }
            if (emp.Type == "receptionist")
            {
                f = new FileInfo("Help/receptionist_guide.docx");
            }

            if (f != null && f.Exists)
            {
                System.Diagnostics.Process.Start("winword.exe", f.ToString());
            }
        }

        #endregion        

                                
    }
}