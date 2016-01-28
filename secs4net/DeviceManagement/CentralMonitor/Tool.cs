using System.Collections.ObjectModel;
using System.ComponentModel;
using Cim.Services;
using System;

namespace CentralMonitor {
    sealed class Tool : INotifyPropertyChanged {
        public string Id { get; }
        string _Url;
        public string Url {
            get { return _Url; }
            set {
                if (value != _Url) {
                    _Url = value;
                    NotifyPropertyChanged("Url");
                }
            }
        }

        DateTime _regTime;
        public DateTime RegisiterTime {
            get { return _regTime; }
            set {
                if (value != _regTime) {
                    _regTime = value;
                    NotifyPropertyChanged("RegisiterTime");
                }
            }
        }

        public Tool(string id) {
            this.Id = id;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged(string name) {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
