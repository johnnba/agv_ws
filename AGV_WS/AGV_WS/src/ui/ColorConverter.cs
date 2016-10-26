using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace AGV_WS.src.ui
{
  
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                if (value.ToString().CompareTo("Gray") == 0)
                {
                    return Brushes.Gray;
                }
                else if (value.ToString().CompareTo("LightGray") == 0)
                {
                    return Brushes.LightGray;
                }
                else if (value.ToString().CompareTo("Green") == 0)
                {
                    return Brushes.Green;
                }
                else if (value.ToString().CompareTo("LightGreen") == 0)
                {
                    return Brushes.LightGreen;
                }
                else if (value.ToString().CompareTo("Blue") == 0)
                {
                    return Brushes.Blue;
                }
                else if (value.ToString().CompareTo("LightBlue") == 0)
                {
                    return Brushes.LightBlue;
                }
                else if (value.ToString().CompareTo("Red") == 0)
                {
                    return Brushes.Red;
                }
                else if (value.ToString().CompareTo("Orange") == 0)
                {
                    return Brushes.Orange;
                }
                else if (value.ToString().CompareTo("White") == 0)
                {
                    return Brushes.White;
                }
                else if (value.ToString().CompareTo("LightYellow") == 0)
                {
                    return Brushes.LightYellow;
                }
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
