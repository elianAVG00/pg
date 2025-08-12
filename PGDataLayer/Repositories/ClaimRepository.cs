using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PGDataLayer.EF;
using PGDataLayer.Models;

namespace PGDataLayer.Repositories
{
    internal class ClaimRepository
    {
        internal static bool CanUserWorkWithPaymentClaimByPaymentClaimNumber(string user, long claimnumber)
        {


            using (var context = new EF.PaymentGatewayEntities())
            {
                long? njm = (from tai in context.TransactionAdditionalInfo
                           join pc in context.PaymentClaim
                           on tai.TransactionIdPK equals pc.TransactionId
                           join us in context.UserService
                           on tai.ServiceId equals us.ServiceId
                           join u in context.User
                           on us.UserId equals u.Id
                           where u.IsActive && us.IsActive && u.username == user && pc.PaymentClaimNumber == claimnumber
                           select tai.TransactionIdPK
                    ).FirstOrDefault();

                if (njm != null && njm != 0)
                {
                    return true;
                }

            }




            return false;
        }
    }
}