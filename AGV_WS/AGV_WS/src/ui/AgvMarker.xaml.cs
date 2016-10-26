using AGV_WS.src.common;
using AGV_WS.src.model;
using System;
using System.Collections.Generic;
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
    /// AgvMarker.xaml 的交互逻辑
    /// </summary>
    public partial class AgvMarker : UserControl
    {
        private UInt64 _agv_id;
        public int OffsetX { get { return (int)bg.Width / 2; } }
        public int OffsetY { get { return (int)bg.Height / 2; } }
        public AgvMarker(UInt64 agvId)
        {
            InitializeComponent();

            _agv_id = agvId;
            labeltext.Text = _agv_id.ToString("D5");
        }

        public AgvInfo AgvInfo { get { return AgvInfoManager.instance.GetAgvInfo(_agv_id); } }
    }
}
