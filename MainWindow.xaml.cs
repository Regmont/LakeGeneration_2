using System.Windows;
using System.Windows.Input;

namespace LakeGeneration_2
{
    public partial class MainWindow : Window
    {
        private Renderer renderer;

        public MainWindow()
        {
            InitializeComponent();

            KeyDown += MainWindow_KeyDown;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int actualWidth = (int)ActualWidth;
            int actualHeight = (int)ActualHeight;

            renderer = new Renderer();
            displayImage.Source = renderer.InitializeBitmap(actualWidth, actualHeight);

            Lake lake = new Lake(200, 500, 200, 500, 0.1f, 0.05f);
            Generator generator = new Generator(actualWidth, actualHeight, lake);

            renderer.DrawDepthGrid(generator.GetGrid());
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
