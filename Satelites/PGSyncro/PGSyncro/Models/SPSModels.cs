using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGSyncro.Models
{
    public class SPSModels
    {


    }



    public class SPSPaymentResponseModel
    {
        public int limit { get; set; }
        public int offset { get; set; }
        public List<Result> results { get; set; }
        public bool hasMore { get; set; }
    }
    public class StatusDetails
    {
        public string ticket { get; set; }
        public string card_authorization_code { get; set; }
        public string address_validation_code { get; set; }
        public object error { get; set; }
    }

    public class Customer
    {
        public string id { get; set; }
        public string email { get; set; }
    }

    public class Identification
    {
        public string type { get; set; }
        public string number { get; set; }
    }

    public class CardHolder
    {
        public Identification identification { get; set; }
        public string name { get; set; }
    }

    public class CardData
    {
        public string card_number { get; set; }
        public CardHolder card_holder { get; set; }
    }

    public class Result
    {
        public int id { get; set; }
        public string site_transaction_id { get; set; }
        public int payment_method_id { get; set; }
        public string card_brand { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public string status { get; set; }
        public StatusDetails status_details { get; set; }
        public string date { get; set; }
        public Customer customer { get; set; }
        public string bin { get; set; }
        public int installments { get; set; }
        public object first_installment_expiration_date { get; set; }
        public string payment_type { get; set; }
        public List<object> sub_payments { get; set; }
        public string site_id { get; set; }
        public object fraud_detection { get; set; }
        public object aggregate_data { get; set; }
        public object establishment_name { get; set; }
        public object spv { get; set; }
        public object confirmed { get; set; }
        public object pan { get; set; }
        public object customer_token { get; set; }
        public string emv_issuer_data { get; set; }
        public string token { get; set; }
        public CardData card_data { get; set; }
    }
    public class SPSNewValidator { 
        public int ServiceID { get; set; }
        public string publicAUTHkey { get; set; }
    }

    public class SPSCommunicationAPI
    {
        public string LogRequest { get; set; }
        public string LogResponse { get; set; }
        public string returnValue { get; set; }
        public bool resultCanBeParsed { get; set; }
    }

    public class SPSXMLServiceResponse
    {



        public string Motivo { get; set; }

        public string Moneda { get; set; }

        public string Direccionentrega { get; set; }

        public string Validaciondomicilio { get; set; }

        public string CodigoPedido { get; set; }

        public string NombreEntrega { get; set; }

        public DateTime? FechaHora { get; set; }

        public string TelefonoComprador { get; set; }

        public string BarrioEntrega { get; set; }

        public string CodAutorizacion { get; set; }

        public string PaisEntrega { get; set; }

        public int? Cuotas { get; set; }

        public string ValidaFechaNac { get; set; }

        public string ValidaNroDoc { get; set; }

        public string Titular { get; set; }

        public string Pedido { get; set; }

        public string ZipEntrega { get; set; }

        public decimal? Monto { get; set; }

        public string Tarjeta { get; set; }

        public DateTime? FechaEntrega { get; set; }

        public string EmailComprador { get; set; }

        public string ValidaNroPuerta { get; set; }

        public string CiudadEntrega { get; set; }

        public string ValidaTipoDoc { get; set; }

        public string NOperacion { get; set; }

        public string EstadoEntrega { get; set; }

        public string Resultado { get; set; }

        public string MensajeEntrega { get; set; }

        public string ParamSitio { get; set; }

        public string TipoDoc { get; set; }

        public string TipoDocDescri { get; set; }

        public int? NroDoc { get; set; }

        public DateTime? FechaContraCargo { get; set; }

        public string MotivoContraCargo { get; set; }

        public string SiteContraCargo { get; set; }

        public string ResultadoAutenticacionVBV { get; set; }

        public string NroTarjetaVisible { get; set; }

        public string MotivoAdicional { get; set; }

        public string NroTicket { get; set; }

        public string IdMotivo { get; set; }
        /*
        private PropertyInfo[] _PropertyInfos = null;

        public override string ToString()
        {
            if (_PropertyInfos == null)
                _PropertyInfos = this.GetType().GetProperties();

            var sb = new StringBuilder();

            foreach (var info in _PropertyInfos)
            {
                var value = info.GetValue(this, null) ?? "(null)";
                sb.AppendLine(info.Name + ": " + value.ToString());
            }

            return sb.ToString();
        }
        */
    }

    public class SPSWebServiceRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Idsite { get; set; }
        public string Nrooperacion { get; set; }
        public string Importe { get; set; }
    }
}
