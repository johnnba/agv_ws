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

            AgvCmd cmd = new AgvCmd();
            cmd.Key = 1;
            cmd.Value = 0;
            AgvCmd cmd2 = new AgvCmd();
            cmd.Key = 2;
            cmd.Value = 1;
            AgvCmd cmd3 = new AgvCmd();
            cmd.Key = 3;
            cmd.Value = 0;

            AgvStep step = new AgvStep();
            step.CardId = "S01";
            step.Cmds = new ObservableCollection<AgvCmd>();
            step.Cmds.Add(cmd);
            step.Cmds.Add(cmd2);
            step.Cmds.Add(cmd3);
            AgvStep step2 = new AgvStep();
            step.CardId = "S02";
            AgvStep step3 = new AgvStep();
            step.CardId = "S03";
            AgvStep step4 = new AgvStep();
            step.CardId = "S04";
            AgvStep step5 = new AgvStep();
            step.CardId = "S05";

            AgvTask task = new AgvTask();
            task.Id = 1;
            task.Name = "task 01";
            task.Loop = 10;
            task.Steps = new ObservableCollection<AgvStep>();
            task.Steps.Add(step);
            task.Steps.Add(step);
            task.Steps.Add(step);
            task.Steps.Add(step);
            task.Steps.Add(step);

            //AgvPlan plan = new AgvPlan();
            //plan.Id = 1;
            //plan.Name = "plan 01";
            //plan.Tasks = new ObservableCollection<AgvTask>();
            //plan.Tasks.Add(task);

            //_plans.Add(plan);
        }
        public static readonly AgvPlanManager instance = new AgvPlanManager();

        public AgvPlan GetPlan(int planId)
        {
            return _plans.FirstOrDefault<AgvPlan>(p => p.Id == planId);
        }
    }



}
