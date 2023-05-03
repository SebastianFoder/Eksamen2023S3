using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using IO;
using System.Collections.ObjectModel;

namespace BIZ
{
    public class ClassBIZ : ClassNotify
    {
        private ClassControlerDB controllerDB;
        private bool _customerIsEnabled;

        private ClassEmployee _selectedEmployee;
        private ClassEmployee _editableEmployee; //Maybe not used
        private List<ClassEmployee> _listEmployee;

        private ClassCustomer _selectedCustomer;
        private ClassCustomer _editableCustomer;
        private List<ClassCustomer> _listCustomer;

        private ClassOrderLine _orderline;
        private ClassOrderLine _fallbackOrderline;
        private ClassInvoice _invoice;
        private decimal _OrderPrice;

        private ClassProduct _selectedProduct; //Maybe not used
        private List<ClassProduct> _listProduct;

        private Dictionary<string, int> _listCountry;
        private ClassUserLogin _userLogin;

        public ClassBIZ()
        {
            customerIsEnabled = false;

            controllerDB = new ClassControlerDB();

            selectedCustomer = new ClassCustomer();
            editableCustomer = new ClassCustomer();
            listCustomer = new List<ClassCustomer>();

            orderline = new ClassOrderLine();
            invoice = new ClassInvoice();
            OrderPrice = 0m;

            selectedProduct = new ClassProduct();
            listProduct = new List<ClassProduct>();

            listCountry = new Dictionary<string, int>();
            userLogin = new ClassUserLogin();

            

            GetCustomersFromDB();
            GetEmployeesFromDB(); // Tilføj Salary til alle employees
            GetProductsFromDB();
            GetContriesFromDB();
        }

        public bool customerIsEnabled
        {
            get { return _customerIsEnabled; }
            set
            {
                if (_customerIsEnabled != value)
                {
                    _customerIsEnabled = value;
                }
                Notify("customerIsEnabled");
                Notify("orderIsEnabled");
            }
        }
        public bool orderIsEnabled {
            get
            {
                return selectedCustomer != null && customerIsEnabled && selectedCustomer.Id != 0;
            }
        }

        public ClassEmployee selectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                if (_selectedEmployee != value)
                {
                    _selectedEmployee = value;
                }
                Notify("selectedEmployee");
            }
        }
        public ClassEmployee editableEmployee
        {
            get { return _editableEmployee; }
            set
            {
                if (_editableEmployee != value)
                {
                    _editableEmployee = value;
                }
                Notify("editableEmployee");
            }
        }
        public List<ClassEmployee> listEmployee
        {
            get { return _listEmployee; }
            set
            {
                if (_listEmployee != value)
                {
                    _listEmployee = value;
                }
                Notify("listEmployee");
            }
        }
        public ClassCustomer selectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                if (_selectedCustomer != value)
                {
                    _selectedCustomer = value;
                }
                Notify("selectedCustomer");
                Notify("orderIsEnabled");
            }
        }
        public ClassCustomer editableCustomer
        {
            get { return _editableCustomer; }
            set
            {
                if (_editableCustomer != value)
                {
                    _editableCustomer = value;
                }
                Notify("editableCustomer");
            }
        }
        public List<ClassCustomer> listCustomer
        {
            get { return _listCustomer; }
            set
            {
                if (_listCustomer != value)
                {
                    _listCustomer = value;
                }
                Notify("listCustomer");
            }
        }
        public ClassOrderLine orderline
        {
            get { return _orderline; }
            set
            {
                if (_orderline != value)
                {
                    _orderline = value;
                    if (value != null)
                    {
                        fallbackOrderline = new ClassOrderLine(value);
                    }
                }
                Notify("orderline");
            }
        }
        public ClassOrderLine fallbackOrderline
        {
            get { return _fallbackOrderline; }
            set
            {
                if (_fallbackOrderline != value)
                {
                    _fallbackOrderline = value;
                }
                Notify("fallbackOrderline");
            }
        }
        public ClassInvoice invoice
        {
            get { return _invoice; }
            set
            {
                if (_invoice != value)
                {
                    _invoice = value;
                }
                Notify("invoice");
            }
        }
        public ClassProduct selectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                if (_selectedProduct != value)
                {
                    _selectedProduct = value;
                }
                Notify("selectedProduct");
            }
        }
        public List<ClassProduct> listProduct
        {
            get { return _listProduct; }
            set
            {
                if (_listProduct != value)
                {
                    _listProduct = value;
                }
                Notify("listProduct");
            }
        }
        public Dictionary<string, int> listCountry
        {
            get { return _listCountry; }
            set
            {
                if (_listCountry != value)
                {
                    _listCountry = value;
                }
                Notify("listCountry");
            }
        }
        public ClassUserLogin userLogin
        {
            get { return _userLogin; }
            set
            {
                if (_userLogin != value)
                {
                    _userLogin = value;
                }
                Notify("userLogin");
            }
        }
        public decimal OrderPrice
        {
            get { return _OrderPrice; }
            set
            {
                if (_OrderPrice != value)
                {
                    _OrderPrice = value;
                }
                Notify("OrderPrice");
            }
        }




        /// <summary>
        /// Gets all customers from the database and puts them in to listCustomer
        /// </summary>
        public void GetCustomersFromDB()
        {
            listCustomer = controllerDB.GetAllCustomersFromDB();
        }
        /// <summary>
        /// Gets all employees from the database and puts them in to listEmployee
        /// </summary>
        public void GetEmployeesFromDB()
        {
            listEmployee = controllerDB.GetAllEmployeesFromDB();
        }

        /// <summary>
        /// Takes all products in the database and puts it in the list listProduct
        /// </summary>
        public void GetProductsFromDB()
        {
            listProduct = controllerDB.GetAllProductsFromDB();
        }

        /// <summary>
        /// Takes all Contries from the database and gets the Id and Names and puts them into a dictionary
        /// </summary>
        public void GetContriesFromDB()
        {
            listCountry = controllerDB.GetAllContriesFromDB();
        }
        
        /// <summary>
        /// Saves Customer in Database
        /// </summary>
		public void SaveNewCustomer()
        {
            // Insert the customer in the database
            var id = controllerDB.InsertCustomerToDB(editableCustomer);

            listCustomer = new List<ClassCustomer>(controllerDB.GetAllCustomersFromDB());
            selectedCustomer = listCustomer.Where(x => x.Id == id).Single();
        }

        /// <summary>
        /// Updates Customer in Database with new data
        /// </summary>
		public void UpdateCustomer()
		{
            controllerDB.UpdateCustomer(editableCustomer);
            int id = editableCustomer.Id;

            listCustomer = new List<ClassCustomer>(controllerDB.GetAllCustomersFromDB());
            selectedCustomer = listCustomer.Where(x => x.Id == id).Single();
        }

        /// <summary>
        /// Deletes a customer in the database
        /// </summary>
        public void DeleteCustomer()
        {
            controllerDB.DeleteCustomer(selectedCustomer);
            listCustomer = new List<ClassCustomer>(controllerDB.GetAllCustomersFromDB());
            selectedCustomer = new ClassCustomer();
        }

        /// <summary>
        /// Checks to see if the login is valid
        /// And sets the employee to the right one if the login is valid
        /// </summary>
        /// <returns>The ID of the employee if it found any</returns>
        public int CheckUserLogin()
        {
            int res = controllerDB.CheckUserLogin(userLogin);
            if (res != 0)
            {
                userLogin.Employee = listEmployee.Where(x => x.Id == res).Single();
            }
            return res;
        }

        /// <summary>
        /// Check if all the fields are filled in the new customer
        /// </summary>
        /// <returns>If all the fields are filled</returns>
        public bool AreAllFieldsFilled()
        {
            if (editableCustomer.contactTitle.Length > 0)
            {
                if (editableCustomer.Address != new ClassAddress())
                {
                    if (editableCustomer.contactTitle.Length > 0)
                    {
                        if (editableCustomer.companyName.Length > 0)
                        {
                            if (editableCustomer.contactName.Length > 0)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Adds metal to the invoice
        /// </summary>
        public void AddMetalToOrder()
        {
            invoice.OrderLines.Add(fallbackOrderline);
            UpdateOrderPrice();
        }

        public void UpdateOrderPrice()
        {
            OrderPrice = invoice.GetPaymentAmount();
        }

        public void MakeOrder()
        {
            controllerDB.InsertInvoiceInDB(invoice, userLogin.Employee);
        }
    }
}