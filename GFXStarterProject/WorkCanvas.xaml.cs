using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace GFXStarterProject
{
    /// <summary>
    /// A separate class for the canvas itself
    /// </summary>
    public sealed partial class WorkCanvas
    {
        /// <summary>
        /// Initialize the canvas- make the canvas incredibly large
        /// </summary>
        public WorkCanvas()
        {
            InitializeComponent();
            Loaded += WorkCanvas_Loaded;

        }

        /// <summary>
        /// Set the view into the center of the canvas
        /// </summary>
        private void WorkCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            myScrollViewer.ChangeView(myScrollViewer.ScrollableWidth / 2 - myScrollViewer.ActualWidth / 2,
                myScrollViewer.ScrollableHeight / 2 - myScrollViewer.ActualHeight / 2, myScrollViewer.ZoomFactor);
        }

        /// <summary>
        /// Gets the canvas
        /// </summary>
        /// <returns>The canvas named workspace</returns>
        public Canvas GetCanvas()
        {
            return Workspace;
        }

        /// <summary>
        /// Gets the scrollviwer
        /// </summary>
        /// <returns>Returns the scrollviewer named myScrollViewer</returns>
        public ScrollViewer GetScrollViewer()
        {
            return myScrollViewer;
        }
    }
}
