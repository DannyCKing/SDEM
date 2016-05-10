using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEMViewModels.Messages
{
    public interface IMessageParser
    {
        IMessageContent ParseMessage(string message);
    }
}
