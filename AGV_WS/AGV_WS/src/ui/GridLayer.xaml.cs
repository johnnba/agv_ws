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
    /// GridLayer.xaml 的交互逻辑
    /// </summary>
    public partial class GridLayer : UserControl
    {
        private MapView _map;

        public GridLayer()
        {
            InitializeComponent();
        }
        public void SetMap(MapView indoorMap)
        {
            _map = indoorMap;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (_map == null)
            {
                return;
            }

            if (Properties.Settings.Default.ShowGrid)
            {
                DrawGrid(dc);
            }
            if (Properties.Settings.Default.ShowOrigin)
            {
                DrawOrigin(dc);
            }

        }


        public Point getOriginalPointInPixel()
        {
            return new Point(0, 0);
        }

        private void DrawGrid(DrawingContext dc)
        {
            if (_map == null)
            {
                return;
            }

            double cell = 3;

            Rect adjusted = new Rect(0 - cell, 0 - cell, ActualWidth + cell, ActualHeight + cell);


            //double left = _gridWidth * Math.Ceiling(adjusted.Left / _gridWidth);
            //double top = _gridHeight * Math.Ceiling(adjusted.Top / _gridHeight);

            SolidColorBrush brush = new SolidColorBrush((Color)System.Windows.Media.ColorConverter.ConvertFromString("#66414F63"));
            Pen pen = new Pen(brush, 1);
            pen.Freeze();  //冻结画笔，这样能加快绘图速度


            double gridwidth = Properties.Settings.Default.GridWidth * Properties.Settings.Default.PixelsPerMeter * _map.mapScaleTransform.ScaleX;
            double gridheight = Properties.Settings.Default.GridHeight * Properties.Settings.Default.PixelsPerMeter * _map.mapScaleTransform.ScaleY;

            cell = 3 * _map.mapScaleTransform.ScaleX;

            if (gridwidth > 0 && gridheight > 0)
            {
                Point originalPoint = getOriginalPointInPixel();
                originalPoint.Offset(_map.mapTranslateTransform.X, _map.mapTranslateTransform.Y);
                originalPoint = _map.mapScaleTransform.Transform(originalPoint);

                double left = originalPoint.X;
                if (left < adjusted.X)
                {
                    while (left < adjusted.X)
                    {
                        left += gridwidth;
                    }
                }
                else
                {
                    while (left > adjusted.X)
                    {
                        left -= gridwidth;
                    }
                }

                double top = originalPoint.Y;
                if (top < adjusted.Y)
                {
                    while (top < adjusted.Y)
                    {
                        top += gridheight;
                    }
                }
                else
                {
                    while (top > adjusted.Y)
                    {
                        top -= gridheight;
                    }
                }
                for (double y = top; y < adjusted.Bottom; y += gridheight)
                {
                    for (double x = left; x < adjusted.Right; x += gridwidth)
                    {
                        dc.DrawLine(pen, new Point(x, y - cell), new Point(x, y + cell));
                        dc.DrawLine(pen, new Point(x - cell, y), new Point(x + cell, y));
                    }
                }
            }
        }
        private void DrawOrigin(DrawingContext dc)
        {
            if (_map == null)
            {
                return;
            }

            Pen pen = new Pen(Brushes.Red, 2);

            Point originalPoint = getOriginalPointInPixel();
            originalPoint.Offset(_map.mapTranslateTransform.X, _map.mapTranslateTransform.Y);
            originalPoint = _map.mapScaleTransform.Transform(originalPoint);

            double radius = 6 * _map.mapScaleTransform.ScaleX;

            dc.DrawEllipse(null, pen, originalPoint, radius, radius);

            dc.DrawLine(pen, new Point(originalPoint.X, originalPoint.Y - radius), new Point(originalPoint.X, originalPoint.Y + radius));
            dc.DrawLine(pen, new Point(originalPoint.X - radius, originalPoint.Y), new Point(originalPoint.X + radius, originalPoint.Y));

        }
    }
}
