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
using BusinessLogicLibrary;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Window
    {
        String userName;
        String password;
        Employee emp;
        public ChangePassword(String userName, String password, Employee emp)
        {
            this.emp = emp;
            this.userName = userName;
            this.password = password;
            InitializeComponent();
            this.ResizeMode = System.Windows.ResizeMode.CanMinimize;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            label4.Visibility = Visibility.Hidden;
            label5.Visibility = Visibility.Hidden;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //Submit
            if (oldPasswordBox.Password == password)
            {
                label4.Visibility = Visibility.Hidden;
                if (newPasswordBox.Password == confirmNewPassword.Password)
                {
                    label5.Visibility = Visibility.Hidden;
                    UserOperations operations = new UserOperations();
                    operations.changePassword(userName,oldPasswordBox.Password,newPasswordBox.Password);
                    HomeWindow hm = new HomeWindow(userName, password, emp);
                    hm.Show();
                    this.Close();
                }
                else
                {
                    label5.Visibility = Visibility.Visible;
                }
            }
            else
            {
                label4.Visibility = Visibility.Visible;
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
