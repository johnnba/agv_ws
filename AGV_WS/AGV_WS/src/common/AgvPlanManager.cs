using AGV_WS.src.model;
using AGV_WS.src.utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace AGV_WS.src.common
{
    public class AgvPlanManager
    {
        private ObservableCollection<AgvPlan> _plans;
        private AgvPlanManager()
        {
            _plans = new ObservableCollection<AgvPlan>();

            //AgvCmd cmd = new AgvCmd();
            //cmd.type = 1;
            //cmd.value = 0;
            //AgvCmd cmd2 = new AgvCmd();
            //cmd.type = 2;
            //cmd.value = 1;
            //AgvCmd cmd3 = new AgvCmd();
            //cmd.type = 3;
            //cmd.value = 0;

            //AgvStep step = new AgvStep();
            //step.cardid = "S01";
            //step.cmdset = new ObservableCollection<AgvCmd>();
            //step.cmdset.Add(cmd);
            //step.cmdset.Add(cmd2);
            //step.cmdset.Add(cmd3);
            //AgvStep step2 = new AgvStep();
            //step.cardid = "S02";
            //AgvStep step3 = new AgvStep();
            //step.cardid = "S03";
            //AgvStep step4 = new AgvStep();
            //step.cardid = "S04";
            //AgvStep step5 = new AgvStep();
            //step.cardid = "S05";

            //AgvTask task = new AgvTask();
            //task.taskid = 1;
            //task.taskname = "task 01";
            //task.loop = 10;
            //task.stepset = new ObservableCollection<AgvStep>();
            //task.stepset.Add(step);
            //task.stepset.Add(step);
            //task.stepset.Add(step);
            //task.stepset.Add(step);
            //task.stepset.Add(step);

            //AgvPlan plan = new AgvPlan();
            //plan.Id = 1;
            //plan.Name = "plan 01";
            //plan.Tasks = new ObservableCollection<AgvTask>();
            //plan.Tasks.Add(task);

            //_plans.Add(plan);
        }
        public static readonly AgvPlanManager instance = new AgvPlanManager();

        public void AddPlan(AgvPlan plan)
        {
            foreach (AgvPlan p in _plans)
            {
                if (p.planid == plan.planid)
                {
                    _plans.Remove(p);
                }
            }
            _plans.Add(plan);
        }
        public AgvPlan GetPlan(int planId)
        {
            return _plans.FirstOrDefault<AgvPlan>(p => p.planid == planId);
        }
    }



}
