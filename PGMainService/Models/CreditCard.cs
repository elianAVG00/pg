using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGMainService.Models
{
    public class CreditCard
    {
        /// <summary>
        /// Identificador de tarjeta crediticia
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre de la tarjeta crediticia
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Tipo de la tarjeta crediticia
        /// </summary>
        public string Type { get; set; }
    }
}