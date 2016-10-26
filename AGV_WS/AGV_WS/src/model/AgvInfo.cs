using AGV_WS.src.common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace AGV_WS.src.model
{
    public class AgvInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private UInt64 _id;
        private UInt16 _position_card_id;
        private UInt16 _status;
        private UInt16 _usonic;
        private UInt16 _speed;
        private UInt16 _voltage;
        private bool _online;
        private int _current_plan_id;
        
        public string Color { get { return _online ? "Black" : "Gray"; } }
        public UInt64 Id { get { return _id; } set { _id = value; } }
        public UInt16 PositionCardId { get { return _position_card_id; } set { _position_card_id = value; NotifyPropertyChanged("Position"); } }
        public UInt16 UltraSonic { get { return _usonic; } set { _usonic = value; NotifyPropertyChanged("UltraSonic"); } }
        public UInt16 Speed { get { return _speed; } set { _speed = value; NotifyPropertyChanged("Speed"); } }
        public UInt16 Voltage { get { return _voltage; } set { _voltage = value; NotifyPropertyChanged("Voltage"); NotifyPropertyChanged("PowerColor");  } }
        public string Position { get { return _position_card_id.ToString("D5"); } }
        public UInt16 Status { get { return _status; } set { _status = value; NotifyPropertyChanged("Status"); } }
        public bool Online { get { return _online; } set { _online = value; NotifyPropertyChanged("Online"); NotifyPropertyChanged("Color"); } }
        public int CurrentPlanId { get { return _current_plan_id; } set { _current_plan_id = value; } }
        public AgvPlan CurrentPlan { get { return AgvPlanManager.instance.GetPlan(_current_plan_id); } }


        public string PowerColor
        {
            get
            {
                if (_voltage <= 10)
                {
                    return "Red";
                }
                else if (_voltage <= 60)
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
