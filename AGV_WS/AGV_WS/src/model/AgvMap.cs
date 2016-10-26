using AGV_WS.src.utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace AGV_WS.src.model
{
    public class AgvMapPath : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public int PathId { get; set; }
        public int Position1X { get; set; }
        public int Position1Y { get; set; }
        public int Position2X { get; set; }
        public int Position2Y { get; set; }
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }

    public class AgvMapCard : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public int CardId { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }

    public class AgvMap : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public int Id { get; set; }
        public ObservableCollection<AgvMapPath> Paths { get; set; }
        public ObservableCollection<AgvMapCard> Cards { get; set; }
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public bool getCardPosition(UInt16 cardId, out Point point)
        {
            foreach (AgvMapCard card in Cards)
            {
                if (card.CardId == cardId)
                {
                    point = new Point(card.PositionX, card.PositionY);
                    return true;
                }
            }
            point = new Point();
            return false;
        }

    }
}
