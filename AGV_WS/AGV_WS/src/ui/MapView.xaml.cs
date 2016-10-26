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
using System.Windows.Threading;

namespace AGV_WS.src.ui
{
    /// <summary>
    /// MapView.xaml 的交互逻辑
    /// </summary>
    public partial class MapView : UserControl
    {
        private AgvMap _map;
        private DispatcherTimer _timer;
        private double updateSpeed = 100;
        private bool _isMouseLeftButtonDown = false;
        private Point _startPoint;

        public MapView()
        {
            InitializeComponent();

            MouseWheel += MapView_MouseWheel;
            MouseMove += MapView_MouseMove;
            MouseLeftButtonDown += MapView_MouseLeftButtonDown;
            MouseLeftButtonUp += MapView_MouseLeftButtonUp;

            _timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(updateSpeed) };
            _timer.Tick += new EventHandler(timer_Tick);
            _timer.Start();
        }

        private void MapView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isMouseLeftButtonDown = false;
        }

        private void MapView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Focus();

            _isMouseLeftButtonDown = true;
            _startPoint = e.GetPosition(null);
           
        }

        private void MapView_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseLeftButtonDown && e.LeftButton == MouseButtonState.Pressed)
            {
                Point p = e.GetPosition(null);

                mapTranslateTransform.X += (p.X - _startPoint.X) * (1 / mapScaleTransform.ScaleX);
                mapTranslateTransform.Y += (p.Y - _startPoint.Y) * (1 / mapScaleTransform.ScaleY);

                _startPoint = p;

                grid.InvalidateVisual();
            }
        }
        private void MapView_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double scale = e.Delta * 0.001;

            if (mapScaleTransform.ScaleX + scale > 0.3 && mapScaleTransform.ScaleY + scale > 0.3)
            {
                Point point = e.GetPosition(this);

                mapScaleTransform.CenterX = point.X;
                mapScaleTransform.CenterY = point.Y;

                mapScaleTransform.ScaleX += scale;
                mapScaleTransform.ScaleY += scale;

                grid.InvalidateVisual();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            refreshAgv();

            grid.InvalidateVisual();
        }

        private void refreshAgv()
        {
            if (_map != null)
            {
                foreach (AgvMarker marker in agvLayer.Children)
                {
                    AgvInfo agv = marker.AgvInfo;
                    if (agv.Online)
                    {
                        UInt16 cardId = agv.PositionCardId;
                        Point position;
                        if (_map.getCardPosition(cardId, out position))
                        {
                            marker.SetValue(Canvas.LeftProperty, position.X - marker.OffsetX);
                            marker.SetValue(Canvas.TopProperty, position.Y - marker.OffsetY);
                            marker.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        marker.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        public void SetMap(AgvMap map)
        {
            _map = map;

            bgMapLayer.Children.Clear();
            cardLayer.Children.Clear();

            if (_map != null)
            {
                var path = new Path();
                path.StrokeThickness = 3.0;
                path.Stroke = Brushes.Blue;

                var pathGeometry = new PathGeometry();
                foreach (AgvMapPath p in _map.Paths)
                {
                    Point point1 = new Point(p.Position1X, p.Position1Y);
                    var pathFigure = new PathFigure { StartPoint = point1 };
                    Point point2 = new Point(p.Position2X, p.Position2Y);
                    pathFigure.Segments.Add(new LineSegment { Point = point2 });
                    pathGeometry.Figures.Add(pathFigure);
                }
                path.Data = pathGeometry;
                bgMapLayer.Children.Add(path);

                foreach (AgvMapCard c in _map.Cards)
                {
                    CardMarker cm = new CardMarker(c.CardId);
                    cm.SetValue(Canvas.LeftProperty, (double)c.PositionX - cm.OffsetX);
                    cm.SetValue(Canvas.TopProperty, (double)c.PositionY - cm.OffsetY);
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

            _startPoint = new Point(mapTranslateTransform.X, mapTranslateTransform.Y);
            grid.SetMap(this);
        }
        public void setAgvs()
        {
            agvLayer.Children.Clear();

            foreach (AgvInfo agv in AgvInfoManager.instance.Agvs)
            {
                AgvMarker am = new AgvMarker(agv.Id);
                am.Visibility = Visibility.Collapsed;
                agvLayer.Children.Add(am);
            }

        }
    }
}


