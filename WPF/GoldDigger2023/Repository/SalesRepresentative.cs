using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class SalesRepresentative : BaseSalariedEmployee
    {
        protected decimal monthlySales;
        protected double commisionRate;

        public SalesRepresentative() : base()
        {
            monthlySales = 0m;
            commisionRate = 0D;
        }

        public SalesRepresentative(int inId, string inName, ClassAddress inAddress, decimal inSalary, string inTitle, int inTId, decimal inSales, double inCommisionRate) : base(inId, inName, inAddress, inSalary, inTitle, inTId)
        {
            monthlySales = inSales;
            commisionRate = inCommisionRate;
        }

        public SalesRepresentative(SalesRepresentative inSalesRep) : base(inSalesRep)
        {
            monthlySales = inSalesRep.monthlySales;
            commisionRate = inSalesRep.commisionRate;
        }

        public override decimal Earnings() 
        {
            return salary + (decimal)commisionRate;
        }
    }
}
