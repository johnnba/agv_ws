using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AGV_WS.src.model
{
    public class AgvTask : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int taskid { get; set; }
        public string taskname { get; set; }
        public int loop { get; set; }
        public int stepnum { get; set; } //待定
        public ObservableCollection<AgvStep> stepset { get; set; }

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
