using System;

namespace SDEMViewModels.Models
{
    public class ChatUser
    {
        public Status UserStatus { get; set; }

        public Guid UserId { get; set; }

        public string UserName { get; set; }
    }
}
