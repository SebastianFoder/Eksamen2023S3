using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ClassAddress : ClassNotify
    {
        //Private field
        private string _StreetName;
        private string _ZipCity;
        private KeyValuePair<string, int> _Country;
        private string _Phone;
        private string _MailAdr;

        //Constructor to ClassAddress
        public ClassAddress()
        {
            StreetName = string.Empty;
            ZipCity = string.Empty;
            Country = new KeyValuePair<string, int>();
            Phone = string.Empty;
            MailAdr = string.Empty;
        }

        // Overloaded Constructor to ClassAddress
        public ClassAddress(ClassAddress inAddress)
        {
            StreetName= inAddress.StreetName;
            ZipCity= inAddress.ZipCity;
            Country= inAddress.Country;
            Phone= inAddress.Phone;
            MailAdr= inAddress.MailAdr;
        }

        public string StreetName
        {
            get { return _StreetName; }
            set
            {
                if (_StreetName != value)
                {
                    _StreetName = value;
                }
                Notify("StreetName");
            }
        }
        public string ZipCity
        {
            get { return _ZipCity; }
            set
            {
                if (_ZipCity != value)
                {
                    _ZipCity = value;
                }
                Notify("ZipCity");
            }
        }
        public KeyValuePair<string, int> Country
        {
            get { return _Country; }
            set
            {
                if (_Country.Value != value.Value)
                {
                    _Country = value;
                }
                Notify("Country");
            }
        }
        public string Phone
        {
            get { return _Phone; }
            set
            {
                if (_Phone != value)
                {
                    _Phone = value;
                }
                Notify("Phone");
            }
        }
        public string MailAdr
        {
            get { return _MailAdr; }
            set
            {
                if (_MailAdr != value)
                {
                    _MailAdr = value;
                }
                Notify("MailAdr");
            }
        }


    }
}
