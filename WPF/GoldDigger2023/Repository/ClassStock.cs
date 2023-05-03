using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ClassStock  : ClassNotify
    {
        private string _Warehouse;
        private string _StorageRack;
        private string _Shelf;
        private double _StockQuantity;

        public ClassStock()
        {
            Warehouse = string.Empty;
            StorageRack = string.Empty;
            Shelf = string.Empty;
            StockQuantity = 0;
        }

        public ClassStock(ClassStock inStock)
        {
            Warehouse = inStock.Warehouse;
            StorageRack = inStock.StorageRack;
            Shelf = inStock.Shelf;
            StockQuantity = inStock.StockQuantity;
        }

        public string Warehouse
        {
            get { return _Warehouse; }
            set
            {
                if (_Warehouse != value)
                {
                    _Warehouse = value;
                }
                Notify("Warehouse");
            }
        }
        public string StorageRack
        {
            get { return _StorageRack; }
            set
            {
                if (_StorageRack != value)
                {
                    _StorageRack = value;
                }
                Notify("StorageRack");
            }
        }
        public string Shelf
        {
            get { return _Shelf; }
            set
            {
                if (_Shelf != value)
                {
                    _Shelf = value;
                }
                Notify("Shelf");
            }
        }
        public double StockQuantity
        {
            get { return _StockQuantity; }
            set
            {
                if (_StockQuantity != value)
                {
                    _StockQuantity = value;
                }
                Notify("StockQuantity");
            }
        }
    }
}
