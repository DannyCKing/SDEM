
using System.Text;
using Crypt;
namespace SDEMViewModels.Global
{
    public class XMLUtils
    {
        public static string FormatXML(byte[] message)
        {
            var encoding = new UTF8Encoding(false);
            var messageAsString = encoding.GetString(message);
            // messageAsString = new PasswordConverter().Decrypt(messageAsString);

            messageAsString = messageAsString.Replace("\r\n", string.Empty);
            messageAsString = messageAsString.Replace("\0", string.Empty);
            return messageAsString;
        }

        public static string FormatXMLSecure(byte[] message)
        {
            var encoding = new UTF8Encoding(false);
            var messageAsString = encoding.GetString(message);
            messageAsString = messageAsString.Replace("\0", string.Empty);
            messageAsString = new PasswordConverter().Decrypt(messageAsString);

            messageAsString = messageAsString.Replace("\r\n", string.Empty);
            messageAsString = messageAsString.Replace("\0", string.Empty);
            return messageAsString;
        }
    }
}
