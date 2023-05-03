using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Repository;

namespace IO
{
    public class ClassControlerDB : ClassDbCon
    {
        static string sqlConStr = @"Server=(localdb)\MSSQLLocalDB;DataBase=GoldDiggerDB;Trusted_Connection=True";

        public ClassControlerDB()
        {
            SetCon(sqlConStr);
        }


        public int InsertInvoiceInDB(ClassInvoice inInvoice, ClassEmployee inEmployee)
        {
            int res = 0;
            int id = 0;

            try
            {
                string sqlQuery = "INSERT INTO Invoice (CustomerId, Employe, InvoiceDate, InvoiceAmount) " +
                    "VALUES (@CustomerId, @Employe, @InvoiceDate, @InvoiceAmount) " +
                    "SELECT SCOPE_IDENTITY()";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = inInvoice.OrderCustomer.Id;
                    cmd.Parameters.Add("@Employe", SqlDbType.Int).Value = inEmployee.Id;
                    cmd.Parameters.Add("@InvoiceDate", SqlDbType.Date).Value = DateTime.Now;
                    cmd.Parameters.Add("@InvoiceAmount", SqlDbType.Money).Value = inInvoice.GetPaymentAmount();
                    id = ExecuteScalarInDB(cmd);
                }

                foreach (ClassOrderLine line in inInvoice.OrderLines)
                {
                    sqlQuery = "INSERT INTO InvoiceLines (Id, InvoiceId, ItemId, Quantity, PriceDKK, PriceEUR, PriceUSD) " +
                    "VALUES (@Id, @InvoiceId, @ItemId, @Quantity, @PriceDKK, @PriceEUR, @PriceUSD)";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        // id not set as a unique identifyer in db
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Convert.ToInt32(DateTime.Now.Millisecond * DateTime.Now.DayOfYear + (DateTime.Now.Year * 10 + id) * (line.Quantity / 10));
                        cmd.Parameters.Add("@InvoiceId", SqlDbType.Int).Value = id;
                        cmd.Parameters.Add("@ItemId", SqlDbType.Int).Value = line.Product.Id;
                        cmd.Parameters.Add("@Quantity", SqlDbType.Decimal).Value = line.Quantity;
                        cmd.Parameters.Add("@PriceDKK", SqlDbType.Money).Value = line.Product.DKKPrice;
                        cmd.Parameters.Add("@PriceEUR", SqlDbType.Money).Value = line.Product.EurPrice;
                        cmd.Parameters.Add("@PriceUSD", SqlDbType.Money).Value = line.Product.UsdPrice;
                        res += ExecuteNonQuery(cmd);
                    }
                }
                
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return res;
        }


        /// <summary>
        /// Gets the employees from database.
        /// </summary>
        /// <returns>All Employees</returns>
        public List<ClassEmployee> GetAllEmployeesFromDB()
        {
            List<ClassEmployee> res = new List<ClassEmployee>();

            try
            {
                string sqlQuery = "SELECT Employes.Id, Employes.Name, Employes.Adresse, Employes.Zip, Employes.City, Employes.PhoneNo, " +
                    "Employes.MailAdr, Countries.CountryName, EmployeeSalary.Salary, EmployeTitels.Titel, Countries.Id AS CountryId, EmployeTitels.Id AS TitleId " +
                    "FROM Employes INNER JOIN EmployeTitels ON Employes.TitleId = EmployeTitels.Id LEFT OUTER JOIN EmployeeSalary " +
                    "ON Employes.Id = EmployeeSalary.EmployeeId LEFT OUTER JOIN Countries ON Employes.CountryId = Countries.Id " +
                    "WHERE (Employes.Activ = 1)";

                using (DataTable dt = DBReturnDataTable(sqlQuery))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        BaseSalariedEmployee ce = new BaseSalariedEmployee(
                            Convert.ToInt32(row["Id"]),
                            row["Name"].ToString(),
                            new ClassAddress()
                            {
                                StreetName = row["Adresse"].ToString(),
                                ZipCity = $"{row["Zip"]} {row["City"]}",
                                Country = new KeyValuePair<string, int>(row["CountryName"].ToString(), Convert.ToInt32(row["CountryId"])),
                                Phone = row["PhoneNo"].ToString(),
                                MailAdr = row["MailAdr"].ToString()
                            },
                            Convert.ToDecimal(row["Salary"]),
                            row["Titel"].ToString(),
                            Convert.ToInt32(row["TitleId"])
                            );
                        res.Add(ce);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return res;
        }

        /// <summary>
        /// Inserts a base salaried employee into the database.
        /// </summary>
        /// <param name="emp">a full version of a base salaried employee</param>
        /// <returns>the new Id of the employee</returns>
        public int InsertBaseSalariedEmployeeToDB(BaseSalariedEmployee emp)
        {
            int res = 0;
            try
            {
                string sqlQuery = "INSERT INTO Employes (Name, Adresse, Zip, City, PhoneNo, MailAdr, Activ, CountryId, TitleId) " +
                    "VALUES (@Name, @Adresse, @Zip, @City, @PhoneNo, @MailAdr, @Activ, @CountryId, @TitleId) " +
                    "SELECT SCOPE_IDENTITY()";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = emp.name;
                    cmd.Parameters.Add("@Adresse", SqlDbType.NVarChar).Value = emp.Address.StreetName;
                    cmd.Parameters.Add("@Zip", SqlDbType.NVarChar).Value = emp.Address.ZipCity.Split(' ')[0];
                    string City = "";
                    for (int i = 0; i < emp.Address.ZipCity.Split(' ').Skip(1).ToArray().Length; i++)
                    {
                        if (i + 1 < emp.Address.ZipCity.Split(' ').Skip(1).ToArray().Length)
                        {
                            City += emp.Address.ZipCity.Split(' ').Skip(1).ToArray()[i] + " ";
                        }
                        else
                        {
                            City += emp.Address.ZipCity.Split(' ').Skip(1).ToArray()[i];
                        }
                    }
                    cmd.Parameters.Add("@City", SqlDbType.NVarChar).Value = City;
                    cmd.Parameters.Add("@PhoneNo", SqlDbType.NVarChar).Value = emp.Address.Phone;
                    cmd.Parameters.Add("@MailAdr", SqlDbType.NVarChar).Value = emp.Address.MailAdr;
                    cmd.Parameters.Add("@Activ", SqlDbType.Bit).Value = 1;
                    cmd.Parameters.Add("@CountryId", SqlDbType.Int).Value = emp.Address.Country.Value;
                    cmd.Parameters.Add("@TitleId", SqlDbType.Int).Value = emp.titleId;
                    res = ExecuteScalarInDB(cmd);
                }

                sqlQuery = "INSERT INTO EmployeeSalary (EmployeeId, Salary) " +
                    "VALUES (@EmployeeId, @Salary)";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = res;
                    cmd.Parameters.Add("@Salary", SqlDbType.Decimal).Value = emp.Earnings();
                    if (ExecuteNonQuery(cmd) < 0)
                    {
                        throw new Exception("Error in DB");
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return res;
        }

        /// <summary>
        /// Updates a base salried employee in the database.
        /// </summary>
        /// <param name="emp">base salaried employee to be updated</param>
        /// <returns>If the action was successfull</returns>
        public int UpdateBaseSalriedEmployee(BaseSalariedEmployee emp)
        {
            int res = 0;

            try
            {
                string sqlQuery = "UPDATE Employes " +
                    "SET " +
                    "Name = @Name, " +
                    "Adresse = @Adresse, " +
                    "Zip = @Zip, " +
                    "City = @City, " +
                    "PhoneNo = @PhoneNo, " +
                    "MailAdr = @MailAdr, " +
                    "Activ = 1, " +
                    "CountryId = @CountryId, " +
                    "TitleId = @TitleId " +
                    "WHERE Id = @Id " +
                    "UPDATE EmployeeSalary " +
                    "SET " +
                    "Salary = @Salary " +
                    "WHERE EmployeeId = @Id";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = emp.Id;
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = emp.name;
                    cmd.Parameters.Add("@Adresse", SqlDbType.NVarChar).Value = emp.Address.StreetName;
                    cmd.Parameters.Add("@Zip", SqlDbType.NVarChar).Value = emp.Address.ZipCity.Split(' ')[0];
                    string City = "";
                    for (int i = 0; i < emp.Address.ZipCity.Split(' ').Skip(1).ToArray().Length; i++)
                    {
                        if (i + 1 < emp.Address.ZipCity.Split(' ').Skip(1).ToArray().Length)
                        {
                            City += emp.Address.ZipCity.Split(' ').Skip(1).ToArray()[i] + " ";
                        }
                        else
                        {
                            City += emp.Address.ZipCity.Split(' ').Skip(1).ToArray()[i];
                        }
                    }
                    cmd.Parameters.Add("@City", SqlDbType.NVarChar).Value = City;
                    cmd.Parameters.Add("@PhoneNo", SqlDbType.NVarChar).Value = emp.Address.Phone;
                    cmd.Parameters.Add("@MailAdr", SqlDbType.NVarChar).Value = emp.Address.MailAdr;
                    cmd.Parameters.Add("@Activ", SqlDbType.Bit).Value = 1;
                    cmd.Parameters.Add("@CountryId", SqlDbType.Int).Value = emp.Address.Country.Value;
                    cmd.Parameters.Add("@TitleId", SqlDbType.Int).Value = emp.titleId;
                    res = ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return res;
        }

        /// <summary>
        /// Deletes a base salaried employee from the database
        /// </summary>
        /// <param name="emp">employee to be removed</param>
        /// <returns>If the action was successfull</returns>
        public int DeleteBaseSalriedEmployee(BaseSalariedEmployee emp)
        {
            int res = 0;

            try
            {
                string sqlQuery = "DELETE FROM Employes " +
                    "WHERE Id = @Id " +
                    "DELETE FROM EmployeeSalary " +
                    "WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = emp.Id;
                    res = ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return res;
        }


        /// <summary>
        /// Gets the customers from database.
        /// </summary>
        /// <returns>All Customers</returns>
        public List<ClassCustomer> GetAllCustomersFromDB()
        {
            List<ClassCustomer> res = new List<ClassCustomer>();

            try
            {
                string sqlQuery = "SELECT Customers.Id, Customers.CompanyName, Customers.ContactName, Customers.ContactTitle, Customers.Adresse, " +
                    "Customers.Zip, Customers.City, Customers.CountryId, Customers.PhoneNo, Customers.MailAdr, Countries.CountryName " +
                    "FROM Countries INNER JOIN Customers ON Countries.Id = Customers.CountryId " +
                    "WHERE (Customers.Activ = 1)";

                using (DataTable dt = DBReturnDataTable(sqlQuery))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        ClassCustomer cc = new ClassCustomer(
                            Convert.ToInt32(row["Id"]),
                            // row["CompanyName"].ToString(),
                            new ClassAddress()
                            {
                                StreetName = row["Adresse"].ToString(),
                                ZipCity = $"{row["Zip"]} {row["City"]}",
                                Country = new KeyValuePair<string, int>(row["CountryName"].ToString(), Convert.ToInt32(row["CountryId"])),
                                Phone = row["PhoneNo"].ToString(),
                                MailAdr = row["MailAdr"].ToString()
                            },
                            row["ContactName"].ToString(),
                            row["ContactTitle"].ToString(),
                            row["CompanyName"].ToString()

                            );
                        res.Add(cc);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return (from customer in res
                   orderby customer.companyName ascending
                   select customer).ToList();
        }

        /// <summary>
        /// Inserts a customer into the database.
        /// </summary>
        /// <param name="ctm">Customer to be added</param>
        /// <returns>The new id of the customer</returns>
        public int InsertCustomerToDB(ClassCustomer ctm)
        {
            int res = 0;
            try
            {
                string sqlQuery = "INSERT INTO Customers (CompanyName, ContactName, ContactTitle, Adresse, Zip, City, CountryId, PhoneNo, MailAdr, Activ) " +
                    "VALUES (@CompanyName, @ContactName, @ContactTitle, @Adresse, @Zip, @City, @CountryId, @PhoneNo, @MailAdr, 1) " +
                    "SELECT SCOPE_IDENTITY()";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.Add("@CompanyName", SqlDbType.NVarChar).Value = ctm.companyName;
                    cmd.Parameters.Add("@ContactName", SqlDbType.NVarChar).Value = ctm.contactName;
                    cmd.Parameters.Add("@ContactTitle", SqlDbType.NVarChar).Value = ctm.contactTitle;
                    cmd.Parameters.Add("@Adresse", SqlDbType.NVarChar).Value = ctm.Address.StreetName;
                    cmd.Parameters.Add("@Zip", SqlDbType.NVarChar).Value = ctm.Address.ZipCity.Split(' ')[0];
                    string City = "";
                    for (int i = 0; i < ctm.Address.ZipCity.Split(' ').Skip(1).ToArray().Length; i++)
                    {
                        if (i + 1 < ctm.Address.ZipCity.Split(' ').Skip(1).ToArray().Length)
                        {
                            City += ctm.Address.ZipCity.Split(' ').Skip(1).ToArray()[i] + " ";
                        }
                        else
                        {
                            City += ctm.Address.ZipCity.Split(' ').Skip(1).ToArray()[i];
                        }
                    }
                    cmd.Parameters.Add("@City", SqlDbType.NVarChar).Value = City;
                    cmd.Parameters.Add("@PhoneNo", SqlDbType.NVarChar).Value = ctm.Address.Phone;
                    cmd.Parameters.Add("@MailAdr", SqlDbType.NVarChar).Value = ctm.Address.MailAdr;
                    cmd.Parameters.Add("@Activ", SqlDbType.Bit).Value = 1;
                    cmd.Parameters.Add("@CountryId", SqlDbType.Int).Value = ctm.Address.Country.Value;
                    res = ExecuteScalarInDB(cmd);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return res;
        }

        /// <summary>
        /// Updates a customer in the database.
        /// </summary>
        /// <param name="ctm">The Customer to be updated</param>
        /// <returns>If the action was successfull</returns>
        public int UpdateCustomer(ClassCustomer ctm)
        {
            int res = 0;

            try
            {
                string sqlQuery = "UPDATE Customers " +
                    "SET " +
                    "CompanyName = @CompanyName, " +
                    "ContactName = @ContactName, " +
                    "ContactTitle = @ContactTitle, " +
                    "Adresse = @Adresse, " +
                    "Zip = @Zip, " +
                    "City = @City, " +
                    "PhoneNo = @PhoneNo, " +
                    "MailAdr = @MailAdr, " +
                    "Activ = 1, " +
                    "CountryId = @CountryId " +
                    "WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = ctm.Id;
                    cmd.Parameters.Add("@CompanyName", SqlDbType.NVarChar).Value = ctm.companyName;
                    cmd.Parameters.Add("@ContactName", SqlDbType.NVarChar).Value = ctm.contactName;
                    cmd.Parameters.Add("@ContactTitle", SqlDbType.NVarChar).Value = ctm.contactTitle;
                    cmd.Parameters.Add("@Adresse", SqlDbType.NVarChar).Value = ctm.Address.StreetName;
                    cmd.Parameters.Add("@Zip", SqlDbType.NVarChar).Value = ctm.Address.ZipCity.Split(' ')[0];
                    string City = "";
                    for (int i = 0; i < ctm.Address.ZipCity.Split(' ').Skip(1).ToArray().Length; i++)
                    {
                        if (i + 1 < ctm.Address.ZipCity.Split(' ').Skip(1).ToArray().Length)
                        {
                            City += ctm.Address.ZipCity.Split(' ').Skip(1).ToArray()[i] + " ";
                        }
                        else
                        {
                            City += ctm.Address.ZipCity.Split(' ').Skip(1).ToArray()[i];
                        }
                    }
                    cmd.Parameters.Add("@City", SqlDbType.NVarChar).Value = City;
                    cmd.Parameters.Add("@PhoneNo", SqlDbType.NVarChar).Value = ctm.Address.Phone;
                    cmd.Parameters.Add("@MailAdr", SqlDbType.NVarChar).Value = ctm.Address.MailAdr;
                    cmd.Parameters.Add("@Activ", SqlDbType.Bit).Value = 1;
                    cmd.Parameters.Add("@CountryId", SqlDbType.Int).Value = ctm.Address.Country.Value;
                    res = ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return res;
        }

        /// <summary>
        /// Deletes a Customer from the Database.
        /// </summary>
        /// <param name="ctm">The Customer to be deleted</param>
        /// <returns>If the action was successfull</returns>
        public int DeleteCustomer(ClassCustomer ctm)
        {
            int res = 0;

            try
            {
                string sqlQuery = "DELETE FROM Customers " +
                    "WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = ctm.Id;
                    res = ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return res;
        }


        /// <summary>
        /// Gets all Contries from the database and gets the Id and Names and puts them into a dictionary
        /// </summary>
        /// <returns>Dictionary of Countries Key is the Name Value the Id</returns>
        public Dictionary<string, int> GetAllContriesFromDB()
        {

            Dictionary<string, int>  res = new Dictionary<string, int>();
            string sqlQuery = "SELECT Id, CountryName FROM Countries";

            try
            {
                using (DataTable dt = DBReturnDataTable(sqlQuery))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        res.Add(row["CountryName"].ToString(), Convert.ToInt32(row["Id"]));
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return res;
        }

        /// <summary>
        /// Gets all products from the database
        /// </summary>
        /// <returns></returns>
        public List<ClassProduct> GetAllProductsFromDB()
        {


            // Doesnt work due to IIS Express certificate error
            //ClassCallWebApi api = new ClassCallWebApi();
            //List<ClassApiResponds> apiResponds = api.GetURLContentsAsync().Result; 

            List<ClassProduct> res = new List<ClassProduct>();
            List<ClassApiResponds> apiResponds = MimicAPI();

            // Adds the stock to the products
            try
            {

                List<ClassStock> stocks = new List<ClassStock>();

                string sqlQuery = "SELECT MetalId, Warehouse, StockRack, Shelf, StockQuantity " +
                    "FROM Stock";

                using (DataTable dt = DBReturnDataTable(sqlQuery))
                {
                    foreach (var product in apiResponds)
                    {
                        ClassProduct cp = new ClassProduct(
                            product.id
                            );
                        cp.Name = product.metalName;
                        cp.DKKPrice = Convert.ToDouble(product.priceDkkPerKg);
                        cp.EurPrice = Convert.ToDouble(product.priceEurPerKg);
                        cp.UsdPrice = Convert.ToDouble(product.priceUsdPerKg);
                        DataRow row = dt.Rows[0];
                        foreach (DataRow checkRow in dt.Rows)
                        {
                            if (Convert.ToInt32(checkRow["MetalId"]) == product.id)
                            {
                                row = checkRow;
                                break;
                            }
                        }
                        ClassStock cs = new ClassStock();
                        cs.Warehouse = row["Warehouse"].ToString();
                        cs.StorageRack = row["StockRack"].ToString();
                        cs.Shelf = row["Shelf"].ToString();
                        cs.StockQuantity = Convert.ToDouble(row["StockQuantity"]);
                        cp.Stock = cs;
                        res.Add(cp);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return res;
        }

        /// <summary>
        /// Checks a userlogin and returns the employee ID if it found any
        /// </summary>
        /// <param name="inLogin">the login to be checked</param>
        /// <returns>The employee ID if it found any</returns>
        public int CheckUserLogin(ClassUserLogin inLogin)
        {

            int res = 0;

            try
            {
                List<ClassStock> stocks = new List<ClassStock>();

                string sqlQuery = "SELECT Username, Password, EmployeeID " +
                    "FROM Users " +
                    "WHERE Username = @Username AND Password = @Password";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.Add("@Username", SqlDbType.NVarChar).Value = inLogin.Username;
                    cmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = inLogin.Password;
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dtRes = new DataTable();
                        adapter.Fill(dtRes);
                        if (dtRes.Rows.Count > 0)
                        {
                            res = Convert.ToInt32(dtRes.Rows[0]["EmployeeID"]);
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }       
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return res;
        }

        /// <summary>
        /// Gets the exchangerate and calculates the price of the metals 
        /// </summary>
        /// <returns>List of all the metals in diffrent Valutas</returns>
        public List<ClassApiResponds> MimicAPI()
        {
            // makes containers
            List<ClassApiResponds> res = new List<ClassApiResponds>();
            Dictionary<string, decimal> rates = new Dictionary<string, decimal>();
            Dictionary<int, KeyValuePair<string, decimal>> metalPrices = new Dictionary<int, KeyValuePair<string, decimal>>();
            decimal dkkRate;
            decimal eurRate;

            // Gets the DKK and EUR rates from database
            try
            {
                string sqlQuery = "SELECT Code, Rates FROM Rates " +
                    "WHERE Code = 'DKK' OR Code = 'EUR'";

                // goes through the datatable returned by the sql query and adds the exchange rates and the codes of the valuta to a dictionary
                using (DataTable dt = DBReturnDataTable(sqlQuery))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        rates.Add(row["Code"].ToString(), Convert.ToDecimal(row["Rates"]));
                    }
                }
                // selects the exchange rates for DKK and EUR from the dictionary
                dkkRate = rates["DKK"];
                eurRate = rates["EUR"];
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            // Gets the metals from database
            try
            {
                string sqlQuery = "SELECT Id, MetalName, PricePerKg FROM PreciousMetals";

                using (DataTable dt = DBReturnDataTable(sqlQuery))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        metalPrices.Add(Convert.ToInt32(row["Id"]), new KeyValuePair<string, decimal>(row["MetalName"].ToString(), Convert.ToDecimal(row["PricePerKg"])));
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            // Calculates the prices in EUR and USD for the metals like the API does
            foreach (KeyValuePair<int, KeyValuePair<string, decimal>> metal in metalPrices)
            {
                res.Add(new ClassApiResponds
                {
                    id = metal.Key,
                    metalName = metal.Value.Key,
                    priceDkkPerKg = metal.Value.Value,
                    priceEurPerKg = (metal.Value.Value / dkkRate) * eurRate,
                    priceUsdPerKg = metal.Value.Value / dkkRate
                });
            }
            return res;
        }       
    }
}