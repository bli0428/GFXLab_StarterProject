using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace GFXStarterProject
{
    /// <summary>
    /// The superclass of all nodes in the project.
    /// </summary
    /// <remarks>
    /// Provides touch and drag functionality for instances of the subclasses, 
    /// along with pinch resizing.
    /// </remarks>
    public abstract class Transform : Control
    {
        private TranslateTransform _dragTranslation;
        private ScaleTransform _resize;
        private readonly Canvas _workspace = MainPage.Main.Workspace.GetCanvas();

        // A list of all existing visual links
        private static readonly List<Connector> Links = new List<Connector>();

        //A TransformGroup is used to allow for multiple transformations at once.
        private readonly TransformGroup _transformations = new TransformGroup();

        /**
         * The following 5 variables aren't used in this class, except for Zoom.
         * They are used in subclasses in order to always put new Nodes in the
         * center of the user's view.
         */
        protected float Zoom = MainPage.MyScrollViewer.ZoomFactor;
        protected double OffsetX = MainPage.MyScrollViewer.HorizontalOffset;
        protected double OffsetY = MainPage.MyScrollViewer.VerticalOffset;
        protected double ViewWidth = MainPage.MyScrollViewer.ActualWidth;
        protected double ViewHeight = MainPage.MyScrollViewer.ActualHeight;

        /// <summary>
        /// A function that allows the input UIElement elt to move around the canvas
        /// when the user uses touch and drag motions
        /// </summary>
        /// <param name="elt">Object that will enable translation movements </param>
        public void AllowTranslation(UIElement elt)
        {
            elt.ManipulationMode = ManipulationModes.All;

            // Listener for the ManipulationDelta event.
            elt.ManipulationDelta += moveable_ManipulationDelta;
            // New translation transform populated in 
            // the ManipulationDelta handler.
            _dragTranslation = new TranslateTransform();
            // Apply the translation to the UIElement
            // elt.RenderTransform = this.dragTranslation;

            _transformations.Children.Add(_dragTranslation);
            elt.RenderTransform = _transformations;
        }

        /// <summary>
        /// A function that allows the input UIElement elt to be resized when the user
        /// spreads fingers apart or pinch
        /// </summary>
        /// <param name="elt">Object that will enable resizing </param>
        public void AllowResize(UIElement elt)
        {
            elt.ManipulationMode = ManipulationModes.All;
            elt.ManipulationDelta += size_ManipulationDelta;
            _resize = new ScaleTransform();
            elt.RenderTransform = _resize;

            _transformations.Children.Add(_resize);
            elt.RenderTransform = _transformations;

        }

        public void AllowTouch(UIElement elt)
        {
            elt.PointerPressed += node_PointerPressed;
            elt.PointerReleased += node_PointerReleased;
            elt.PointerExited += node_PointerExited;
        }


        /// <summary>
        /// Handler for the ManipulationDelta event. ManipulationDelta data is 
        /// loaded into the translation transform and applied to the object/node.
        /// </summary>
        /// <param name="sender">The object/node sending the event</param>
        /// <param name="e">The event of the user touching the node and dragging</param>
        private void moveable_ManipulationDelta(object sender,
            ManipulationDeltaRoutedEventArgs e)
        {
            // Reassigning Zoom might seem a bit redundant, but this allows the Zoom variable to be updated
            // to the current zoomfactor rather than the zoomfactor at the time the sender object was initialized
            Zoom = MainPage.MyScrollViewer.ZoomFactor;
            // Moves the node. Zoom allows the translation to scale to match the user's
            // drag speed when the view is zoomed in or out
            _dragTranslation.X += e.Delta.Translation.X / Zoom;
            _dragTranslation.Y += e.Delta.Translation.Y / Zoom;

            foreach (var link in Links)
            {
                link.NewPosition();
            }

        }

        /// <summary>
        /// Another handler for the ManipulationDelta event. This time, the data is loaded
        /// into a scale transformation in order to resize the node.
        /// </summary>
        /// <param name="sender">The node sendnig the event</param>
        /// <param name="e">The event of the user pinching/unpinching</param>
        private void size_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            float newScale = e.Delta.Scale;

            _resize.ScaleX *= newScale;
            _resize.ScaleY *= newScale;

            var elt = (UIElement) sender;
            Connector.Scales[elt] *= newScale;

            foreach (var link in Links)
            {
                link.NewPosition();
            }
        }

        /// <summary>
        /// Handler for when the user lifts finger off object. First checks if the user wants to delete
        /// then checks if the user wants to add a link, then checks if the user wants to change colors. 
        /// </summary>
        /// <param name="sender"> The object from which the finger is lifted</param>
        /// <param name="e">The event being sent</param>
        private void node_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            var elt = sender as UIElement;

            // Pointer moved outside Rectangle hit test area.
            // Reset the dimensions of the Rectangle.

            if (null == elt) return;

            var children = _workspace.Children;

            // I check if Delete is set to true, and if so, I remove the node and any attached connectors 
            if (MainPage.Delete)
            {
                _workspace.Children.Remove(elt);
                foreach (var link in Links)
                {
                    if (!children.Contains(link.GetE1()) ||
                        !children.Contains(link.GetE2()))
                    {
                        children.Remove(link.Linker);
                    }
                }
               
            }
            // I check is IsLink is true, and make sure that the user isn't trying to link
            // the links themselves, which gets very glitchy. If so, then I check first to
            // LinkNode is storing a node.
            else if (MainPage.IsLink && elt.GetType() != typeof(Line))
            {
                // This means the user is picking the first node
                if (Connector.LinkNode == null)
                {
                    Connector.LinkNode = elt;
                }
                // This means the user already picked the first node, and is picking a second to link.
                else
                {
                   var visLink = new Connector(elt);
                   children.Add(visLink.Linker);
                    Connector.LinkNode = null;
                    Links.Add(visLink);
                }
            // If the user doesn't want to link or delete, then this checks to see if the user wants
            // to change a node's color. Each node has a slightly different behavior in this case.
            } else if (MainPage.ColorChange)
            {
                ChangeColor(MainPage.FillColor);
            }
        }

        /// <summary>
        /// Sends element to the back if the node is a visual link line and the user finishes
        /// the tap event.
        /// </summary>
        private static void node_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var elt = sender as UIElement;

            if (elt is Line)
            {
                Canvas.SetZIndex(elt, -1);
            }
        }

        /// <summary>
        /// If the user presses a node, send the node to the front
        /// </summary>
        private void node_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var elt = sender as UIElement;
            var highZ = _workspace.Children.Select(Canvas.GetZIndex).Concat(new[] { 0 }).Max();
            Canvas.SetZIndex(elt, highZ + 1);

        }

        /// <summary>
        /// The color changed varies based on 
        /// </summary>
        /// <param name="color"></param>
        protected abstract void ChangeColor(Windows.UI.Color color);
    }
}
