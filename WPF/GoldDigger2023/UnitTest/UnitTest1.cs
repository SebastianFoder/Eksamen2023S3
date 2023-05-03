using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Repository;
using System.Data;
using IO;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        ClassRegEX regex = new ClassRegEX();

        // TestContext er VIGTIG! Og skal skrives med stort T, ellers kan DataSource ikke finde dataen
        private TestContext TestContext;

        [TestMethod]
        [DataSource("System.Data.SqlClient", @"Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=DDUT2022; Integrated Security=True", "EmailMixValidAndInvalid", DataAccessMethod.Sequential)]
        public void ValideringAfMailadresse()
        {
            //Arrange
            string Email = TestContext.DataRow["Email"].ToString();
            bool exp = Convert.ToBoolean(TestContext.DataRow["Valid"]);

            //Act
            bool res = regex.IsMailAddress(Email);
            //Assert
            Assert.AreEqual(exp, res);
        }
    }
}