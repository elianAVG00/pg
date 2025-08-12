using PGDataAccess.Mappers;
using PGDataAccess.Models;
using PGDataAccess.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using PGDataAccess.EF;

namespace PGDataAccess.Repository
{
    public static class SecurityRepository
    {
        public static ValidatorServiceConfigModel GetValidatorConfigByServiceId(int serviceId, int validatorId)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    var vsconf = context.ValidatorServiceConfig.FirstOrDefault(sconf => sconf.ServiceId == serviceId && sconf.ValidatorId == validatorId);
                    return (vsconf != null) ? Mapper.ValidatorServiceConfig_EFToModel(vsconf) : null;
                }
            }
            catch (Exception ex)
            {
                ;
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        public static List<RolModel> GetRolesByUsername(string username)
        {
            try
            {
                var rolList = new List<RolModel>();

                using (var context = new PGDataEntities())
                {
                    var roles = from r in context.Rol
                                join ur in context.UserRol on r.Id equals ur.rolId
                                join u in context.User on ur.userId equals u.Id
                                where u.username == username && u.IsActive && r.IsActive && ur.IsActive
                                select r;

                    foreach (Rol rol in roles)
                    {
                        rolList.Add(Mapper.Rol_EFToModel(rol));
                    }
                }
                return rolList;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
      
            }
        }

        public static UserModel GetUserbyUsername(string username)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    var user = context.User.FirstOrDefault(u => u.username == username);
                    return Mapper.User_EFToModel(user);
                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, ex);
                throw new Exception("Ocurrió un error interno", ex.InnerException);
            }
        }

        public static UserModel GetUser(int userId)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    var user = context.User.FirstOrDefault(u => u.Id == userId);
                    return Mapper.User_EFToModel(user);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        public static List<ServiceModel> GetServicesByUser(int userId)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    List<ServiceModel> services = (
                        from userservice in context.UserService
                        join serv in context.Services
                        on userservice.ServiceId equals serv.ServiceId
                        where userservice.UserId == userId && serv.IsActive

                        select new ServiceModel {
                            ClientId = serv.ClientId,
                            Description = serv.Description,
                            IsActive = serv.IsActive,
                            MerchantId = serv.MerchantId,
                            Name = serv.Name,
                            ServiceId = serv.ServiceId,
                        }).ToList();
                    return services;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        public static int LoginUser(string username, string password)
        {
            string hashedPswd = CustomTool.HashCode(password);
            try
            {
                using (var context = new PGDataEntities())
                {
                    var user =
                        context.User.FirstOrDefault(
                            u => u.username == username && u.Pswd.pswd1 == hashedPswd && u.IsActive && u.Pswd.IsActive);
                    return (user != null) ? user.Id : 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        /// <summary>
        /// Comprueba si el usuario especificado tiene el servicio asociado por su merchantId
        /// Fecha estelar: 2017.05.04
        /// </summary>
        /// <param name="username">usuario</param>
        /// <param name="merchantId">merchant</param>
        /// <returns>1/0</returns>
        public static bool CanUserGetMerchantInfo(string username, string merchantId)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    LogRepository.InsertLogCommon(LogTypeModel.Info,"Parametros: username= " + username + " - merchantId = " + merchantId);

                    if (!string.IsNullOrWhiteSpace(username)) {
                    var canUserGet = (from serv in context.Services
                                          join userServ in context.UserService
                                              on serv.ServiceId equals userServ.ServiceId
                                          join users in context.User
                                              on userServ.UserId equals users.Id
                                              join servConf in context.ServicesConfig
                                              on serv.ServiceId equals servConf.ServiceId
                                          where 
                                          users.username == username 
                                          && users.IsActive 
                                          && userServ.IsActive 
                                          && servConf.IsPaymentSecured 
                                          && serv.MerchantId == merchantId
                                          && serv.IsActive 
                                          && servConf.IsActive
                                          select userServ.UserServiceId
                                          ).FirstOrDefault();

                    return (canUserGet != 0);
                    } else {

                        var canMerchantPayWOUser = (from serv in context.Services
                                              join servConf in context.ServicesConfig
                                              on serv.ServiceId equals servConf.ServiceId
                                          where 
                                          serv.MerchantId == merchantId
                                          && serv.IsActive 
                                          && servConf.IsActive
                                          && !servConf.IsPaymentSecured
                                          select serv.ServiceId
                                          ).FirstOrDefault();
                        return (canMerchantPayWOUser != 0);
                    
                    }
                }
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, nullex);
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }
    }
}