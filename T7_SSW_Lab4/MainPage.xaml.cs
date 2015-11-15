using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace T7_SSW_Lab4
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        private readonly Schedule _schedule = new Schedule();
        private const int TestsCount = 100;

        // Colors for graphs
        private readonly List<Color> _colors = new List<Color>
        {
            Color.FromArgb(100, 153, 0, 0),
            Color.FromArgb(100, 51, 102, 0),
            Color.FromArgb(100, 0, 102, 102),
            Color.FromArgb(100, 0, 51, 102),
            Color.FromArgb(100, 76, 0, 153),
            Color.FromArgb(100, 153, 0, 176),
            Color.FromArgb(100, 32, 32, 32)
        };

        private int _colorIdx;


        public MainPage()
        {
            InitializeComponent();
        }

        private void ButtonGenerate_Click(object sender, RoutedEventArgs e)
        {
            // Generate points for graph
            var points = new List<Point>();
            for (var connectivity = 1; connectivity <= 100; connectivity += 5)
            {
                var possibility = .0;
                for (var testNumber = 0; testNumber < TestsCount; testNumber++)
                    if (_schedule.CheckConflict(Convert.ToInt16(SliderSize.Value), connectivity))
                        possibility++;
                possibility /= TestsCount;
                points.Add(new Point((int)(connectivity * CanvasGraph.RenderSize.Width * .01), (int)(CanvasGraph.RenderSize.Height - possibility * CanvasGraph.RenderSize.Height)));
            }

            // Draw graph
            var b = new SolidColorBrush(_colors[_colorIdx++ % _colors.Count]);
            for (var i = 0; i < points.Count - 1; i++)
            {
                var line = new Line
                {
                    Stroke = b,
                    X1 = points[i].X,
                    X2 = points[i + 1].X,
                    Y1 = points[i].Y,
                    Y2 = points[i + 1].Y,
                    StrokeThickness = 2
                };

                CanvasGraph.Children.Add(line);
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            _colorIdx = 0;
            CanvasGraph.Children.Clear();
        }

        private void CanvasGraph_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ButtonClear_Click(sender, null);
        }
    }
}

