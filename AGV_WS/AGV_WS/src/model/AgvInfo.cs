using AGV_WS.src.common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AGV_WS.src.model
{
    public class AgvInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private UInt64 _id;
        private string _position;
        private int _status;
        private int _power;
        private bool _online;
        private int _current_plan_id;

        public UInt64 Id { get { return _id; } set { _id = value; } }
        public string Position { get { return _position; } set { _position = value; } }
        public int Status { get { return _status; } set { _status = value; } }
        public int Power { get { return _power; } set { _power = value; } }
        public bool Online { get { return _online; } set { _online = value; } }
        public int CurrentPlanId { get { return _current_plan_id; } set { _current_plan_id = value; } }
        public AgvPlan CurrentPlan { get { return AgvPlanManager.instance.GetPlan(_current_plan_id); } }


        public string Color
        {
            get
            {
                if (_power <= 10)
                {
                    return "Red";
                }
                else if (_power <= 60)
                {
                    return "Orange";
                }
                else
                {
                    return "Green";
                }
            }
        }
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
