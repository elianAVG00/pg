using PGDataAccess.Repository;
using System;
using System.Security.Cryptography;
namespace PGDataAccess.Tools
{
    public class CustomTool
    {
        public static string HashCode(string str)
        {
            var encoder = new System.Text.ASCIIEncoding();
            byte[] buffer = encoder.GetBytes(str);
            SHA256 cryptoTransformSHA256 = SHA256.Create();
            string hash = BitConverter.ToString(cryptoTransformSHA256.ComputeHash(buffer)).Replace("-", "");

            return hash;
        }

        public static long ConvertTransactionToTN(string transactionId)
        {
            string trunkedTransaction = transactionId.Substring(0, 3);
            string transactionToConvert = transactionId;
            if (transactionId.Substring(0, 3) == "003")
            {
                //Es SPS Vieja 
                transactionToConvert = transactionId.Substring(3);
            }
            else if (Convert.ToInt32(transactionId) < 70000000)
            {
                //ES NPS VIEJA
                transactionToConvert = PaymentRepository.GetTransactionNumberAsStringByTransactionId(transactionId);
            }
            else
            {
                //ES DE PG
                transactionToConvert = transactionId;
            }
            long convertedTransactionToReturn = Convert.ToInt64(transactionToConvert);
            return convertedTransactionToReturn;
        }

        public static string ConvertTNtoTransaction(long transactionNumber)
        {
            string convertedTransactionToReturn = "";
            convertedTransactionToReturn = transactionNumber.ToString();
            if (transactionNumber < 14464886)
            { 
                //Es SPS Vieja 
                convertedTransactionToReturn = "003" + convertedTransactionToReturn;
            }
            return convertedTransactionToReturn;
        }
    }
}