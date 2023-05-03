using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ClassProduct : ClassEntity
    {
        private string _Name;
        private double _DKKPrice;
        private double _EurPrice;
        private double _UsdPrice;
        private ClassStock _Stock;

        public ClassProduct()
        {
            Name = string.Empty;
            DKKPrice = 0.0;
            EurPrice = 0.0;
            UsdPrice = 0.0;
            Stock = new ClassStock();
        }

        public ClassProduct(int inId) : base(inId)
        {
            Name = string.Empty;
            DKKPrice = 0.0;
            EurPrice = 0.0;
            UsdPrice = 0.0;
            Stock = new ClassStock();
        }

        public ClassProduct(ClassProduct inProduct)
        {
            Name = inProduct.Name;
            DKKPrice = inProduct.DKKPrice;
            EurPrice = inProduct.EurPrice;
            UsdPrice = inProduct.UsdPrice;
            Stock = new ClassStock(inProduct.Stock);
        }

        public double UsdPrice
        {
            get { return _UsdPrice; }
            set
            {
                if (_UsdPrice != value)
                {
                    _UsdPrice = value;
                }
                Notify("UsdPrice");
            }
        }
        public double EurPrice
        {
            get { return _EurPrice; }
            set
            {
                if (_EurPrice != value)
                {
                    _EurPrice = value;
                }
                Notify("EurPrice");
            }
        }
        public double DKKPrice
        {
            get { return _DKKPrice; }
            set
            {
                if (_DKKPrice != value)
                {
                    _DKKPrice = value;
                }
                Notify("DKKPrice");
            }
        }
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                }
                Notify("Name");
            }
        }
        public ClassStock Stock
        {
            get { return _Stock; }
            set
            {
                if (_Stock != value)
                {
                    _Stock = value;
                }
                Notify("Stock");
            }
        }
    }
}
