using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGMainService.Models
{
    public class AppConfigModel
    {
        public int ConfigId { get; set; }

        public string Setting { get; set; }

        public string Value { get; set; }

        public string ConfigType { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public DateTime? DeleteTime { get; set; }

        public int? ViewOrderInWebAdmin { get; set; }

        public string ViewSpecialBackgroundColor { get; set; }
    }
}