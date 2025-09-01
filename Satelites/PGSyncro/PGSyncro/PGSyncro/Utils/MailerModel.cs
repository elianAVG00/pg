using System.Collections.Generic;

namespace PGSyncro.Utils
{
    public class MailerModel
    {
        public List<string> To { get; set; }

        public List<string> CC { get; set; }

        public List<string> BCC { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public bool HTML { get; set; }

        public IDictionary<string, string> TemplateKeys { get; set; }

        public string FromAddress { get; set; }

        public string FromDisplayName { get; set; }

        public List<FileAttachment> Attachment { get; set; }

        public List<IncrustedResource> IncrustedResources { get; set; }
    }
}