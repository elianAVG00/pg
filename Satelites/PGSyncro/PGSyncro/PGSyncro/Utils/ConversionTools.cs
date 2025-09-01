using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace PGSyncro.Utils
{
    public class ConversionTools
    {
        public ConversionTools() { }
        public string GetKeyOrEmpty(Dictionary<string, string> dictionary, string key)
        {
            return dictionary.TryGetValue(key, out string value) ? value : "";
        }

        public string EncriptStringWithSHA256(string key, string message)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
        public int? ToNullableInt32(string s)
        {
            return int.TryParse(s, out int i) ? i : (int?)null;
        }

        public decimal ConvertStringToDecimal(string strDecimal)
        {
            if (string.IsNullOrWhiteSpace(strDecimal)) return 0M;

            string numericString = strDecimal.Replace(".", ",");
            if (!numericString.Contains(","))
            {
                numericString += ",00";
            }
            string[] parts = numericString.Split(',');
            string integerPart = Regex.Replace(parts[0], "[^0-9-]", "");
            string decimalPart = parts.Length > 1 ? Regex.Replace(parts[1], "[^0-9]", "") : "00";

            decimalPart = decimalPart.PadRight(2, '0').Substring(0, 2);

            if (long.TryParse(integerPart + decimalPart, out long cents))
            {
                return (decimal)cents / 100;
            }
            return 0M;
        }
    }
}