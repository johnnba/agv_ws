using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AGV_WS.src.model
{
    public class AgvCmd : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int Key { get; set; }

        public int Value { get; set; }

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
