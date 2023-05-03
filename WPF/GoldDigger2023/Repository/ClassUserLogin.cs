using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    /// <summary>
    /// Tilladese til at lave denne Class
    /// 
    /// Hej
    ///
    /// Da det er et krav at kunne logge ind, er det nødvendigt at have en class som kan bruges til at kontrollere login data.
    /// Derfor spørges der om lov til at lave sådan en class
    ///
    /// Med Venlig Hilsen
    /// Ida Jessen Møller
    ///
    /// Hej Ida
    /// Det har gruppen min tilladelse til 😉
    /// 
    /// </summary>
    public class ClassUserLogin : ClassNotify
    {
        private string _Username;
        private string _Password;
        private ClassEmployee _Employee;

        public ClassUserLogin()
        {
            Username = string.Empty;
            Password = string.Empty;
        }

        public string Username
        {
            get { return _Username; }
            set
            {
                if (_Username != value)
                {
                    _Username = value;
                }
                Notify("Username");
            }
        }
        public string Password
        {
            get { return _Password; }
            set
            {
                if (_Password != value)
                {
                    _Password = value;
                }
                Notify("Password");
            }
        }
        public ClassEmployee Employee
        {
            get { return _Employee; }
            set
            {
                if (_Employee != value)
                {
                    _Employee = value;
                }
                Notify("Employee");
            }
        }
    }
}
