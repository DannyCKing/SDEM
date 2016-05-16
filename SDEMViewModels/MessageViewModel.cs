using System;

namespace SDEMViewModels
{
    public class MessageViewModel : NotifyPropertyChanged
    {
        private bool _IsPreviousMessageSameSender = false;

        private string _Sender;

        private DateTime _MessageDateStamp;

        private string _MessageContent;

        #region IsPreviousMessageSameSender

        public bool IsPreviousMessageSameSender
        {
            get
            {
                return _IsPreviousMessageSameSender;
            }
            set
            {
                if (value == _IsPreviousMessageSameSender)
                    return;

                _IsPreviousMessageSameSender = value;
                RaisePropertyChanged("IsPreviousMessageSameSender");
            }
        }

        #endregion

        #region MessageDateStamp

        public DateTime MessageDateStamp
        {
            get
            {
                return _MessageDateStamp;
            }
            set
            {
                if (value == _MessageDateStamp)
                    return;

                _MessageDateStamp = value;
                RaisePropertyChanged("MessageDateStamp");
            }
        }

        #endregion

        #region MessageContent

        public string MessageContent
        {
            get
            {
                return _MessageContent;
            }
            set
            {
                if (value == _MessageContent)
                    return;

                _MessageContent = value;
                RaisePropertyChanged("MessageContent");
            }
        }

        #endregion

        #region Sender

        public string Sender
        {
            get
            {
                return _Sender;
            }
            set
            {
                if (value == _Sender)
                    return;

                _Sender = value;
                RaisePropertyChanged("Sender");
            }
        }

        #endregion


        public MessageViewModel(string sender, string content, DateTime timestamp, bool isPreviousMessageSameSender)
        {
            Sender = sender;
            MessageContent = content;
            MessageDateStamp = timestamp;
            IsPreviousMessageSameSender = isPreviousMessageSameSender;
        }
    }
}
