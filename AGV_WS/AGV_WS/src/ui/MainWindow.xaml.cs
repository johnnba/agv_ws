using AGV_WS.src.common;
using AGV_WS.src.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AGV_WS.src.ui
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {

            ObservableCollection<AgvInfo> agvInfos = new ObservableCollection<AgvInfo>();
            agvListView.ItemsSource = agvInfos;

            AgvInfo info = new AgvInfo();
            info.Id = 1;
            info.Position = "L01 S01";
            info.Status = 2;
            info.Power = 60;
            AgvInfo info2 = new AgvInfo();
            info2.Id = 2;
            info2.Position = "L02 S02";
            info2.Status = 2;
            info2.Power = 90;
            AgvInfo info3 = new AgvInfo();
            info3.Id = 3;
            info3.Position = "L02 S03";
            info3.Status = 2;
            info3.Power = 10;


            agvInfos.Add(info);
            agvInfos.Add(info2);
            agvInfos.Add(info3);

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

            AgvPlan plan = new AgvPlan();
            plan.Id = 1;
            plan.Name = "plan 01";
            plan.Tasks = new ObservableCollection<AgvTask>();
            plan.Tasks.Add(task);

            List<AgvPlan> plans = new List<AgvPlan>();
            plans.Add(plan);
            agvPathTree.ItemsSource = plans;


        }

        private void mapViewer_Loaded(object sender, RoutedEventArgs e)
        {
            mapViewer.Init();
            mapViewer.setCards();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(sender.GetType() == typeof(Button))
            {
                Button button = sender as Button;
                if (button != null)
                {
                    if (button.Name == "toolbarStart")
                    {
                        ServiceManager.instance.Start();
                    }
                    else if (button.Name == "toolbarStop")
                    {
                        ServiceManager.instance.Stop();
                    }
                }
            }
            
        }
    }
}
