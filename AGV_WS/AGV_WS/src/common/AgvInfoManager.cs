using AGV_WS.src.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace AGV_WS.src.common
{
    public class AgvInfoManager
    {
        private ObservableCollection<AgvInfo> _agvs;

        private UInt64 _current_agv_id;

        public ObservableCollection<AgvInfo> Agvs { get { return _agvs; } }

        public AgvInfo CurrentAgv { get { return _agvs.FirstOrDefault<AgvInfo>(p => p.Id == _current_agv_id); } }

        public static readonly AgvInfoManager instance = new AgvInfoManager();
        private AgvInfoManager()
        {
            _agvs = new ObservableCollection<AgvInfo>();

            List<Agv> agvs = getAgvs();

            foreach (Agv agv in agvs)
            {
                AgvInfo info = new AgvInfo();
                info.Id = agv.Id;
                info.Online = false;
                _agvs.Add(info);
            } 
        }

        private List<Agv> getAgvs()
        {
            List<Agv> agvs = new List<Agv>();

            agvs.Add(new Agv(1));
            //agvs.Add(new Agv(2));
            //agvs.Add(new Agv(3));

            return agvs;
        }



        public void SetCurrent(UInt64 currentId)
        {
            _current_agv_id = currentId;
        }

    }
}
