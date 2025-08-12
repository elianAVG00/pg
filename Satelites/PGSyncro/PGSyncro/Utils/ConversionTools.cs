using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGSyncro.Utils
{
    public static class StringExtension
    {
        public static string GetLast(this string source, int tail_length)
        {
            if (tail_length >= source.Length)
                return source;
            return source.Substring(source.Length - tail_length);
        }
    }
    public static class ConversionTools
    {
        public static string GetKeyOrEmpty(Dictionary<string,string> dictionary, string key) {
            if (dictionary.TryGetValue(key, out string defaultValue))
            {
                return defaultValue;
            }
            else {
                return "";
            }
        }
        public static String EncriptStringWithSHA256(string key, string message)
        {

            UTF8Encoding encoding = new UTF8Encoding();
            byte[] keyBytes = encoding.GetBytes(key);
            byte[] messageBytes = encoding.GetBytes(message);
            System.Security.Cryptography.HMACSHA256 cryptographer = new System.Security.Cryptography.HMACSHA256(keyBytes);

            byte[] bytes = cryptographer.ComputeHash(messageBytes);

            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            //return HashHMACHex(key, message);
        }
        public static int? ToNullableInt32(string s)
        {
            int i;
            if (Int32.TryParse(s, out i)) return i;
            return null;
        }
        public static decimal ConvertStringToDecimal(string strDecimal)
        {

            int amountInteger = Convert.ToInt32(ConvertDecimalToInt(strDecimal));
            decimal amountDecimal = (decimal)amountInteger / 100;
            return amountDecimal;
        }
        public static  string ConvertDecimalToInt(string strDecimal)
        {
            strDecimal = strDecimal.Replace(".", ",");

            if (strDecimal.Contains(","))
            {
                int decimalSeparator = strDecimal.LastIndexOf(",");  //POSICION DE LA ULTIMA COMA
                int count = strDecimal.Split(',').Length - 1;

                string integerPart = strDecimal.Substring(0, decimalSeparator);
                string decimalPart = strDecimal.Substring(decimalSeparator, strDecimal.Length - decimalSeparator);
                integerPart = integerPart.Replace(",", "");
                decimalPart = decimalPart.Replace(",", "");

                if (count > 1)
                {
                    string finalNumber = "";
                    int cantidadDeDecimales = strDecimal.Length - decimalSeparator - 1;
                    switch (cantidadDeDecimales)
                    {
                        case 0:
                            finalNumber = integerPart + "00";
                            break;

                        case 1:
                            finalNumber = integerPart + decimalPart + "0";
                            break;

                        case 2:
                            finalNumber = integerPart + decimalPart;
                            break;

                        default:
                            finalNumber = integerPart + decimalPart + "00";
                            break;
                    }
                    return finalNumber;
                }
                else if (count == 1)
                {
                    string finalNumber = "";
                    int cantidadDeDecimales = strDecimal.Length - decimalSeparator - 1;
                    switch (cantidadDeDecimales)
                    {
                        case 0:
                            finalNumber = integerPart + "00";
                            break;

                        case 1:
                            finalNumber = integerPart + decimalPart + "0";
                            break;

                        case 2:
                            finalNumber = integerPart + decimalPart;
                            break;

                        default:
                            finalNumber = integerPart + decimalPart + "00";
                            break;
                    }
                    return finalNumber;
                }
                else
                {
                    if (decimalPart.Length == 1)
                    {
                        return (integerPart + decimalPart + "0");
                    }
                    else
                    {
                        return integerPart + decimalPart;
                    }
                }
            }
            else
            {
                return (strDecimal + "00");
            }
        }


    }
}
