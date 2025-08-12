using PGSyncro.EFData;
using PGSyncro.Models;
using PGSyncro.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace PGSyncro.Validators
{
   public static class NPSCall
    {
        public static TransactionOriginal GetNPSTransaction(GetTransactionsToSync_Result transaction, string hash)
        {
            TransactionOriginal retorno = new TransactionOriginal();
            SyncroModel QueryResponse = new SyncroModel
            {
                ServiceType = "NPS 2.0"
            };
            retorno.ModuleType = "payment";
            retorno.TransactionIdPK = transaction.TransactionIdPK;

            try
            {

                var sb = new StringBuilder();
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                NPS_ServiceReference_PROD.RequerimientoStruct_SimpleQueryTx queryPROD = new NPS_ServiceReference_PROD.RequerimientoStruct_SimpleQueryTx();
                NPS_ServiceReference_PROD.PaymentServicePlatform clientPROD = new NPS_ServiceReference_PROD.PaymentServicePlatform();

                queryPROD.psp_MerchantId = transaction.UniqueCode;
                queryPROD.psp_QueryCriteriaId = transaction.TransactionId;
                queryPROD.psp_QueryCriteria = "T";
                queryPROD.psp_Version = "2.2";
                queryPROD.psp_PosDateTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                sb.Append(queryPROD.psp_MerchantId);
                sb.Append(queryPROD.psp_PosDateTime);
                sb.Append(queryPROD.psp_QueryCriteria);
                sb.Append(queryPROD.psp_QueryCriteriaId);
                sb.Append(queryPROD.psp_Version);
                queryPROD.psp_SecureHash = ConversionTools.EncriptStringWithSHA256(hash, sb.ToString());

                clientPROD.Proxy = NetworkTools.GetWebProxy();
                QueryResponse.RequestLog = JsonConvert.SerializeObject(queryPROD);
                NPS_ServiceReference_PROD.RespuestaStruct_SimpleQueryTx answerPROD = clientPROD.SimpleQueryTx(queryPROD);

                QueryResponse.ResponseLog = JsonConvert.SerializeObject(answerPROD);



                if (answerPROD.psp_ResponseCod != "2")
                {
                    QueryResponse.HasTransaction = false;
                    QueryResponse.HasError = true;
                    QueryResponse.ErrorInQuery = "Hubo un error en la comunicación";
                    QueryResponse.HasResponse = false;
                }
                else
                {
                    NPS_ServiceReference_PROD.RespuestaStruct_SimpleQueryTx_Transactions npsTransaction =  answerPROD.psp_Transaction;
                    retorno.OriginalCode    = npsTransaction.psp_ResponseCod;
                    long longAmount = long.TryParse(npsTransaction.psp_Amount, out longAmount) ? longAmount : 0;
                    retorno.Amount = longAmount;
                    retorno.AuthorizationCode = npsTransaction.psp_AuthorizationCode;
                    retorno.Card4LastDigits = npsTransaction.psp_CardNumber_LFD;
                    retorno.CardHolder = npsTransaction.psp_CardHolderName;
                    retorno.CardMask = npsTransaction.psp_CardMask;
                    retorno.Mail = npsTransaction.psp_CustomerMail;
                    retorno.NroLote = npsTransaction.psp_BatchNro;
                    int intPayments = int.TryParse(npsTransaction.psp_NumPayments, out intPayments) ? intPayments : 0;
                    retorno.Payments = intPayments;
                    retorno.TicketNumber = npsTransaction.psp_TicketNumber;

                    QueryResponse.HasTransaction = true;
                    QueryResponse.HasError = false;
                    QueryResponse.HasResponse = true;

                }
            }
            catch (Exception ex)
            {
                QueryResponse.HasTransaction = false;
                QueryResponse.HasError = true;
                QueryResponse.ErrorInQuery = ex.Message;
            }

            retorno.QueryResponse = QueryResponse;
            return retorno;



        }
    }
}
