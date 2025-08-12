using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGSyncro.Models
{
    public class SimpleQueryTx_RequerimientoModel
    {


        public string MerchantId { get; set; }
        public string QueryCriteriaId { get; set; }
        public string QueryCriteria { get; set; }
        public string SecurityHash { get; set; }


    }
    public class SimpleQueryTx_RespuestaModel
    {
        public string CommunicationResponseCode { get; set; }
        public string TransactionResponseCode { get; set; }
        public NpsTransactionModel Transaction { get; set; }


       
    }
    public class NpsTransactionModel
    {
        public string psp_ResponseCod { set; get; }

        /// <remarks/>
        public string psp_ResponseMsg { set; get; }

        /// <remarks/>
        public string psp_ResponseExtended { set; get; }

        /// <remarks/>
        public string psp_TransactionId { set; get; }

        /// <remarks/>
        public string psp_MerchantId { set; get; }

        /// <remarks/>
        public string psp_TxSource { set; get; }

        /// <remarks/>
        public string psp_Operation { set; get; }

        /// <remarks/>
        public string psp_MerchTxRef { set; get; }

        /// <remarks/>
        public string psp_MerchOrderId { set; get; }

        /// <remarks/>
        public string psp_MerchAdditionalRef { set; get; }

        /// <remarks/>
        public string psp_Amount { set; get; }

        /// <remarks/>
        public string psp_NumPayments { set; get; }

        /// <remarks/>
        public string psp_PaymentAmount { set; get; }

        /// <remarks/>
        public string psp_FirstPaymentDeferral { set; get; }

        /// <remarks/>
        public string psp_Currency { set; get; }

        /// <remarks/>
        public string psp_Country { set; get; }

        /// <remarks/>
        public string psp_Product { set; get; }

        /// <remarks/>
        public string psp_AuthorizationCode { set; get; }

        /// <remarks/>
        public string psp_BatchNro { set; get; }

        /// <remarks/>
        public string psp_SequenceNumber { set; get; }

        /// <remarks/>
        public string psp_TicketNumber { set; get; }

        /// <remarks/>
        public string psp_CardNumber_FSD { set; get; }

        /// <remarks/>
        public string psp_CardNumber_LFD { set; get; }

        /// <remarks/>
        public string psp_CardMask { set; get; }

        /// <remarks/>
        public string psp_CardHolderName { set; get; }

        /// <remarks/>
        public string psp_CustomerMail { set; get; }

        /// <remarks/>
        public string psp_CustomerId { set; get; }

        /// <remarks/>
        public string psp_CustomerIpAddress { set; get; }

        /// <remarks/>
        public string psp_CustomerHttpUserAgent { set; get; }

        /// <remarks/>
        public string psp_MerchantMail { set; get; }

        /// <remarks/>
        public string psp_ClTrnId { set; get; }

        /// <remarks/>
        public string psp_ClExternalMerchant { set; get; }

        /// <remarks/>
        public string psp_ClExternalTerminal { set; get; }

        /// <remarks/>
        public string psp_ClResponseCod { set; get; }

        /// <remarks/>
        public string psp_ClResponseMsg { set; get; }

        /// <remarks/>
        public string psp_PosDateTime { set; get; }

        /// <remarks/>
        public string psp_PurchaseDescription { set; get; }

        /// <remarks/>
        public string psp_SoftDescriptor { set; get; }

        /// <remarks/>
        public string psp_Plan { set; get; }

        /// <remarks/>
        public string psp_VisaArg_VBV_Secured { set; get; }

        /// <remarks/>
        public string psp_3dSecure_Eci { set; get; }

        /// <remarks/>
        public string psp_3dSecure_EciMsg { set; get; }

        /// <remarks/>
        public string psp_3dSecure_Secured { set; get; }

        /// <remarks/>
        public string psp_VisaArg_DA_Result { set; get; }

        /// <remarks/>
        public string psp_AmexArg_AVS_Result { set; get; }

        /// <remarks/>
        public string psp_MasterArg_AVS_Result { set; get; }

        /// <remarks/>
        public string psp_CardSecurityCodeResult { set; get; }

        /// <remarks/>
        public string psp_CustomerMailVerification { set; get; }
    }


    }
