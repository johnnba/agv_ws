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
            agvs.Add(new Agv(2));
            agvs.Add(new Agv(3));

            return agvs;
        }

        public void SetCurrent(UInt64 agvId)
        {
            _current_agv_id = agvId;
        }

        public AgvInfo GetAgvInfo(UInt64 agvId)
        {
            return _agvs.FirstOrDefault<AgvInfo>(p => p.Id == agvId);
        }

        public void SetOnline(UInt64 agvId, bool online)
        {
            foreach (AgvInfo info in _agvs)
            {
                if (info.Id == agvId)
                {
                    info.Online = online;
                }
            }
        }

        public void SetAgvStatus(UInt64 agvId, UInt16 status)
        {
            foreach (AgvInfo info in _agvs)
            {
                if (info.Id == agvId)
                {
                    info.Status = status;
                }
            }
        }
        public void SetAgvPosition(UInt64 agvId, UInt16 cardId)
        {
            foreach (AgvInfo info in _agvs)
            {
                if (info.Id == agvId)
                {
                    info.PositionCardId = cardId;
                }
            }
        }
        public void SetAgvPlan(UInt64 agvId, UInt16 planId)
        {
            foreach (AgvInfo info in _agvs)
            {
                if (info.Id == agvId)
                {
                    info.CurrentPlanId = planId;
                }
            }
        }
        public void SetAgvUltraSonic(UInt64 agvId, UInt16 usonic)
        {
            foreach (AgvInfo info in _agvs)
            {
                if (info.Id == agvId)
                {
                    info.UltraSonic = usonic;
                }
            }
        }
        public void SetAgvSpeed(UInt64 agvId, UInt16 speed)
        {
            foreach (AgvInfo info in _agvs)
            {
                if (info.Id == agvId)
                {
                    info.Speed = speed;
                }
            }
        }
        public void SetAgvVoltage(UInt64 agvId, UInt16 valtage)
        {
            foreach (AgvInfo info in _agvs)
            {
                if (info.Id == agvId)
                {
                    info.Voltage = valtage;
                }
            }
        }
    }
}
