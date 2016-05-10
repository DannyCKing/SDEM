using System;

namespace SDEMViewModels.Models
{
    public class Message
    {
        public string Sender { get; set; }

        public DateTime MessageDateStamp { get; set; }

        public string MessageContent { get; set; }
    }
}
