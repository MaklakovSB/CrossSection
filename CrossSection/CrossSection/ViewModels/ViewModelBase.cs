using CrossSection.Interfaces;
using System;
using System.ComponentModel;

namespace CrossSection.ViewModels
{
    public class ViewModelBase : IViewModel, INotifyPropertyChanged
    {
        #region Имплементация IViewModel

        public event Action Close;

        public virtual void OnClose()
        {
            Close?.Invoke();
        }

        #endregion

        #region Имплементация INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
