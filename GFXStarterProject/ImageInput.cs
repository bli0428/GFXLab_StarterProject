using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace GFXStarterProject
{
    /// <summary>
    /// A class that stores an Image object and 
    /// </summary>
    internal class ImageInput : Transform
    {
        public Image UploadImage { get; set; }

        /// <summary>
        /// Initializes an image from a given fileStream
        /// </summary>
        /// <param name="fileStream">The source from within the files where the image was
        /// uploaded </param>
        public ImageInput(Windows.Storage.Streams.IRandomAccessStream fileStream)
        {
            // Initialize the image once the user has picked an image from file
            var bitMap = new BitmapImage();
            bitMap.SetSource(fileStream);
            var rectGeo = new Windows.UI.Xaml.Media.RectangleGeometry();
            var rect = new Windows.Foundation.Rect();
            rect.X = 100;
            rect.Y = 100;
            rect.Width = 500;
            rect.Height = 500;
            rectGeo.Rect = rect;
            UploadImage = new Image
            {
                Source = bitMap,
                // Set the Image in the center of the view
                Margin = new Thickness(
                    (OffsetX + ViewWidth / 2) / Zoom,
                    (OffsetY + ViewHeight / 2) / Zoom,
                    0, 0),
                Clip = rectGeo
            };

            // Allows the image to respond to touch, pinch, and drag events
            AllowTouch(UploadImage);
            AllowResize(UploadImage);
            AllowTranslation(UploadImage);

            // Add the image to the Connector's scale dictionary
            Connector.Scales.Add(UploadImage, 1);
        }
        
        //Do nothing in this case
        protected override void ChangeColor(Windows.UI.Color color)
        { 
        }
    }
}
