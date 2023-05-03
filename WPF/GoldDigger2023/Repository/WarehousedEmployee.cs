using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class WarehousedEmployee : BaseSalariedEmployee
    {
        protected decimal DangerSupplement;
        protected double ExperienceBonus;

        public WarehousedEmployee() : base()
        {
            DangerSupplement = 0m;
            ExperienceBonus = 0D;
        }

        public WarehousedEmployee(int inId, string inName, ClassAddress inAddress, string inTitle, int inTId, decimal inSalary, decimal inSupplement, double inExperience) : base(inId, inName, inAddress, inSalary, inTitle, inTId)
        {
            DangerSupplement = inSupplement;
            ExperienceBonus = inExperience;
        }

        public WarehousedEmployee(WarehousedEmployee inWare) : base(inWare)
        {
            DangerSupplement = inWare.DangerSupplement;
            ExperienceBonus = inWare.ExperienceBonus;
        }

        public override decimal Earnings()
        {
            return 0M; //salary + DangerSupplement + ExperienceBonus;
        }
    }
}
