using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ClassInvoice : ClassEntity, IPayable
    {

        private ObservableCollection<ClassOrderLine> _OrderLines;
        private ClassCustomer _OrderCustomer;

        public ClassInvoice()
        {
            OrderCustomer = new ClassCustomer();
            OrderLines = new ObservableCollection<ClassOrderLine>();
        }
        public ClassInvoice(ClassInvoice inVoice)
        {
            OrderLines = inVoice.OrderLines;
            OrderCustomer = inVoice.OrderCustomer;
        }

        public ClassCustomer OrderCustomer
        {
            get { return _OrderCustomer; }
            set
            {
                if (_OrderCustomer != value)
                {
                    _OrderCustomer = value;
                }
                Notify("OrderCustomer");
            }
        }
        public ObservableCollection<ClassOrderLine> OrderLines
        {
            get { return _OrderLines; }
            set
            {
                if (_OrderLines != value)
                {
                    _OrderLines = value;
                }
                Notify("OrderLines");
            }
        }


        /// <summary>
        /// Goes through all of the orders and return the sum of all the products prices times the quantity of said product
        /// </summary>
        /// <returns></returns>
        public decimal GetPaymentAmount()
        {
            decimal sum = 0;
            foreach (ClassOrderLine line in OrderLines)
            {
                // Convert to decimal before multiplying to increase precision 
                sum += (decimal)line.Product.DKKPrice * (decimal)line.Quantity;
            }
            return sum;
        }
    }
}
