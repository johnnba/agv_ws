﻿using System;
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
    /// CardMarker.xaml 的交互逻辑
    /// </summary>
    public partial class CardMarker : UserControl
    {
        private int _id;
        public int OffsetX { get { return (int)bg.Width / 2; } }
        public int OffsetY { get { return (int)bg.Height / 2; } }
        public CardMarker(int id)
        {
            _id = id;
            InitializeComponent();

            labeltext.Text = _id.ToString("D5");
        }
    }
}
