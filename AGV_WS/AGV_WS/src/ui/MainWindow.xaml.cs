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

            //窗口最大化
            this.WindowState = WindowState.Maximized;
        }

        private void Init()
        {
            agvListView.ItemsSource = AgvInfoManager.instance.Agvs;

        }
        private void currentAgvChanged()
        {
            List<AgvPlan> plans = new List<AgvPlan>();
            if (AgvInfoManager.instance.CurrentAgv != null && AgvInfoManager.instance.CurrentAgv.CurrentPlan != null)
            {
                plans.Add(AgvInfoManager.instance.CurrentAgv.CurrentPlan);
            }
            agvPathTree.ItemsSource = plans;
        }

        private void mapViewer_Loaded(object sender, RoutedEventArgs e)
        {
            mapViewer.Init();
            AgvMap map = new AgvMap(1);
            mapViewer.SetMap(map);

            mapViewer.setAgvs();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(sender.GetType() == typeof(Button))
            {
                Button button = sender as Button;
                if (button != null)
                {
                    if (button.Name == "toolbarPlanView")
                    {
                        AgvPlanView.instance.Owner = this;
                        AgvPlanView.instance.Show();
                    }
                    else if (button.Name == "toolbarStart")
                    {
                        ServiceManager.instance.Start();
                    }
                    else if (button.Name == "toolbarStop")
                    {
                        ServiceManager.instance.Stop();
                    }
                    else if (button.Name == "toolbarExit")
                    {
                        ExitApp();
                    }
                }
            }
        }

        private void ExitApp()
        {
            if (System.Windows.MessageBox.Show(Properties.Resources.IDS_EXITPROMPT,
                                               Properties.Resources.IDS_APP,
                                                MessageBoxButton.YesNo,
                                                MessageBoxImage.Question,
                                                MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                Environment.Exit(0);
            }
            
        }

        private void agvListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AgvInfo agvinfo = agvListView.SelectedItem as AgvInfo;

            AgvInfoManager.instance.SetCurrent(agvinfo.Id);

            currentAgvChanged();

        }
    }
}
