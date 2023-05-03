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
    /// Interaction logic for UserControlDataRelatingToEmployees.xaml
    /// </summary>
    public partial class UserControlDataRelatingToEmployees : UserControl
    {
        ClassBIZ BIZ;
        public UserControlDataRelatingToEmployees(ClassBIZ inBIZ)
        {
            InitializeComponent();
            BIZ = inBIZ;
        }
    }
}
