using System;
using TetrisInterfaces;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace TetrisWPF.ViewModel
{
    internal static class View
    {
        private const byte StrokeThickness = 1;
        private const byte SizeRectangle = 25;
        private const byte WidthHeightNFigureBoard = 4;
        private const float Opac = 1f;
        private static readonly SolidColorBrush BorderColor = new SolidColorBrush(Colors.Black);

        public static SolidColorBrush GetColor(TColor source)
        {
            SolidColorBrush color;
            switch (source)
            {
                case TColor.BlueViolet:
                    color = new SolidColorBrush(Colors.BlueViolet);
                    break;
                case TColor.Brown:
                    color = new SolidColorBrush(Colors.Brown);
                    break;
                case TColor.Green:
                    color = new SolidColorBrush(Colors.Green);
                    break;
                case TColor.Orange:
                    color = new SolidColorBrush(Colors.DarkOrange);
                    break;
                case TColor.Pink:
                    color = new SolidColorBrush(Colors.DeepPink);
                    break;
                case TColor.Red:
                    color = new SolidColorBrush(Colors.Red);
                    break;               
              default:
                  color = new SolidColorBrush(Colors.Black);
                  break;
            }
            return color;
        }



        public static Rectangle CreateRectangle(SolidColorBrush color, byte j, byte i)
        {
            Rectangle rectangle = new Rectangle()
            {
                StrokeThickness = StrokeThickness,
                Stroke = BorderColor,
                Width = SizeRectangle,
                Height = SizeRectangle,
                Opacity = Opac
            };

            rectangle.Fill = color;
            rectangle.SetValue(Canvas.LeftProperty, j * (double)SizeRectangle);
            rectangle.SetValue(Canvas.TopProperty, i * (double)SizeRectangle);
            return rectangle;
        }
    }
}
