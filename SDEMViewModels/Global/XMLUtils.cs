
namespace SDEMViewModels.Global
{
    public class XMLUtils
    {
        public static string FormatXML(byte[] message)
        {
            var messageAsString = System.Text.Encoding.UTF8.GetString(message);
            messageAsString = messageAsString.Replace("\r\n", string.Empty);
            messageAsString = messageAsString.Replace("\0", string.Empty);
            return messageAsString;
        }
    }
}
