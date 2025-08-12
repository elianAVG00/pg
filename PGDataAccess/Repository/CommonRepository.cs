using PGDataAccess.EF;
using PGDataAccess.Mappers;
using PGDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;

namespace PGDataAccess.Repository
{
    public static class CommonRepository
    {

        public static LanguageModel GetLanguageByCode(string code)
        {
            LanguageModel langToReturn = new LanguageModel();

            try
            {
                using (var context = new PGDataEntities())
                {
                    langToReturn = (
                        from lang in context.Language
                        where lang.ISO3166.Contains(code) || lang.ISO6391.Contains(code) || lang.ISO6392.Contains(code)
                        select new LanguageModel
                        {
                            Id = lang.Id,
                            IsActive = lang.IsActive,
                            ISO3166 = lang.ISO3166,
                            ISO6391 = lang.ISO6391,
                            ISO6392 = lang.ISO6392,
                            NativeName = lang.NativeName
                        }
                        ).FirstOrDefault();

                    if (langToReturn == null)
                    {
                        langToReturn = (
                        from lang in context.Language
                        where lang.Id == 1
                        select new LanguageModel
                        {
                            Id = lang.Id,
                            IsActive = lang.IsActive,
                            ISO3166 = lang.ISO3166,
                            ISO6391 = lang.ISO6391,
                            ISO6392 = lang.ISO6392,
                            NativeName = lang.NativeName
                        }
                        ).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
      
            }
            return langToReturn;
        }

        public static List<ChannelModel> GetAllChannels()
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("");
                    LogRepository.InsertLogCommon(LogTypeModel.Info,
                                  string.Concat("Parametros: ", inputParameters));
                    List<ChannelModel> channels = new List<ChannelModel>();
                    var channelListEntity = context.Channels.ToList();
                    ChannelModel channel;
                    foreach (var channelEntity in channelListEntity)
                    {
                        channel = Mapper.Channel_EFToModel(channelEntity);
                        channels.Add(channel);
                    }
                    return channels;
                }
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, nullex);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        public static List<ValidatorModel> GetAllValidators()
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("");
                    LogRepository.InsertLogCommon(LogTypeModel.Info, 
                                  string.Concat("Parametros: ", inputParameters));
                    List<ValidatorModel> validators = new List<ValidatorModel>();
                    var validatorListEntity = context.Validators.ToList();
                    ValidatorModel validator;
                    foreach (var validatorEntity in validatorListEntity)
                    {
                        validator = Mapper.Validator_EFToModel(validatorEntity);
                        validators.Add(validator);
                    }
                    return validators;
                }
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, nullex);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
      
            }
        }

        public static ClientModel GetClienttByShortName(string shortName)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("shortName=", shortName);
                    LogRepository.InsertLogCommon(LogTypeModel.Info,
                                  string.Concat("Parametros: ", inputParameters));

                    var clientEntity = context.Clients.FirstOrDefault(c => c.ShortName == shortName);
                    return Mapper.Client_EFToModel(clientEntity);
                }
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, nullex);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
      
            }
        }

        public static ClientModel GetClienttById(int clientId)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("clientId=", clientId);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, 
                                  string.Concat("Parametros: ", inputParameters));

                    var clientEntity = context.Clients.FirstOrDefault(c => c.ClientId == clientId);
                    return Mapper.Client_EFToModel(clientEntity);
                }
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, nullex);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        public static CurrencyModel GetCurrencyByIso(string iso)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("iso=", iso);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, 
                                  string.Concat("Parametros: ", inputParameters));

                    var currencyEntity = context.Currency.FirstOrDefault(c => c.IsoCode == iso);
                    return (Mapper.Currency_EFToModel(currencyEntity));
                }
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, nullex);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
      
            }
        }

        public static List<CurrencyModel> GetCurrencies()
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("");
                    LogRepository.InsertLogCommon(LogTypeModel.Info, 
                                  string.Concat("Parametros: ", inputParameters));
                    List<CurrencyModel> currencies = new List<CurrencyModel>();
                    CurrencyModel currency;
                    var listCurrenciesEntities = context.Currency.ToList();
                    foreach (var currencyEntity in listCurrenciesEntities)
                    {
                        currency = Mapper.Currency_EFToModel(currencyEntity);
                        currencies.Add(currency);
                    }
                    return currencies;
                }
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, nullex);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        public static DocTypeModel GetDocTypeByShortName(string shortName)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("shortName=", shortName);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, "DataAccesService.GetDocTypeByShortName",
                                  string.Concat("Parametros: ", inputParameters), "");

                    DocType docTypeEntity = context.DocType.FirstOrDefault(d => d.ShortName == shortName);
                    return Mapper.DocType_EFToModel(docTypeEntity);
                }
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info,nullex);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        public static DocTypeModel GetDocTypeById(int docTypeId)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("docTypeId=", docTypeId);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, 
                                  string.Concat("Parametros: ", inputParameters));

                    DocType docTypeEntity = context.DocType.FirstOrDefault(d => d.DocTypeId == docTypeId);
                    return Mapper.DocType_EFToModel(docTypeEntity);
                }
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, nullex);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        public static bool CanUserGetTransaction(string username, string transactionId)
        {
            try
            {
                long transactionNumber = Tools.CustomTool.ConvertTransactionToTN(transactionId);
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("username=", username, ",transactionId=", transactionId);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, 
                                  string.Concat("Parametros: ", inputParameters), TransactionNumber: transactionId,logtransactionType: LogTransactionType.TransactionNumber);

                    User userEntity = context.User.FirstOrDefault(u => u.username == username && u.IsActive);
                    Transactions transactionEntity =
                        context.TransactionAdditionalInfo.FirstOrDefault(tai => tai.TransactionNumber == transactionNumber).Transactions;
                    EF.Services serviceEntity = context.Services.FirstOrDefault(s => s.MerchantId == transactionEntity.MerchantId);
                    if ((userEntity != null) && (serviceEntity != null))
                    {
                        UserService userServiceEntity =
                            context.UserService.FirstOrDefault(
                                us => us.UserId == userEntity.Id && us.ServiceId == serviceEntity.ServiceId && us.IsActive);
                        return (userServiceEntity != null);
                    }
                    else
                        return false;
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

        public static bool CanUserAccessToService(string username, string serviceName)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("username=", username, ",serviceName=", serviceName);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, 
                                  string.Concat("Parametros: ", inputParameters));

                    //lo transformo en un serviceName en caso de que me llege un merchantId

                    User userEntity = context.User.FirstOrDefault(u => u.username == username && u.IsActive);
                    EF.Services serviceEntity = context.Services.FirstOrDefault(s => s.Name == serviceName);
                    if ((userEntity != null) && (serviceEntity != null))
                    {
                        UserService userServiceEntity =
                            context.UserService.FirstOrDefault(
                                us => us.UserId == userEntity.Id && us.ServiceId == serviceEntity.ServiceId && us.IsActive);
                        return (userServiceEntity != null);
                    }
                    else
                        return false;
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

        public static List<ClientModel> GetClients()
        {
            List<ClientModel> clients = new List<ClientModel>();
            try
            {
                using (var context = new PGDataEntities())
                {
                    LogRepository.InsertLogCommon(LogTypeModel.Info, "DataAccesService.GetClients");
                    ClientModel client;

                    var listClientEntity = context.Clients.ToList();

                    if (listClientEntity.Count > 0)
                    {
                        foreach (var clientEntity in listClientEntity)
                        {
                            client = Mapper.Client_EFToModel(clientEntity);
                            clients.Add(client);
                        }
                    }
                }
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, nullex);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
            return clients;
        }

        public static List<ServiceModel> GetServices()
        {
            List<ServiceModel> services = new List<ServiceModel>();
            try
            {
                using (var context = new PGDataEntities())
                {
                    LogRepository.InsertLogCommon(LogTypeModel.Info, "DataAccesService.GetServices");
                    ServiceModel service;

                    var listServiceEntity = context.Services.Include("ServicesConfig").ToList();

                    if (listServiceEntity.Count > 0)
                    {
                        foreach (var serviceEntity in listServiceEntity)
                        {
                            try
                            {
                                service = Mapper.Service_EFToModel(serviceEntity);
                                services.Add(service);
                            }
                            catch (Exception)
                            {
                                LogRepository.InsertLogCommon(LogTypeModel.Error, string.Concat("Servicio ", serviceEntity.Name, " sin configuración"));
                               
                            }
                        }
                    }
                }
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, nullex);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
            return services;
        }

        public static ServiceConfigModel GetServiceConfigByServiceId(int id)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("id=", id);
                    LogRepository.InsertLogCommon(LogTypeModel.Info,  string.Concat("Parametros: ", inputParameters));

                    ServicesConfig serviceconfigEntity = (from serv in context.Services
                                                             join servconfig in context.ServicesConfig
                                                             on serv.ServiceId equals servconfig.ServiceId
                                                             where serv.ServiceId == id
                                                             select servconfig).FirstOrDefault();

                    context.Services.FirstOrDefault(s => s.ServiceId == id);
                    return (serviceconfigEntity != null) ? Mapper.ServiceConfig_EFToModel(serviceconfigEntity) : null;
                }
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, nullex);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }


        public static ServiceModel GetServiceById(int id)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("id=", id);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, string.Concat("Parametros: ", inputParameters));

                    var serviceEntity = context.Services.FirstOrDefault(s => s.ServiceId == id);
                    return (serviceEntity != null) ? Mapper.Service_EFToModel(serviceEntity) : null;
                }
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, nullex);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        public static ServiceModel GetServiceByMerchantId(string merchantId)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("merchantId=", merchantId);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, string.Concat("Parametros: ", inputParameters));

                    var serviceEntity = context.Services.FirstOrDefault(s => s.MerchantId == merchantId);
                    return (serviceEntity != null) ? Mapper.Service_EFToModel(serviceEntity) : null;
                }
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, nullex);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        public static ValidatorModel GetValidatorByCode(string validatorCode)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("validatorId=", validatorCode);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, string.Concat("Parametros: ", inputParameters));

                    var validatorEntity = context.Validators.FirstOrDefault(v => v.Name == validatorCode);
                    return Mapper.Validator_EFToModel(validatorEntity);
                }
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, nullex);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        public static ValidatorModel GetValidatorById(int validatorId)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("validatorId=", validatorId);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, string.Concat("Parametros: ", inputParameters));

                    var validatorEntity = context.Validators.FirstOrDefault(v => v.ValidatorId == validatorId);
                    return Mapper.Validator_EFToModel(validatorEntity);
                }
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, nullex);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        public static ProductModel GetProductById(int productId)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("productId=", productId);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, string.Concat("Parametros: ", inputParameters));

                    var productEntity = context.Products.FirstOrDefault(p => p.ProductId == productId);
                    return Mapper.Product_EFToModel(productEntity);
                }
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, nullex);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        public static List<ProductModel> GetAllProducts()
        {
            using (var context = new PGDataEntities())
            {
                List<ProductModel> products = new List<ProductModel>();
                ProductModel product;
                var listProductsEntities = context.Products.ToList();
                foreach (var productEntity in listProductsEntities)
                {
                    product = Mapper.Product_EFToModel(context.Products.FirstOrDefault(p => p.ProductId == productEntity.ProductId));
                    products.Add(product);
                }
                return products.OrderBy(c => c.Description).ToList();
            }
        }

        public static ChannelModel GetChannelByName(string channelName)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("channelName=", channelName);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, string.Concat("Parametros: ", inputParameters));

                    var channelEntity = context.Channels.FirstOrDefault(c => c.Name == channelName);
                    return (Mapper.Channel_EFToModel(channelEntity));
                }
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, nullex);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        public static void UpdateService(ServiceModel service)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("service.Id=", service.ServiceId);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, string.Concat("Parametros: ", inputParameters));

                    var originalService = context.Services.FirstOrDefault(s => s.ServiceId == service.ServiceId);
                    originalService = Mapper.Service_ModelToEF(originalService, service);
                    context.SaveChanges();
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                throw;
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, nullex);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        public static void UpdateClient(ClientModel client)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("service.Id=", client.ClientId);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, string.Concat("Parametros: ", inputParameters));
                    Clients clientEntity = context.Clients.FirstOrDefault(c => c.ClientId == client.ClientId);
                    clientEntity = Mapper.Client_ModelToEF(clientEntity, client);
                    context.SaveChanges();
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                throw;
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, nullex);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
      
            }
        }

        public static int? GetProductIdOfValidator(int productId, int validatorId)
        {
            using (var context = new PGDataEntities())
            {
                ProductsValidators productValidator = context.ProductsValidators.FirstOrDefault(pv => pv.ProductId == productId && pv.ValidatorId == validatorId);
                if (productValidator != null)
                    return productValidator.CardCode;
                else
                    return null;
            }
        }

        public static List<ProductModel> GetProductsByService(int serviceId)
        {
            using (var context = new PGDataEntities())
            {
                var productsList = (from prod in context.Products
                                    join conf in context.Configurations on prod.ProductId equals conf.ProductId
                                    where conf.ServiceId == serviceId && conf.IsActive == true
                                    select (
                                           new ProductModel
                                           {
                                                   ProductId = prod.ProductId,
                                                   ProductCode = prod.ProductCode,
                                                   Type = prod.Type,
                                                   Description = prod.Description,
                                                   IsActive = prod.IsActive,
                                                   CreatedBy = prod.CreatedBy,
                                                   CreatedOn = prod.CreatedOn,
                                                   UpdatedBy = prod.UpdatedBy,
                                                   UpdatedOn = prod.UpdatedOn,
                                               }
                                       )).OrderBy(c => c.Description).ToList<ProductModel>();

                return productsList;
            }

        }

        public static string GetAppConfig(string key)
        {
            using (var context = new PGDataEntities())
            {
                return context.AppConfig.FirstOrDefault(app => app.Setting == key).Value;
            }
        }

        public static void InsertClient(ClientModel client)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    LogRepository.InsertLogCommon(LogTypeModel.Info, "DataAccesService.InsertCliet");
                    client.CreatedOn = DateTime.Now;
                    client.CreatedBy = "WebAdmin";
                    context.Clients.Add(Mapper.Client_ModelToEF(client));
                    context.SaveChanges();

                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                throw;
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, nullex);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        public static List<ConfigurationModel> GetConfigurations()
        {

            using (var context = new PGDataEntities())
            {
                //List<EF.Configurations> configurations = context.Configurations.ToList();


                var configurations = (from c in context.Configurations
                                      join p in context.Products on c.ProductId equals p.ProductId
                                      join v in context.Validators on c.ValidatorId equals v.ValidatorId
                                      join ch in context.Channels on c.ChannelId equals ch.ChannelId
                                      join s in context.Services on c.ServiceId equals s.ServiceId
                                     
                                      select new ConfigurationModel
                                      {
                                          ConfigurationId = c.ConfigurationId,
                                          ChannelId = ch.ChannelId,
                                          ChannelName = ch.Name,
                                          ProductId = p.ProductId,
                                          ProductName = p.Description,
                                          ValidatorId = v.ValidatorId,
                                          ValidatorName = v.Name,
                                          ServiceId = c.ServiceId,
                                          ServiceName = s.Name,
                                          UniqueCode = c.UniqueCode,
                                          CommerceNumber = c.CommerceNumber,
                                          IsActive = c.IsActive
                                      }).OrderBy(c => c.ServiceName).ThenBy(c => c.ChannelName).ToList();


                return configurations;
            }
            


        }

        public static void InsertConfiguration(ConfigurationModel configuration)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    LogRepository.InsertLogCommon(LogTypeModel.Info, "DataAccesService.InsertConfiguration");

                    if (configuration.ChannelId == 0)
                        configuration.ChannelId = context.Channels.FirstOrDefault(ch => ch.Name == configuration.ChannelName).ChannelId;

                    if (configuration.ProductId == 0)
                        configuration.ProductId = context.Products.FirstOrDefault(p => p.Description == configuration.ProductName).ProductId;

                    if (configuration.ValidatorId == 0)
                        configuration.ValidatorId = context.Validators.FirstOrDefault(v => v.Name == configuration.ValidatorName).ValidatorId;

                    configuration.CreatedOn = DateTime.Now;
                    configuration.CreatedBy = "WebAdmin";
                    context.Configurations.Add(Mapper.Configuration_ModelToEF(configuration));
                    context.SaveChanges();

                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                throw;
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, nullex);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        public static void RemoveConfiguration(int configurationId)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    LogRepository.InsertLogCommon(LogTypeModel.Info, string.Concat("Parametros: ", configurationId));

                    var configuration = context.Configurations.FirstOrDefault(c => c.ConfigurationId == configurationId);
                    context.Configurations.Remove(configuration);
                    context.SaveChanges();

                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                throw;
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, nullex);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

    }
}