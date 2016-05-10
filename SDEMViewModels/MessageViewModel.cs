using System;

namespace SDEMViewModels
{
    public class MessageViewModel : NotifyPropertyChanged
    {
        private string _Sender;

        private DateTime _MessageDateStamp;

        private string _MessageContent;

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


        public MessageViewModel(string sender, string content, DateTime timestamp)
        {
            Sender = sender;
            MessageContent = content;
            MessageDateStamp = timestamp;
        }
    }
}
