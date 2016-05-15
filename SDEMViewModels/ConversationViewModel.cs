using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SDEMViewModels.Global;
using SDEMViewModels.Messages;
using SDEMViewModels.Models;

namespace SDEMViewModels
{
    public class ConversationViewModel : NotifyPropertyChanged
    {
        #region Messages

        public ObservableCollection<MessageViewModel> _Messages;

        public ObservableCollection<MessageViewModel> Messages
        {
            get
            {
                if (_Messages == null)
                {
                    _Messages = new ObservableCollection<MessageViewModel>();
                }

                return _Messages;
            }
            set
            {
                if (_Messages == value)
                    return;

                _Messages = value;
                RaisePropertyChanged("Messages");
            }
        }

        #endregion

        #region SendMessage Command

        private ICommand _SendMessageCommand;
        public ICommand SendMessageCommand
        {
            get
            {
                return _SendMessageCommand;
            }
            set
            {
                _SendMessageCommand = value;
            }
        }
        #endregion

        #region User

        public ChatUser User { get; private set; }

        #endregion

        #region CurrentMessageText

        private string _CurrentMessage;

        public string CurrentMessage
        {
            get
            {
                return _CurrentMessage;
            }
            set
            {
                if (_CurrentMessage == value)
                    return;

                _CurrentMessage = value;
                RaisePropertyChanged("CurrentMessage");
            }
        }

        #endregion
        public ConversationViewModel(ChatUser user)
        {
            User = user;

            SendMessageCommand = new RelayCommand(p => true, SendMessage);
            if (Messages.Count == 0)
                Messages.Add(new MessageViewModel("SDEM Helper", "You have no chat history with this user.  Say something.", DateTime.Now));
        }

        public ConversationViewModel(ObservableCollection<MessageViewModel> messages)
        {
            _Messages = messages;
        }

        private void SendMessage(object param = null)
        {
            var directMessage = new DirectMessageContent(Settings.Instance.UserId, CurrentMessage);
            var xml = new DirectMessageCreator().CreateMessage(directMessage);

            User.TCPClient.Send(xml);

            // Clear out message after sending it
            CurrentMessage = "";
        }
    }
}
