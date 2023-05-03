using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace IO
{
    public class ClassDbCon
    {
        private string connectionString;
        private SqlCommand command;
        protected SqlConnection con;

        public ClassDbCon()
        {
            // oprette en statisk opsætning af forbindelse
            // this.connectionString = "Din egen connection string";
            // this.con = new SqlConnection(connectionString);
        }
        public ClassDbCon(string inConnectionString)
        {
            this.connectionString = inConnectionString;
            this.con = new SqlConnection(connectionString);
        }

        protected void SetCon(string inConnectionString)
        {
            this.connectionString = inConnectionString;
            this.con = new SqlConnection(connectionString);
        }

        /// <summary>
        /// Denne metode åbner forbindelsen til databasen.
        /// Den undersøger om alle betingelser er opfyldt for at åbne forbindelsen, inden den åbnes.
        /// Hvis betingelserne ikke er opfyldt, vil den prøve at håndtere de mest almindelige fejl og mangler. 
        /// </summary>
        protected void OpenDB()
        {
            try
            {
                if (this.con != null && con.State == ConnectionState.Closed)
                {
                    con.Open(); // Åbner forbindelse til databasen
                }
                else
                {
                    // Undersøger om fejlen skyldes at instansen con IKKE er initialiseret
                    if (!(this.con != null))
                    {
                        // Initialisere con med den angivne connectionString
                        con = new SqlConnection(connectionString);
                        // Udføre et rekursivt kald, for at foretage et nyt forsøg på at skabe
                        // forbindelse til databasen.
                        OpenDB();
                    }
                    else
                    {
                        // Hvis nej - Lukker forbindelsen og laver et rekursivt kald, for at
                        // foretage et nyt forsøg på at skabe forbindelse til databasen.
                        CloseDB();
                        OpenDB();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Denne metode lukker forbindelsen til en given database der knyttet via con.
        /// </summary>
        protected void CloseDB()
        {
            try
            {
                con.Close();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Denne metode har til formål, at udføre de handlinger i databasen, som ikke
        /// kræver at der returneres et resultatsæt.
        /// Metoden vil dog altid returnere en intiger værdi der angiver om handlingen 
        /// gik godt eller skidt.
        /// Returneres: -1 er handlingen ikke blevet udført
        /// Returneres: Et tal fra 0 til N, indikerer det at udtrykket kunne eksekveres 
        /// og angiver hvor mange datasæt der blev påvirket
        /// </summary>
        /// <param name="sqlQuery">string</param>
        /// <returns>int</returns>
        protected int ExecuteNonQuery(string sqlQuery)
        {
            int res = 0;
            try
            {
                OpenDB();
                using (command = new SqlCommand(sqlQuery, con))
                {
                    res = command.ExecuteNonQuery();
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
            return res;
        }

        protected async Task<int> ExecuteNonQueryAsync(string sqlQuery)
        {
            int res = 0;
            try
            {
                OpenDB();
                using (command = new SqlCommand(sqlQuery, con))
                {
                    res = await command.ExecuteNonQueryAsync();
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
            return res;
        }

        protected int ExecuteNonQuery(SqlCommand inCommand)
        {
            int res = 0;
            try
            {
                OpenDB();
                res = inCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                CloseDB();
            }
            return res;
        }

        protected async Task<int> ExecuteNonQueryAsync(SqlCommand inCommand)
        {
            int res = 0;
            try
            {
                OpenDB();
                res = await inCommand.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                CloseDB();
            }
            return res;
        }


        protected int ExecuteScalarInDB(SqlCommand inCommand)
        {
            int res = 0;
            try
            {
                OpenDB();
                res = Convert.ToInt32(inCommand.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                CloseDB();
            }
            return res;
        }

        protected async Task<int> ExecuteScalarInDBAsync(SqlCommand inCommand)
        {
            int res = 0;
            try
            {
                OpenDB();
                res = Convert.ToInt32(await inCommand.ExecuteScalarAsync());
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                CloseDB();
            }
            return res;
        }


        /// <summary>
        /// Denne metode skal håndtere forespørgelser til databasen som skal 
        /// returnere et resultatsæt.
        /// Det resultatsæt der modtages fra DB, konverteres over i en 
        /// collection af typen DataTable.
        /// </summary>
        /// <param name="sqlQuery">string</param>
        /// <returns>DataTable</returns>
        protected DataTable DBReturnDataTable(string sqlQuery)
        {
            DataTable dtRes = new DataTable();
            try
            {
                OpenDB();
                // Her initialiseres instansen af SqlCommand med parameterne
                // string query og SqlConnection con
                using (command = new SqlCommand(sqlQuery, con))
                {
                    // Her foretages kaldet til databasen ved, at der
                    // oprettes en ny instans af en SqlDataAdapter. Resultatet
                    // overføres til en abstrakt datatype = adapter.
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        // Her overføres data fra den abstrakte datatype til
                        // den DataTable metoden skal returnere
                        adapter.Fill(dtRes);
                    }
                }
            }
            // Håndtere de exceptions (fejl) der måtte opstå under
            // kommunikationen med databasen
            catch (SqlException ex)
            {
                throw ex;
            }
            // Ved angivelse af 'finally' sikre jeg, at det der står i 'finally'
            // altid bliver udført, uanset om koden kunne eksekveres med eller uden fejl
            finally
            {
                CloseDB();
            }

            return dtRes;
        }

        protected async Task<DataTable> DBReturnDataTableAsync(string sqlQuery)
        {
            DataTable dtRes = new DataTable();
            try
            {
                OpenDB();
                // Her initialiseres instansen af SqlCommand med parameterne
                // string query og SqlConnection con
                using (command = new SqlCommand(sqlQuery, con))
                {
                    // Her foretages kaldet til databasen ved, at der
                    // oprettes en ny instans af en SqlDataAdapter. Resultatet
                    // overføres til en abstrakt datatype = adapter.
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        // Her overføres data fra den abstrakte datatype til
                        // den DataTable metoden skal returnere
                        await Task.Run(() =>
                        {
                            adapter.Fill(dtRes);
                        });                             
                    }
                }
            }
            // Håndtere de exceptions (fejl) der måtte opstå under
            // kommunikationen med databasen
            catch (SqlException ex)
            {
                throw ex;
            }
            // Ved angivelse af 'finally' sikre jeg, at det der står i 'finally'
            // altid bliver udført, uanset om koden kunne eksekveres med eller uden fejl
            finally
            {
                CloseDB();
            }

            return dtRes;
        }


        protected string DBReturnString(string sqlQuery)
        {
            string res = "";
            bool foundOne = false;

            try
            {
                // Åbner forbindelsen til databasen
                OpenDB();
                // Opretter en ny instans af SqlCommand med parameterne sqlQuery og con, 
                // som indeholder henholdsvis min sql forspørgelse og information omkring 
                // hvilken database data skal hentes fra.
                using (command = new SqlCommand(sqlQuery, con))
                {
                    // Her eksekveres forespørgelsen på databasen og svaret gemmes i reader
                    // som er af datatypen SqlDataReader der har samme egenskaber som en
                    // StreamReader, altså egenskaber der gør den
                    // egnet til at modtage og holde en stream af tekst
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Hvis reader har modtaget et resultat fra databasen, skal
                        // den udføre koden i while loopet
                        while (reader.Read())
                        {
                            // Læser teksten fra reader og indsætter den i res.
                            res = reader.GetString(0);
                            // Bolsk værdi, der angiver at der er modtaget et resultat.
                            foundOne = true;
                            // Afbryder iterationen efter første iteration.
                            break;
                        }
                        if (!foundOne)
                        {
                            // Hvis der ikke findes et resultat i DB, skal der returneres
                            // standard tekst eller en tom tekststreng.
                            res = "No Data Found!";
                        }
                    }
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

            return res;
        }

        protected async Task<string> DBReturnStringAsync(string sqlQuery)
        {
            string res = "";
            bool foundOne = false;

            try
            {
                // Åbner forbindelsen til databasen
                OpenDB();
                // Opretter en ny instans af SqlCommand med parameterne sqlQuery og con, 
                // som indeholder henholdsvis min sql forspørgelse og information omkring 
                // hvilken database data skal hentes fra.
                using (command = new SqlCommand(sqlQuery, con))
                {
                    // Her eksekveres forespørgelsen på databasen og svaret gemmes i reader
                    // som er af datatypen SqlDataReader der har samme egenskaber som en
                    // StreamReader, altså egenskaber der gør den
                    // egnet til at modtage og holde en stream af tekst
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        // Hvis reader har modtaget et resultat fra databasen, skal
                        // den udføre koden i while loopet
                        while (reader.Read())
                        {
                            // Læser teksten fra reader og indsætter den i res.
                            res = reader.GetString(0);                            
                            // Bolsk værdi, der angiver at der er modtaget et resultat.
                            foundOne = true;
                            // Afbryder iterationen efter første iteration.
                            break;
                        }
                        if (!foundOne)
                        {
                            // Hvis der ikke findes et resultat i DB, skal der returneres
                            // standard tekst eller en tom tekststreng.
                            res = "No Data Found!";
                        }
                    }
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

            return res;
        }


        /// <summary>
        /// Denne metode skal håndtere forespørgelser til databasen som skal 
        /// returnere et resultatsæt.
        /// Forespørgelsen skal foretages gennem en StoredProcedure på SqlServeren.
        /// Det resultatsæt der modtages fra DB, konverteres over i en collection 
        /// af typen DataTable.
        /// </summary>
        /// <param name="inCommand">SqlCommand</param>
        /// <returns>DataTable</returns>
        protected DataTable MakeCallToStoredProcedure(SqlCommand inCommand)
        {
            DataTable dtRes = new DataTable();
            try
            {
                // Åbner forbindelsen til databasen.
                OpenDB();
                // Her intialiseres en instans af SqlDataAdapter med værdien i inCommand
                using (SqlDataAdapter adapter = new SqlDataAdapter(inCommand))
                {
                    // Her overføres data fra adapter til den DataTable, metoden skal returnere.
                    adapter.Fill(dtRes);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally 
            {
                // Lukker forbindelsen til databasen.
                CloseDB(); 
            }  

            return dtRes;
        }

        protected async Task<DataTable> MakeCallToStoredProcedureAsync(SqlCommand inCommand)
        {
            DataTable dtRes = new DataTable();
            try
            {
                // Åbner forbindelsen til databasen.
                OpenDB();
                // Her intialiseres en instans af SqlDataAdapter med værdien i inCommand
                using (SqlDataAdapter adapter = new SqlDataAdapter(inCommand))
                {
                    // Her overføres data fra adapter til den DataTable, metoden skal returnere.
                    await Task.Run(() =>
                    {
                        adapter.Fill(dtRes);
                    });
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                // Lukker forbindelsen til databasen.
                CloseDB();
            }

            return dtRes;
        }
    }
}
