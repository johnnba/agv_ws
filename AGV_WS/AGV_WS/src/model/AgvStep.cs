using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AGV_WS.src.model
{
    public class AgvStep : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string cardid { get; set; }
        public int cmdnum { get; set; }
        public ObservableCollection<AgvCmd> cmdset{ get; set; }

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
