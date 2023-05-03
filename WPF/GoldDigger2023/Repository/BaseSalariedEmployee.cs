using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class BaseSalariedEmployee : ClassEmployee
    {
        protected decimal salary;

        /// <summary>
        /// Constructor to BaseSalariedEmployee, adds salary to all properties from ClassEmployee that is base
        /// </summary>
        public BaseSalariedEmployee() : base()
        {
            salary = 0m;
        }


        /// <summary>
        /// Overloaded constructor to BaseSalariedEmployee
        /// </summary>
        /// <param name="inId"></param>
        /// <param name="inName"></param>
        /// <param name="inAddress"></param>
        /// <param name="inSalary"></param>
        /// <param name="inTitle"></param>
        /// <param name="inTId"></param>
        public BaseSalariedEmployee(int inId, string inName, ClassAddress inAddress, decimal inSalary, string inTitle, int inTId) : base(inId, inName, inAddress, inTitle, inTId)
        {
            salary = inSalary;
        }

        public BaseSalariedEmployee(BaseSalariedEmployee inEmployee) : base(inEmployee)
        {
            salary = inEmployee.salary;
        }


        /// <summary>
        /// An overide Method that returns how much an employee earns
        /// </summary>
        /// <returns>salary as a decimal</returns>
        public override decimal Earnings()
        {
            return salary;
        }
    }
}