using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Data;
using System.Windows.Navigation;
using BusinessLogicLibrary;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for Window4.xaml
    /// </summary>
    public partial class Window4 : Window
    {
        public Window4()
        {
            InitializeComponent();
            this.ResizeMode = System.Windows.ResizeMode.CanMinimize;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            textBox1.Focus();
            label3.Visibility = Visibility.Hidden;
            button1_Click(sender, e);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            UserOperations operations = new UserOperations();
            LoginStatus status = operations.isValidUser(textBox1.Text,passwordBox1.Password);
            if (status.ValidUser)
            {
                label3.Visibility = Visibility.Hidden;
                if (status.FirstLogin)
                {
                    if (System.Windows.MessageBox.Show("You need to change your password.","Change Password",MessageBoxButton.OK,MessageBoxImage.Information) == MessageBoxResult.OK)
                    {
                        ChangePassword change = new ChangePassword(textBox1.Text, passwordBox1.Password, status.Employee);
                        change.Show();
                        this.Close();
                    }
                }
                else
                {
                    HomeWindow hm = new HomeWindow(textBox1.Text, passwordBox1.Password, status.Employee);
                    hm.Show();
                    this.Close();
                }
            }
            else
                label3.Visibility = Visibility.Visible;
            
        }
    }
}
