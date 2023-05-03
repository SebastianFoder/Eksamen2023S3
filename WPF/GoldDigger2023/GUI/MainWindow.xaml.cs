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
using BIZ;
using GUI.Usercontrols;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ClassBIZ BIZ = new ClassBIZ();

        UserControlLogin UClogin;
        UserControlCustomer UCcustomer;
        UserControlCustomerEdit UCcustomerEdit;
        UserControlInvoice UCinvoice;
        UserControlDataRelatingToEmployees UCdataRelatingToEmployees;
        UserControlMetalPriceInfo UCdataMetalPriceInfo;
        UserControlMetalPriceEdit UCdataMetalPriceEdit;

        public MainWindow()
        {
            InitializeComponent();
            MainGrid.DataContext = BIZ;

            UClogin = new UserControlLogin(BIZ, GridLogin, GridPrice);
            UCcustomer = new UserControlCustomer(BIZ, GridCustomer);
            UCcustomerEdit = new UserControlCustomerEdit(BIZ, GridCustomer);
            UCinvoice = new UserControlInvoice(BIZ);
            UCdataRelatingToEmployees = new UserControlDataRelatingToEmployees(BIZ);
            UCdataMetalPriceInfo = new UserControlMetalPriceInfo(BIZ, GridPrice);
            UCdataMetalPriceEdit = new UserControlMetalPriceEdit(BIZ, GridPrice);

            GridLogin.Children.Add(UClogin);
            GridCustomer.Children.Add(UCcustomer);
            GridOrder.Children.Add(UCinvoice);
            
            GridEmploye.Children.Add(UCdataRelatingToEmployees);
        }
    }
}
