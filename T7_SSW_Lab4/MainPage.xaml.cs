using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace T7_SSW_Lab4
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        private readonly Schedule _schedule = new Schedule();

        public MainPage()
        {
            InitializeComponent();
        }

        private void ButtonGenerate_Click(object sender, RoutedEventArgs e)
        {
            
            _schedule.GenerateMarix(10, Convert.ToInt16(SliderConnectivity.Value));

            TextBoxMatrix.Text = "";
            foreach (var row in _schedule.Matrix)
            {
                for (var j = 0; j < _schedule.Matrix.Count; j++)
                    TextBoxMatrix.Text += $"{row[j],4}";
                TextBoxMatrix.Text += "\n";
            }
        }

        private async void ButtonTransform_Click(object sender, RoutedEventArgs e)
        {
            _schedule.TransformMatrix();
            TextBoxTransformed.Text = "";
            foreach (var row in _schedule.Matrix)
            {
                for (var j = 0; j < _schedule.Matrix.Count; j++)
                    TextBoxTransformed.Text += $"{row[j],4}";
                TextBoxTransformed.Text += "\n";
            }
            var dialog = new MessageDialog(_schedule.CheckConflict().ToString());
            await dialog.ShowAsync();
        }
    }
}
