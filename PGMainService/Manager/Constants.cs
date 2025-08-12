using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGMainService.Manager
{
    public class Constants
    {
        #region Constantes
        public const string PG_HTTP_PING_RESPONSE_PAYMENT = "PG_HTTP_PING_RESPONSE_PAYMENT";
        public const string PG_HTTP_BADREQUEST = "PG_HTTP_BADREQUEST";
        public const string PG_HTTP_NOAUTH = "PG_HTTP_NOAUTH";
        public const string PG_HTTP_ERROR = "PG_HTTP_ERROR";
        public const string PG_HTTP_NO_CREDENTIALS = "PG_HTTP_NO_CREDENTIALS";
        public const string PG_HTTP_PING_RESPONSE_PAYMENT_OFF = "PG_HTTP_PING_RESPONSE_PAYMENT_OFF";
        public const string PAYMENT_NOCONFIG = "PAYMENT_NOCONFIG";
        public const string PAYMENT_NOSERVICE = "PAYMENT_NOSERVICE";
        public const string PAYMENT_INVALIDAMOUNT = "PAYMENT_INVALIDAMOUNT";
        public const string PAYMENT_INVALIDPRODUCT = "PAYMENT_INVALIDPRODUCT";
        public const string PAYMENT_INVALIDCHAN = "PAYMENT_INVALIDCHAN";
        public const string PAYMENT_INVALIDCURRENCY = "PAYMENT_INVALIDCURRENCY";
        public const string PG_HTTP_CONFIG_ERROR = "PG_HTTP_CONFIG_ERROR";
        public const string PG_HTTP_NOTACCEPTABLE = "PG_HTTP_NOTACCEPTABLE";
        public const string PG_HTTP_OK = "PG_HTTP_OK";
        public const string PG_HTTP_REDIRECT = "PG_HTTP_REDIRECT";
        #endregion
    }
}