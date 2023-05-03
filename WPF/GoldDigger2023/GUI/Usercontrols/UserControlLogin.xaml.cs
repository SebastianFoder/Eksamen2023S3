using BIZ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI.Usercontrols
{
    /// <summary>
    /// Interaction logic for UserControlLogin.xaml
    /// </summary>
    public partial class UserControlLogin : UserControl
    {
        ClassBIZ BIZ;
        Grid homeGrid;
        Grid priceGrid;

        public UserControlLogin(ClassBIZ inBIZ, Grid inhomeGrid, Grid inPriceGrid)
        {
            InitializeComponent();
            BIZ = inBIZ;
            homeGrid = inhomeGrid;
            priceGrid = inPriceGrid;
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if (BIZ.userLogin.Username.Length > 0 && BIZ.userLogin.Password.Length > 0)
            {
                if (BIZ.CheckUserLogin() != 0)
                {
                    homeGrid.Children.Remove(this);
                    BIZ.customerIsEnabled = true;
                    priceGrid.Children.Add(new UserControlMetalPriceInfo(BIZ, new Grid()));
                }
                else
                {
                    MessageBox.Show("Invalid Username Or Password", "ERROR!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Not all fields have been filled!", "ERROR!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        //{
        //    BIZ.userLogin.Password = PasswordBox.Password;
        //}

    }
}