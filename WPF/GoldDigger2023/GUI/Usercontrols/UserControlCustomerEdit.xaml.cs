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
    /// Interaction logic for UserControlCustomerEdit.xaml
    /// 
    /// Hej 
    /// Jeg spørger om tilladelse til at fra UCCustomerEdit at omdøbe 2 knapper Add New til Save og Update til Cancel
    /// 
    /// Det er OK 😊
    /// 
    /// Hej Jens. 
    /// Vi har nogle ændringer vi gerne vil have godkendt.
    /// Country Dictionary i ClassAddress
    /// titleId i Employees
    /// Combobox i Customer for country
    /// 
    /// Forklaring godtaget og accepteret – ændring er godkendt
    /// 
    /// </summary>
    public partial class UserControlCustomerEdit : UserControl
    {
        ClassBIZ BIZ;
        Grid homeGrid;

        public UserControlCustomerEdit(ClassBIZ inBIZ, Grid inHomeGrid)
        {
            InitializeComponent();
            BIZ = inBIZ;
            homeGrid = inHomeGrid;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (BIZ.AreAllFieldsFilled())
            {
                // If Customer doesn't exist, adds Customer to Database
                if (BIZ.editableCustomer.Id == 0)
                {
                    BIZ.SaveNewCustomer();
                }
                // If Customer exists, updates Customer data in Database
                else
                {
                    BIZ.UpdateCustomer();
                }

                homeGrid.Children.Remove(this);
                BIZ.customerIsEnabled = true;
            }
            else
            {
                // Message, Window title, Buttons available, Icon
                MessageBox.Show("Venligst udfyld alle felter.", "Manglende felter", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            homeGrid.Children.Remove(this);
            BIZ.customerIsEnabled = true;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("You are now gonna delete this Customer.\n\nClick Yes/Ja to delete.\nClick No/Nej to cancel.",
                "Delete Customer", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (MessageBox.Show("Are you sure you want to delete this Customer.\n\nClick Yes/Ja to delete.\nClick No/Nej to cancel.",
                "Delete Customer", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    BIZ.DeleteCustomer();
                }
                else
                {
                    homeGrid.Children.Remove(this);
                    BIZ.customerIsEnabled = true;
                }
            }
            else
            {
                homeGrid.Children.Remove(this);
                BIZ.customerIsEnabled = true;
            }

        }
    }
}
