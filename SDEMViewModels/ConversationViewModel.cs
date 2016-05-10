using System.Collections.ObjectModel;

namespace SDEMViewModels
{
    public class ConversationViewModel : NotifyPropertyChanged
    {
        #region

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

        public ConversationViewModel()
        {
        }

        public ConversationViewModel(ObservableCollection<MessageViewModel> messages)
        {
            _Messages = messages;
        }
    }
}
