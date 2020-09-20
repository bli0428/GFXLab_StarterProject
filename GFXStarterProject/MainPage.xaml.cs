using System;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GFXStarterProject
{
    /// <summary>
    /// Welcome to my project! Just a quick rundown of the features:
    /// In the bar across the top, the user can add textboxes, rectangles, images, and videos.
    /// In addition, when the link button is clicked, then if two nodes are tapped, a line
    /// will be drawn to connect the two nodes.
    /// There also is a delete button which allows the user to remove nodes, and the colors button
    /// allows the user to tap nodes to change their colors.
    /// </summary>
    /// <remarks>
    /// The MainPage class contains the command bar click handlers
    /// </remarks>
    public sealed partial class MainPage
    {
        public static MainPage Main;
        public static ScrollViewer MyScrollViewer;
        public WorkCanvas Workspace;
        
        // Several booleans for buttons that are clicked
        public static bool Delete { get; set; }
        public static bool IsLink { get; set; }
        public static bool ColorChange { get; set; }

        // Default color
        public static Color FillColor = Colors.Black;

        /// <summary>
        /// In here I initialize the commandbar, the canvas, and the scrollviewer
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            Workspace = new WorkCanvas();
            GridView.Children.Add(Workspace);
            MyScrollViewer = Workspace.GetScrollViewer();
            Main = this;

        }

        // Adds a textbox
        private void AddText_Click(object sender, RoutedEventArgs e)
        {
            var text = new TextNode();
            Workspace.GetCanvas().Children.Add(text.Node);
        }

        // Adds a rectangle
        private void AddRect_Click(object sender, RoutedEventArgs e)
        {
            var rect = new Rect();
            Workspace.GetCanvas().Children.Add(rect.MyRect);
        }

        // Opens up the dropdown menu for color
        private void Color_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            Color.Label = "Pick a color";
            FlyoutBase.ShowAttachedFlyout((FrameworkElement) sender);
        }

        // A handler for all clicks on the color menu
        private void ChangeColor_Click(object sender, RoutedEventArgs e)
        {
            var color = sender as MenuFlyoutItem;

            if (color == null) return;
            var colorTag = (string) color.Tag;

            // Set color change to true so that multiple elements can be changed
            ColorChange = true;

            switch (colorTag)
            {
                case "Red":
                {
                    FillColor = Colors.Red;
                    Color.Label = "Red Selected";
                    break;
                }
                case "Blue":
                {
                    FillColor = Colors.Blue;
                    Color.Label = "Blue Selected";
                    break;
                }
                case "Green":
                {
                    FillColor = Colors.Green;
                    Color.Label = "Green Selected";
                    break;
                }
                case "Orange":
                {
                    Color.Label = "Orange Selected";
                    FillColor = Colors.Orange;
                    break;
                }
                case "Yellow":
                {
                    FillColor = Colors.Yellow;
                    Color.Label = "Yellow Selected";
                    break;
                }
                case "Indigo":
                {
                    FillColor = Colors.Indigo;
                    Color.Label = "Indigo Selected";
                    break;
                }
                case "Violet":
                {
                    FillColor = Colors.Violet;
                    Color.Label = "Violet Selected";
                    break;
                }
                case "Black":
                {
                    FillColor = Colors.Black;
                    Color.Label = "Black Selected";
                    break;
                }
                case "White":
                {
                    FillColor = Colors.White;
                    Color.Label = "White Selected";
                    break;
                }
                default:
                {
                    ColorChange = false;
                    break;
                }
            }

        }

        // Add an image
        private async void Image_Click(object sender, RoutedEventArgs e)
        {
            // Create new picker
            var picker =
                new FileOpenPicker
                {
                    ViewMode = PickerViewMode.Thumbnail,
                    SuggestedStartLocation = PickerLocationId.PicturesLibrary
                };

            // Add filetype filters
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            // Retrieve file from picker
            var file = await picker.PickSingleFileAsync();
            if (file == null) return;
            // Open a stream for the selected image file.  
            using (var fileStream = await file.OpenAsync(FileAccessMode.Read))
            {
                var input = new ImageInput(fileStream);
                Workspace.GetCanvas().Children.Add(input.UploadImage);
            }
        }

        //Add a video button
        private async void Video_Click(object sender, RoutedEventArgs e)
        {
            //Create a new picker
            var filePicker = new FileOpenPicker();

            //Add filetype filters
            filePicker.FileTypeFilter.Add(".wmv");
            filePicker.FileTypeFilter.Add(".mp4");
            filePicker.FileTypeFilter.Add(".mkv");

            //Set picker start location to the video library
            filePicker.SuggestedStartLocation = PickerLocationId.VideosLibrary;

            //Retrieve file from picker
            var file = await filePicker.PickSingleFileAsync();

            if (file == null) return;
            var input = new VideoInput(file);
            Workspace.GetCanvas().Children.Add(input.UploadVideo);
        }

        // When active, allows nodes to be connected
        private void Link_Click(object sender, RoutedEventArgs e)
        {
            if ((string)Link.Tag == "Link")
            {
                Link.Tag = "LinkClicked";
                Link.Label = "Tap two nodes to link";
                Link.Background = new SolidColorBrush(Colors.CadetBlue);
                IsLink = true;
            }
            else
            {
                Link.Tag = "Link";
                Link.Label = "Add Visual Link";
                Link.Background = new SolidColorBrush(Colors.AliceBlue);
                IsLink = false;

                // Removes the stored LinkNode - more on this in the Connector class
                Connector.LinkNode = null;
            }
        }

        // When active, all nodes tapped will be removed from canvas
        private void Trash_Click(object sender, RoutedEventArgs e)
        {
            if ((string)Trash.Tag == "Trash")
            {
                Trash.Tag = "TrashClicked";
                Trash.Label = "Tap a node to delete";
                Trash.Background = new SolidColorBrush(Colors.CadetBlue);
                Delete = true;
            }
            else
            {
                Trash.Tag = "Trash";
                Trash.Label = "Delete a node";
                Trash.Background = new SolidColorBrush(Colors.AliceBlue);
                Delete = false;
            }
        }
    }
}
