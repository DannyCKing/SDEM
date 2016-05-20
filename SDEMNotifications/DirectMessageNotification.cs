
namespace SDEMNotifications
{
    public class DirectMessageNotification
    {
        private const string NOTIFICATION_XML = @"<toast>
                                                    <visual>
                                                        <binding template=""ToastImageAndText04"">
                                                            <image id=""1"" src=""{0}""/>
                                                            <text id=""1"">{1}</text>
                                                            <text id=""2"">{2}</text>
                                                        </binding>
                                                    </visual>
                                                  </toast>";


        public static string GetDirectNotificationXml(string senderName, string message)
        {
            return string.Format(NOTIFICATION_XML, Constants.NOTIFICATION_IMAGE_LOCATION,
                senderName, message);
        }
    }
}
