using System;
using System.Xml.Linq;

namespace SDEMViewModels.Messages
{
    public class AliveMessageParser : IMessageParser
    {
        public IMessageContent ParseMessage(string message)
        {
            var xml = XDocument.Parse(message);
            var details = xml.Root.Element("MessageDetails");
            var username = details.Element("Username").Value.ToString();
            var senderId = Guid.Parse(details.Element("SenderId").Value.ToString());
            var ipAddress = details.Element("IPAddress").Value.ToString();
            var port = int.Parse(details.Element("Port").Value.ToString());
            var parsedMessage = new AliveMessageContent(ipAddress, port, senderId, username);

            return parsedMessage;
        }
    }
}
