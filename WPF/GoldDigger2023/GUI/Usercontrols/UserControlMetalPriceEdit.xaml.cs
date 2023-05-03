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
    /// Interaction logic for UserControlMetalPriceEdit.xaml
    /// </summary>
    public partial class UserControlMetalPriceEdit : UserControl
    {
        ClassBIZ BIZ;
        Grid homeGrid;
        public UserControlMetalPriceEdit(ClassBIZ inBIZ, Grid inHomeGrid)
        {
            InitializeComponent();
            BIZ = inBIZ;
            homeGrid = inHomeGrid;
        }
    }
}
