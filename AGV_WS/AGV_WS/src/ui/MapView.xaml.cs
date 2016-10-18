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
        public MapView()
        {
            InitializeComponent();
        }

        public void Init()
        {
            bgMapLayer.Children.Clear();
            planLayer.Children.Clear();
            cardLayer.Children.Clear();
            agvLayer.Children.Clear();
        }

        public void setMap()
        {

        }
        public void setCards()
        {
            cardLayer.Children.Clear();

            double[] pntX = new double[] { 300, 600, 1100, 1200, 1200, 1100, 700, 300, 200, 200 };
            double[] pntY = new double[] { 100, 100, 100, 200, 400, 500, 500, 500, 400, 200 };

            var path = new Path();
            path.StrokeThickness = 3.0;
            path.Stroke = Brushes.Blue;


            var pathGeometry = new PathGeometry();

            for (int i = 0; i < 10; i++)
            {
                CardMarker cm = new CardMarker();
                cm.SetValue(Canvas.LeftProperty, pntX[i] - cm.OffsetX);
                cm.SetValue(Canvas.TopProperty, pntY[i] - cm.OffsetY);

                cm.Visibility = Visibility.Visible;
                cardLayer.Children.Add(cm);

                var pathFigure = new PathFigure { StartPoint = new Point(pntX[i], pntY[i]) };

                double pointX, pointY;
                if (i < 10 - 1)
                {
                    pointX = pntX[i + 1];
                    pointY = pntY[i + 1];

                }
                else
                {
                    pointX = pntX[0];
                    pointY = pntY[0];
                }
                if (i == 2 || i == 4 || i == 7 || i == 9)
                {
                    pathFigure.Segments.Add(new ArcSegment(new Point(pointX, pointY), new Size(100.0, 100.0), 0, false, SweepDirection.Clockwise, true));
                }
                else
                {
                    pathFigure.Segments.Add(new LineSegment { Point = new Point(pointX, pointY) });
                }

                pathGeometry.Figures.Add(pathFigure);

            }

            path.Data = pathGeometry;
            
            planLayer.Children.Add(path);

        }
        public void setAgvs()
        {
        }
    }
}


