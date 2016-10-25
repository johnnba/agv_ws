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
    /// MapView.xaml 的交互逻辑
    /// </summary>
    public partial class MapView : UserControl
    {
        private AgvMap _map;

        public MapView()
        {
            InitializeComponent();
        }

        public void SetMap(AgvMap map)
        {
            _map = map;

            bgMapLayer.Children.Clear();

            if (_map != null)
            {
                var path = new Path();
                path.StrokeThickness = 3.0;
                path.Stroke = Brushes.Blue;

                var pathGeometry = new PathGeometry();
                foreach (AgvMapPath p in _map.Paths)
                {
                    var pathFigure = new PathFigure { StartPoint = p.Position1 };
                    pathFigure.Segments.Add(new LineSegment { Point = p.Position2 });
                    pathGeometry.Figures.Add(pathFigure);
                }
                path.Data = pathGeometry;
                bgMapLayer.Children.Add(path);

                foreach (AgvMapCard c in _map.Cards)
                {
                    CardMarker cm = new CardMarker(c.CardId);
                    cm.SetValue(Canvas.LeftProperty, c.Position.X - cm.OffsetX);
                    cm.SetValue(Canvas.TopProperty, c.Position.Y - cm.OffsetY);
                    //cm.Visibility = Visibility.Visible;
                    cardLayer.Children.Add(cm);
                }
            }
        }
        
        public void Init()
        {
            bgMapLayer.Children.Clear();
            cardLayer.Children.Clear();
            agvLayer.Children.Clear();
        }
        public void setAgvs()
        {
            agvLayer.Children.Clear();

            double[] pntX = new double[] { 300, 600, 1100, 1200, 1200, 1100, 700, 300, 200, 200 };
            double[] pntY = new double[] { 100, 100, 100, 200, 400, 500, 500, 500, 400, 200 };
            string[] label = new string[] { "AGV01", "AGV02", "AGV03", "AGV04", "AGV05", "AGV06", "AGV07", "AGV08", "AGV09", "AGV10" };

            int i = 1;

            AgvMarker am = new AgvMarker(label[i]);
            am.SetValue(Canvas.LeftProperty, pntX[i] - am.OffsetX);
            am.SetValue(Canvas.TopProperty, pntY[i] - am.OffsetY);

            agvLayer.Children.Add(am);
        }
    }
}


