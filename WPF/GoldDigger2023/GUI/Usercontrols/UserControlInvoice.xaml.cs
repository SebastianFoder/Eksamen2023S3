using BIZ;
using Repository;
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
    /// Interaction logic for UserControlInvoice.xaml
    /// </summary>
    public partial class UserControlInvoice : UserControl
    {
        ClassBIZ BIZ;
        public UserControlInvoice(ClassBIZ inBIZ)
        {
            InitializeComponent();
            BIZ = inBIZ;
        }

        private void ButtonAddMetalToOrder_Click(object sender, RoutedEventArgs e)
        {
            if (BIZ.fallbackOrderline.Quantity > 0D && BIZ.fallbackOrderline.Product != null)
            {
                BIZ.AddMetalToOrder();
            }
            else
            {
                MessageBox.Show("FIELDS NOT FULL", "ERROR!");
            }
        }

        private void RemoveOrderlineFromOrder_Click(object sender, RoutedEventArgs e)
        {
            if (BIZ.orderline != null && BIZ.orderline.Quantity > 0 && BIZ.invoice.OrderLines.Count > 0)
            {
                ClassOrderLine orderline = BIZ.invoice.OrderLines.Where(x => x.Quantity == BIZ.orderline.Quantity && x.Product.Id == BIZ.orderline.Product.Id).FirstOrDefault();
                BIZ.invoice.OrderLines.Remove(orderline);
                BIZ.UpdateOrderPrice();
            }
            else
            {
                MessageBox.Show("NO METAL IS SELECTED", "ERROR!");
            }
        }

        private void ButtonCompleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (BIZ.invoice.OrderLines.Count > 0 && BIZ.selectedCustomer.Id > 0)
            {
                BIZ.invoice.OrderCustomer = BIZ.selectedCustomer;
                BIZ.MakeOrder();
                BIZ.invoice = new ClassInvoice();
            }
            
        }
    }
}
