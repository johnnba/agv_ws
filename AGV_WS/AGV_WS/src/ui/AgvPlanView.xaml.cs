using AGV_WS.src.common;
using AGV_WS.src.model;
using AGV_WS.src.utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AGV_WS.src.ui
{
    /// <summary>
    /// AgvPlanView.xaml 的交互逻辑
    /// </summary>
    public partial class AgvPlanView : Window
    {
        private AgvPlan _plan;
        private AgvPlanView()
        {
            InitializeComponent();
            setSessionView();
        }
        public static readonly AgvPlanView instance = new AgvPlanView();

        private void setSessionView()
        {
            if (SessionManager.instance.Sessions.Count == 0)
            {
                sessionListView.ItemsSource = null;
            }
            else
            {
                sessionListView.ItemsSource = SessionManager.instance.Sessions;
            }
        }
        private void openFile()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = @"c:\";
            dlg.RestoreDirectory = true;
            dlg.Title = "Select Json File";
            dlg.Filter = "Json Files (*.json;) | *.json;";
            dlg.ShowDialog();

            if (File.Exists(dlg.FileName))
            {
                FileStream fsRead = System.IO.File.OpenRead(dlg.FileName);

                int fsLen = (int)fsRead.Length;
                byte[] heByte = new byte[fsLen];
                int r = fsRead.Read(heByte, 0, heByte.Length);

                string json = System.Text.Encoding.UTF8.GetString(heByte);
                Logger.Debug(json);
                setPlan(json);

            }
        }
        private void setPlan(string json)
        {
            _plan = AgvPlan.FromJson(json);

            List<AgvPlan> plans = new List<AgvPlan>();
            plans.Add(_plan);

            agvPathTree.ItemsSource = plans;
        }

        private void sendPlan()
        {
            if (sessionListView.SelectedItem != null)
            {
                AgvSession session = sessionListView.SelectedItem as AgvSession;

                if (_plan != null)
                {
                    session.SendPlan(_plan);
                }
                else
                {
                    MessageBox.Show(Properties.Resources.IDS_PLANISNULL);
                }
                
            }
            else
            {
                MessageBox.Show(Properties.Resources.IDS_SELECTAGV);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender.GetType() == typeof(Button))
            {
                Button button = sender as Button;
                if (button != null)
                {
                    if (button.Name == "toolbarOpen")
                    {
                        openFile();
                    }
                    else if (button.Name == "buttonUpdate")
                    {
                        setSessionView();
                    }
                    else if (button.Name == "buttonSend")
                    {
                        sendPlan();
                    }
                }
            }
        }
    }
}
