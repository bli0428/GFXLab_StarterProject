using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace GFXStarterProject
{
    /// <summary>
    /// A simple textbox node.
    /// </summary>
    public class TextNode : Transform
    {
        public TextBox Node { get; set; }
        public TextNode()
        {
            // Initialize the textbox
            Node = new TextBox
            {
                AcceptsReturn = true,
                TextWrapping = TextWrapping.Wrap,
                MaxHeight = 172,
                Width = 300,
                // Center the textbox in the view
                Margin = new Thickness(
                    (OffsetX + ViewWidth / 2) / Zoom,
                    (OffsetY + ViewHeight / 2) / Zoom,
                    0, 0),
                PlaceholderText = "Add Notes Here"
            };
            // Scrollbar in case the user exceeds the bounds of the textbox
            ScrollViewer.SetVerticalScrollBarVisibility(Node, ScrollBarVisibility.Auto);
            // Allows responses to touch, resize, and drag events
            AllowTouch(Node);
            AllowResize(Node);
            AllowTranslation(Node);
            // Adds textbox to Connector's dictionary
            Connector.Scales.Add(Node, 1);
        }

        // Changes the color of the textbox border
        protected override void ChangeColor(Windows.UI.Color color)
        {
            Node.BorderBrush = new SolidColorBrush(color);
        }
    }
}
