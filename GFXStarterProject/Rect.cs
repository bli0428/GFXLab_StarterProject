using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace GFXStarterProject
{
    /// <summary>
    /// A simple rectangle node
    /// </summary>
    /// <remarks>
    /// This class was especially useful in testing most of my functionality and for debugging.
    /// </remarks>
    public class Rect : Transform
    {
        public Rectangle MyRect { get; set; }

        public Rect()
        {
            MyRect = new Rectangle
            {
                // Initialize the rectangle's dimensions
                Height = 100,
                Width = 100,
                // Set the rectangle in the center of the view
                Margin = new Thickness(
                    (OffsetX + ViewWidth / 2) / Zoom,
                    (OffsetY + ViewHeight / 2) / Zoom,
                    0, 0),
                // Set rectangle default color to black
                Fill = new SolidColorBrush(Windows.UI.Colors.Black)
            };
            // Allow touch, resize and drag responses for the rectangle
            AllowTouch(MyRect);
            AllowResize(MyRect);
            AllowTranslation(MyRect);
            // Add the rectangle to the Connector's dictionary
            Connector.Scales.Add(MyRect, 1);
        }

        // Change the color of the rectangle based on user input
        protected override void ChangeColor(Windows.UI.Color color)
        {
            MyRect.Fill = new SolidColorBrush(color);
        }
    }
}
