using AGV_WS.src.model;
using AGV_WS.src.utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace AGV_WS.src.common
{
    public class SessionManager
    {
        private ObservableCollection<AgvSession> _sessions;
        public ObservableCollection<AgvSession> Sessions { get { return _sessions; } }

        public static readonly SessionManager instance = new SessionManager();

        private SessionManager()
        {
            _sessions = new ObservableCollection<AgvSession>();
        }

        public void AddSession(AgvSession session)
        {
            foreach (AgvSession s in _sessions)
            {
                if (s.AgvId == session.AgvId)
                {
                    _sessions.Remove(s);
                }
            }

            _sessions.Add(session);

            print();
        }

        //public void RemoveSession(int agvId)
        //{
        //    foreach (AgvSessionOld session in _sessions)
        //    {
        //        if (session.AgvId == agvId)
        //        {
        //            session.WorkThread.Stop();
        //            _sessions.Remove(session);
        //        }
        //    }

        //    print();
        //}

        public void print()
        {
            Logger.Info("sessions {0}", _sessions.Count);
            foreach (AgvSession session in _sessions)
            {
                session.print();
            }
        }
    }
}
