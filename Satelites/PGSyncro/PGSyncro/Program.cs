using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using PGSyncro.Models;
using PGSyncro.EFData;
using PGSyncro.Utils;

namespace PGSyncro
{
     class Program
    {
        public static string appVersion = "PGSync v." + ConfigurationManager.AppSettings["versionapp"];
        public static int SPSTimeOutInMinutes = Convert.ToInt32(PGConfiguration.GetConfigBySetting("SPS_Timeout"));
        public static int NPSTimeOutInMinutes = Convert.ToInt32(PGConfiguration.GetConfigBySetting("NPS_Timeout"));
        public static string SPS_API_BASE = ConfigurationManager.AppSettings["SPS_API_BASE"];
        public static string SPS_API_TRANSACTION = ConfigurationManager.AppSettings["SPS_API_TRANSACTION"];
        public static long SincroId = 0;
        public static long TotalSincroFromUSP = 0;
        public static bool witherror = false;
        public static SyncroStats stats = new SyncroStats();
        static void Main()
        {
           
            try
            {



                    #region NuevoValidadorSPS

                                    List<SPSNewValidator> newValidators = new List<SPSNewValidator>();
                    string ServicesNewSPS = PGConfiguration.GetConfigBySetting("SPSAPIMethods");
                    if (ServicesNewSPS == null)
                    {

                    PGConfiguration.InsertLog("Main", "No existen servicios de nuevo DECIDIR configurados correctamente");
                }
                    else {
                        List<int> servicesSPS = ServicesNewSPS.Split(',').Select(Int32.Parse).ToList();
                        foreach (int serviceSPS in servicesSPS) {
                        SPSNewValidator nuevoValidador = new SPSNewValidator
                        {
                            ServiceID = serviceSPS,
                            publicAUTHkey = PGConfiguration.GetConfigBySetting("ApiKeyPublicServiceId_" + serviceSPS)
                        };
                        ;
                            newValidators.Add(nuevoValidador);
                        }
                        }
                    #endregion
                    #region Configuraciones de Validadores

                    List<ValidatorServiceConfig> validatorsConfigs = PGConfiguration.GetValidatorsSecurityConfiguration();
                #endregion

                List<GetTransactionsToSync_Result> transaccionesIncompletas = TransactionRepository.GetTransactionsToSync();

                if (!transaccionesIncompletas.Any())
                {
                    //No tiene elementos
                    PGConfiguration.InsertLog("Main", "Error general al obtner transacciones para actualizar");

                }
                else {

                    //Obtengo header
                    GetTransactionsToSync_Result header = transaccionesIncompletas.Where(ti => ti.TypeRow == 1).FirstOrDefault();
                    Program.SincroId = header.MonitorSyncroProcessId??0;
                    Program.TotalSincroFromUSP = header.MonitorSyncroProcessRecordsId ?? 0;

                    foreach (GetTransactionsToSync_Result transactionToWork in transaccionesIncompletas)
                    {
                        if (transactionToWork.TypeRow == 2) {

                            string hash = validatorsConfigs.Where(c => c.ValidatorId == transactionToWork.ValidatorId && c.ServiceId == transactionToWork.ServiceId).FirstOrDefault()?.HashKey;
                            bool spsnewValidator = false;
                            if (transactionToWork.ValidatorId == 3)
                            {
                                spsnewValidator = newValidators.Where(v => v.ServiceID == transactionToWork.ServiceId).Any();
                            }
                            //Comienzo proceso
                            long mid = transactionToWork.MonitorSyncroProcessRecordsId ?? Program.SincroId;
                            if (SyncroTransaction.MonitorBeginTransaction(mid))
                            {
                                //Proceso
                                SyncroProcess proc = SyncroTransaction.ProcesarTransaccion(transactionToWork, hash, spsnewValidator);
                                //Resuelvo resultado y cierro proceso
                                if (SyncroTransaction.MonitorCloseTransaction(mid, proc))
                                {
                                    //Se cerro OK
                                }
                                else
                                {
                                    //No se pudo cerrar
                                    PGConfiguration.InsertLog("Main", "No se pudo cerrar la transaccion");
                                }
                            }
                            else
                            {
                                //No se pudo comenzar proceso en BEGIN

                                PGConfiguration.InsertLog("Main", "No se pudo comenzar el inicio de la transaccion");
                            }
                        }

                    } //cierro foreach

                }


                SyncroTransaction.MonitorCloseProcess();



            }
            catch (Exception ex)
            {
                PGConfiguration.InsertLog("Main", "Error general en sistema de sincronizacion", ex);

                Console.WriteLine("Fallo general en sistema");
            }

        }
       
    }
}
