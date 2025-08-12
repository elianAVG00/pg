using Microsoft.VisualStudio.TestTools.UnitTesting;
using PGSyncro;
using PGSyncro.EFData;
using PGSyncro.Models;
using PGSyncro.Utils;
using System;
using System.Collections.Generic;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GET_TRANSACTIONS_TO_SYNC()
        {
            List<GetTransactionsToSync_Result> transaccionesIncompletas = TransactionRepository.GetTransactionsToSync();
            Assert.IsTrue(transaccionesIncompletas != null);
        }
        [TestMethod]
        public void CHECK_MISSING_CODES()
        {
            DiagnosticResults result = new DiagnosticResults();
            List<TransactionOriginal> transactionsWithErrors = new List<TransactionOriginal>();
            List<InvalidCodes> invalidos = new List<InvalidCodes>();
            //Obtengo todas las transacciones missing
            List<GetTransactionsToSync_Result> transaccionesIncompletas = TransactionRepository.GetTransactionsToSync();
            string hash = "";
            bool spsnewValidator = false;
            result.TransactionsToSync = transaccionesIncompletas.Count;

            //Voy a los validadores
            foreach (GetTransactionsToSync_Result transactionToWork in transaccionesIncompletas)
            {
                if (transactionToWork.ValidatorId == 1)
                {
                    //NPS
                    result.NPS_Transactions++;
                    hash = "Ty4de2n06FJPG3ZtAa91vlmSujr0h70A42li5Zi9ozW215tBf0CYlxzdgqcOyaW2";
                }
                else {
                    //DECIDIR
                    if (transactionToWork.ServiceId == 10) {
                        //ES NEUVO ARBA
                        result.SPS_New_Transactions++;
                        hash = "e13f36cc7249476eab2a834acd996b9a";
                        spsnewValidator = true;
                    } else
                    {
                        result.SPS_Common_Transactions++;
                        hash = "";
                        spsnewValidator = false;
                    }
                }
                TransactionOriginal tos = SyncroTransaction.GetRespuestaDelValidador(transactionToWork, hash, spsnewValidator);
                if (tos.QueryResponse.HasError)
                {
                    result.WithError++;
                    transactionsWithErrors.Add(tos);
                } else {
                    if (!tos.QueryResponse.HasResponse)
                    {
                        result.WithoutResponse++;
                    }
                    else {
                        result.WithReponse++;
                        if (!tos.QueryResponse.HasTransaction)
                        {
                            result.WithoutTransaction++;
                        }
                        else {
                            result.WithTransaction++;
                            Status statusCode = TransactionRepository.GetStatusCodeIdByValidatorCode(transactionToWork.ValidatorId, tos.OriginalCode, tos.ModuleType);
                            if (statusCode == null)
                            {
                                //El codigo especificado no existe!
                                invalidos.Add(new InvalidCodes
                                {
                                    ModuleType = tos.ModuleType,
                                    OriginalCode = tos.OriginalCode,
                                    ValidatorId = transactionToWork.ValidatorId
                                });
                            }
                            else {
                                switch (statusCode.GenericCodeId) {
                                    case 1:
                                        result.Transactions_OK++;
                                        break;
                                    case 2:
                                        result.Transactions_NO++;
                                      
                                        break;
                                    case 3:
                                         result.Transactions_ERROR++;
                                        break;
                                    case 4:
                                        result.Transactions_NA++;
                                        break;
                                }
                            }
                        }
                    }

                

                   

                }
            }

            //Resuelvo resultados sin hacer cambios
            Assert.IsFalse(invalidos.Count > 0);

        }

        [TestMethod]
        public void DECIDIR_ANTIGUA_SIN_TID()
        {

            GetTransactionsToSync_Result transactionToWork = new GetTransactionsToSync_Result()
            {
                TransactionIdPK = 11752382,
                TransactionId = "908923329",
                TransactionNumber = 908923329,
                UniqueCode = "00030919",
                ValidatorId = 3,
                ServiceId = 27
            };


            SyncroProcess result = SyncroTransaction.ProcesarTransaccion(transactionToWork);
            Assert.IsFalse(result.Status.HasError);

        
        }

        [TestMethod]
        public void DECIDIR_ANTIGUA_OK()
        {
            GetTransactionsToSync_Result transactionToWork = new GetTransactionsToSync_Result()
            {
                TransactionIdPK = 11804630,
                TransactionId = "908975577",
                TransactionNumber = 908975577,
                UniqueCode = "00030919",
                ValidatorId = 3,
                ServiceId = 27
            };

            SyncroProcess result = SyncroTransaction.ProcesarTransaccion(transactionToWork);
            Assert.IsFalse(result.Status.HasError);
        }
        [TestMethod]
        public void DECIDIR_ANTIGUA_ERROR()
        {
            GetTransactionsToSync_Result transactionToWork = new GetTransactionsToSync_Result()
            {
                TransactionIdPK = 11854331,
                TransactionId = "909025278",
                TransactionNumber = 909025278,
                UniqueCode = "00030919",
                ValidatorId = 3,
                ServiceId = 27
            };

            SyncroProcess result = SyncroTransaction.ProcesarTransaccion(transactionToWork);
            Assert.IsFalse(result.Status.HasError);
        }
        [TestMethod]
        public void DECIDIR_ANTIGUA_NA()
        {
            GetTransactionsToSync_Result transactionToWork = new GetTransactionsToSync_Result()
            {
                TransactionIdPK = 5083585,
                TransactionId = "902383488",
                TransactionNumber = 902383488,
                UniqueCode = "00050215",
                ValidatorId = 3,
                ServiceId = 18
            };


            SyncroProcess result = SyncroTransaction.ProcesarTransaccion(transactionToWork);
            Assert.IsFalse(result.Status.HasError);
        }

        [TestMethod]
        public void DECIDIR_20_SIN_TID()
        {
            GetTransactionsToSync_Result transactionToWork = new GetTransactionsToSync_Result()
            {
                TransactionIdPK = 11736438,
                TransactionId = "908907385",
                TransactionNumber = 908907385,
                UniqueCode = "00240132",
                ValidatorId = 3,
                ServiceId = 10
            };



            SyncroProcess result = SyncroTransaction.ProcesarTransaccion(transactionToWork, "e13f36cc7249476eab2a834acd996b9a", true);
            Assert.IsFalse(result.Status.HasError);
        }

        [TestMethod]
        public void DECIDIR_20_OK()
        {
            GetTransactionsToSync_Result transactionToWork = new GetTransactionsToSync_Result()
            {
                TransactionIdPK = 11804119,
                TransactionId = "908975066",
                TransactionNumber = 908975066,
                UniqueCode = "00240132",
                ValidatorId = 3,
                ServiceId = 10
            };



            SyncroProcess result = SyncroTransaction.ProcesarTransaccion(transactionToWork, "e13f36cc7249476eab2a834acd996b9a", true);
            Assert.IsFalse(result.Status.HasError);
        }
        

                [TestMethod]
        public void NPS_SIN_TID()
        {
            GetTransactionsToSync_Result transactionToWork = new GetTransactionsToSync_Result()
            {
                CreatedOn = DateTime.Parse("2021-05-04 12:54:54.643"),
                TransactionIdPK = 11659003,
                TransactionId = "",
                TransactionNumber = 908849850,
                UniqueCode = "bapro_pic",
                ValidatorId = 1,
                ServiceId = 5
            };

            SyncroProcess result = SyncroTransaction.ProcesarTransaccion(transactionToWork, "Ty4de2n06FJPG3ZtAa91vlmSujr0h70A42li5Zi9ozW215tBf0CYlxzdgqcOyaW2");
            Assert.IsFalse(result.Status.HasError);
        }

        [TestMethod]
        public void NPS_NA()
        {
            GetTransactionsToSync_Result transactionToWork = new GetTransactionsToSync_Result()
            {
                CreatedOn = DateTime.Parse("2021-05-04 12:54:54.643"),
                TransactionIdPK = 10280639,
                TransactionId = "401313947",
                TransactionNumber = 907471486,
                UniqueCode = "bapro_atpre",
                ValidatorId = 1,
                ServiceId = 3
            };

            SyncroProcess result = SyncroTransaction.ProcesarTransaccion(transactionToWork, "Ty4de2n06FJPG3ZtAa91vlmSujr0h70A42li5Zi9ozW215tBf0CYlxzdgqcOyaW2");
            Assert.IsFalse(result.Status.HasError);
        }

        [TestMethod]
        public void DECIDIR_ERROR_CARPENDI()
        {
            GetTransactionsToSync_Result transactionToWork = new GetTransactionsToSync_Result()
            {
                CreatedOn = DateTime.Parse("2022-02-03 17:46:54.643"),
                TransactionIdPK = 12195420,
                TransactionId = "909366367",
                TransactionNumber = 909366367,
                UniqueCode = "00240132",
                ValidatorId = 3,
                ServiceId = 10
            };

            SyncroProcess result = SyncroTransaction.ProcesarTransaccion(transactionToWork, "e13f36cc7249476eab2a834acd996b9a", true);
            Assert.IsFalse(result.Status.HasError);
        }


    }
}
