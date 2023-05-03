using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    /// <summary>
    /// Hej Jens.
    /// Jeg vil gerne admone om at lave 2 strings til contact i Customer
    /// 
    /// Det er OK med de ændringer du foreslår 😉
    /// </summary>
    public class ClassCustomer : ClassEntity
    {
        //private string _Name;
        private ClassAddress _Address;
        private string _contactName; 
        private string _contactTitle;
        private string _companyName;

        /// <summary>
        /// Constructor to ClassCustomer, gets ID from ClassEntity that is base
        /// </summary>
        public ClassCustomer() : base()
        {
            //Name = string.Empty;
            Address = new ClassAddress();
            contactName = string.Empty;
            contactTitle = string.Empty;
            companyName = string.Empty;
        }

        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// <param name="inId"></param>
        /// <param name="inAddress"></param>
        /// <param name="inContactName"></param>
        /// <param name="inTitle"></param>
        /// <param name="inCompanyName"></param>
        public ClassCustomer(int inId, ClassAddress inAddress, string inContactName, string inTitle, string inCompanyName) : base(inId)
        {
            //Name = inName;
            Address = new ClassAddress(inAddress);
            contactName = inContactName;
            contactTitle = inTitle;
            companyName = inCompanyName;
        }

        public ClassCustomer(ClassCustomer inCustomer) : base(inCustomer)
        {
            //Name = inCustomer.Name;
            Address = new ClassAddress(inCustomer.Address);
            contactName = inCustomer.contactName;
            contactTitle = inCustomer.contactTitle;
            companyName = inCustomer.companyName;
        }

        //public string Name
        //{
        //    get { return _Name; }
        //    set
        //    {
        //        if (_Name != value)
        //        {
        //            _Name = value;
        //        }
        //        Notify("Name");
        //    }
        //}
        public ClassAddress Address
        {
            get { return _Address; }
            set
            {
                if (_Address != value)
                {
                    _Address = value;
                }
                Notify("Address");
            }
        }
        public string contactName
        {
            get { return _contactName; }
            set
            {
                if (_contactName != value)
                {
                    _contactName = value;
                }
                Notify("contactName");
            }
        }
        public string contactTitle
        {
            get { return _contactTitle; }
            set
            {
                if (_contactTitle != value)
                {
                    _contactTitle = value;
                }
                Notify("contactTitle");
            }
        }
        public string companyName
        {
            get { return _companyName; }
            set
            {
                if (_companyName != value)
                {
                    _companyName = value;
                }
                Notify("companyName");
            }
        }
    }
}