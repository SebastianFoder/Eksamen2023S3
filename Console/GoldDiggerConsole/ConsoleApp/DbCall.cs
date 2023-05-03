using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    /// <summary>
    /// Interacts with the GoldDiggerDB database
    /// </summary>
    public class DbCall
    {
        private string _connectionString;
        private SqlConnection myConnection;

        /// <summary>
        /// Sets connection to the GoldDiggerDB database
        /// </summary>
        public DbCall()
        {
            _connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=GoldDiggerDB;Trusted_Connection=True;";
            myConnection = new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Opens connection to the server
        /// </summary>
        private void OpenDB()
        {
            try
            {
                // Hvis forbindelsen eksisterer og er lukket, bliver der åbnet forbindelse
                if (this.myConnection != null && myConnection.State == ConnectionState.Closed)
                {
                    myConnection.Open();
                }
                else
                {
                    // Undersøger hvilken af ovenstående betingelser er forkert
                    // Hvis forbindelsen ikke eksisterer, laver ny forbindelse
                    if (!(this.myConnection != null))
                    {
                        myConnection = new SqlConnection(_connectionString);
                    }
                    // Hvis forbindelsen eksisterer genåbnes forbindelsen
                    else
                    {
                        CloseDB();
                    }
                    OpenDB();
                }
            }
            catch (SqlException ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Closes connection to the server
        /// </summary>
        private void CloseDB()
        {
            try
            {
                myConnection.Close();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to bulkcopy all of USDRates contents into the database
        /// </summary>
        /// <param name="inUsdRate"></param>
        public async Task UpdateRateInDB(USDRate inUsdRate)
        {
            // NOTE: Bulkcopy will only insert, NOT update, which is why we first need to delete everything in Rates
            const string deleteQuery = "DELETE FROM Rates";

            // Creates the table that will be inserted into the database
            DataTable rateTable = new DataTable();
            rateTable.Columns.Add("Id", typeof(int));
            rateTable.Columns.Add("Code", typeof(string));
            rateTable.Columns.Add("Rates", typeof(string));

            try
            {
                // Inserts all the rates from USDRate into the table
                foreach (var rate in inUsdRate.Rates)
                {
                    //Example:         --    EUR       1.024106
                    rateTable.Rows.Add(null, rate.Key, rate.Value);
                }

                OpenDB();
                // Delete everything from the database's Rates table to make space for new rates
                using (SqlCommand cmd = new SqlCommand(deleteQuery, myConnection))
                {
                    await cmd.ExecuteNonQueryAsync();
                }

                // Bulkcopy all the tables contents into the database
                using (SqlBulkCopy copy = new SqlBulkCopy(myConnection))
                {
                    copy.DestinationTableName = $"Rates";
                    await copy.WriteToServerAsync(rateTable);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                CloseDB();
            }
        }
    }
}
