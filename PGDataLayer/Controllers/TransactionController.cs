using PGDataLayer.Models;
using PGDataLayer.Repositories;
using PGDataLayer.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PGDataLayer.Controllers
{
    [AuthorizationFilter]
    public class TransactionController : ApiController
    {
        [Route("transaction/isEPCvalid"), HttpPost]
        public HttpResponseMessage isEPCvalid(IsEPCValidModel input)
        {
            return Request.CreateResponse(HttpStatusCode.OK, TransactionRepository.IsEPCValid(input));
        }

        [Route("transaction/GetRenditionReportOnTheFly"), HttpPost]
        public HttpResponseMessage GetRenditionReportOnTheFly(RenditionFileInput input)
        {
            return Request.CreateResponse(HttpStatusCode.OK, TransactionRepository.GetRenditionReportOnTheFly(input));
        }

        [Route("transaction/UpdateTransactionIdFromTransactionNumber/{transactionIdPK}"), HttpGet]
        public HttpResponseMessage UpdateTransactionIdFromTransactionNumber(long transactionIdPK)
        {
            return Request.CreateResponse(HttpStatusCode.OK, TransactionRepository.UpdateTransactionIdFromTransactionNumber(transactionIdPK));
        }

        [Route("transaction/TN/{transactionIdPK}"), HttpGet]
        public HttpResponseMessage isEPCvalid(long transactionIdPK)
        {
            return Request.CreateResponse(HttpStatusCode.OK, TransactionRepository.GetTransactionNumberByTransactionId(transactionIdPK));
        }

        [Route("transaction/TAI/{transactionIdPK}"), HttpGet]
        public HttpResponseMessage GetTAI(long transactionIdPK)
        {
            return Request.CreateResponse(HttpStatusCode.OK, TransactionRepository.GetTAI(transactionIdPK));
        }
    }
}