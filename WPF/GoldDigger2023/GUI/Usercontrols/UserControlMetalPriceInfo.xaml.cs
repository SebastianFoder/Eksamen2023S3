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
    /// Interaction logic for UserControlMetalPriceInfo.xaml
    /// </summary>
    public partial class UserControlMetalPriceInfo : UserControl
    {
        ClassBIZ BIZ;
        Grid homeGrid;
        //UserControlMetalPriceEdit UCEdit;
        public UserControlMetalPriceInfo(ClassBIZ inBIZ, Grid inhomeGrid)
        {
            InitializeComponent();
            BIZ = inBIZ;
            homeGrid = inhomeGrid;
        }

        // When the usercontrol is loaded, start an async method to refresh the webpage every whole minute
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            while (true)
            {
                // Every whole minute + 3 seconds
                var delay = 63 - DateTime.Now.Second;
                await Task.Delay(delay * 1000);
                APIWebBrowser.Refresh();
            }
        }
    }
}
