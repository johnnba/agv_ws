using AGV_WS.src.utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AGV_WS.src.model
{
    public class AgvPlan : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private AgvPlan() { }
        public int Id { get; set; }
        public string Name { get; set; }
        public int TaskNum { get; set; } //待定
        public ObservableCollection<AgvTask> Tasks { get; set; }
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public string ToJson()
        {
            string json = JsonConvert.SerializeObject(this);

            Logger.Debug(json);

            return json;
        }

        public static AgvPlan FromJson(string jsonstr)
        {
            AgvPlan plan = JsonConvert.DeserializeObject<AgvPlan>(jsonstr);
            return plan;
        }

        public byte[] ToJsonBytes()
        {
            string json = JsonConvert.SerializeObject(this);

            return System.Text.Encoding.Default.GetBytes(json);
        }

    }
}
