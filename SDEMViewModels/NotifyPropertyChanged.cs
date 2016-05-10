using System.ComponentModel;

namespace SDEMViewModels
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
