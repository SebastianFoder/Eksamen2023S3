using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    /// <summary>
    /// Hej Jens.
    /// Jeg vil gerne admone om at lave 1 string til title i Employee
    /// 
    /// Det er OK med de ændringer du foreslår 😉
    /// </summary>
    public abstract class ClassEmployee : ClassEntity, IPayable
    {
        private string _name;
        private ClassAddress _Address;
        private string _title;
        private int _titleId;

        public ClassEmployee() : base()
        {
            name = string.Empty;
            Address = new ClassAddress();
            title = string.Empty;
            titleId = 0;
        }

        public ClassEmployee(int inId, string inName, ClassAddress inAddress, string inTitle, int inTId) : base(inId)
        {
            name = inName;
            Address = new ClassAddress(inAddress);
            title = inTitle;
            titleId = inTId;
        }

        public ClassEmployee(ClassEmployee inEmployee) : base(inEmployee.Id)
        {
            name = inEmployee.name;
            Address = new ClassAddress(inEmployee.Address);
            title = inEmployee.title;
            titleId = inEmployee.titleId;
        }
        
        public string name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                }
                Notify("name");
            }
        }
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
        public string title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                }
                Notify("title");
            }
        }
        public int titleId
        {
            get { return _titleId; }
            set
            {
                if (_titleId != value)
                {
                    _titleId = value;
                }
                Notify("titleId");
            }
        }
        public decimal Payment
        {
            get { return GetPaymentAmount(); }
        }



        public abstract decimal Earnings();

        public decimal GetPaymentAmount()
        {
            return Earnings();
        }
    }
}
