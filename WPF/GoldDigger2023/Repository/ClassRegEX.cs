using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Repository
{
    /// <summary>
    /// Contains Regular Expressions
    /// </summary>
    public class ClassRegEX
    {
        public ClassRegEX() {}

        public bool ContainsNumbers(string input, int minimum = 1)
        {
            return Regex.IsMatch(input, $@"(.*\d){{{minimum}}}");
        }

        public bool IsPhoneNo (string input)
        {
            return Regex.IsMatch(input, @"^(\+?(00)?\d\d[ -]*)?(\d[ -]*){8}$");
        }

        public bool IsCPR(string input)
        {
            return Regex.IsMatch(input, @"^(0[1-9]|[12]\d|3[01])[ -]?(0[1-9]|1[012])[ -]?\d\d[ -]?\d{4}$");
        }

        public bool IsLicensePlate(string input)
        {
            return Regex.IsMatch(input, @"^[A-HJ-PR-Z][A-HJ-NPR-Z][  -]?\d{2}[ -]?\d{3}$");
        }

        public bool IsMailAddress(string input)
        {
            return Regex.IsMatch(input, @"^[!#-'*+/-9=?^-~-]+(\.[!#-'*+/-9=?^-~-]+)*@([a-z0-9]+(-+[a-z0-9]+)*\.)+[a-z0-9]+(-+[a-z0-9]+)*$");
        }
            
    }
}
