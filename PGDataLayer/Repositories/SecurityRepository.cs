using PGDataLayer.EF;
using PGDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGDataLayer.Repositories
{
    public static class SecurityRepository
    {
        public static bool CanUserGetMerchantInfo(MerchantUserInfoInput input)
        {
            try
            {
                using (var _context = new PaymentGatewayEntities()) 
                {
                    var service = _context.Services.FirstOrDefault(s => s.MerchantId == input.merchantId); 
                    if (service == null || !service.IsActive)
                    {
                        return false;
                    }

                    var serviceConfig = _context.ServicesConfig.FirstOrDefault(sc => sc.ServiceId == service.ServiceId); 
                    if (serviceConfig == null || !serviceConfig.IsActive)
                    {
                        return false;
                    }

                    if (string.IsNullOrWhiteSpace(input.username))
                    {
                        return !serviceConfig.IsPaymentSecured;
                    }

                    bool isUserAssociatedWithService = (from us in _context.UserService
                                                        join u in _context.User on us.UserId equals u.Id
                                                        where us.IsActive && us.ServiceId == service.ServiceId &&
                                                              u.IsActive && u.username == input.username 
                                                        select us).Any();

                    //DEBE haber un usuario y DEBE estar asegurado.
                    return isUserAssociatedWithService && serviceConfig.IsPaymentSecured;
                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLog(new LogModel()
                {
                    exception = ex,
                    message = "Error validando permisos para obtener información del Merchant", // Mensaje de error más específico
                    module = "PGDataLayer/SecurityRepository/CanUserGetMerchantInfo",
                    Type = LogType.Exception
                });
                return false;
            }
        }

    }
}