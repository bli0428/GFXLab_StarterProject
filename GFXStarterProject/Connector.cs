using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace GFXStarterProject
{
    /// <summary>
    /// A line that links two nodes together, as opposed to other nodes that are subclasses
    /// of the Transform class, the line has no translation or resize properties, although I still
    /// do implement touch properties, in order to allow the linker to be deleted.
    /// </summary>
    /// <remarks>
    /// This class was the hardest for me to implement, due to the fact that I wanted to allow general
    /// UIElements to be linked together. The difficulty of this lay in the fact that UIElements 
    /// had no height and width dimensions, while Framework elements could only retrieve the original
    /// dimensions of the element. My solution was to use a Dictionary, which would store all the 
    /// scales of existing elements so that I could find the element's top left corner, then add
    /// half of the current height and width.
    /// </remarks>
    internal class Connector : Transform
    {
        // The line itself
        public Line Linker { get; set; }

        // The first element of the link -
        // When the visual link button is active, LinkNode will store the first node before
        // The user has decided on a second node to link to. Otherwise, LinkNode is expected to 
        // be null.
        public static UIElement LinkNode { get; set; }

        private readonly Canvas _workspace = MainPage.Main.Workspace.GetCanvas();

        // Once the link is established, the linked UIElements are stored.
        private readonly UIElement _e1;
        private readonly UIElement _e2;

        // A dictionary that contains all the current Scale values so that the endpoint position can
        // remain at the center of both nodes even when the nodes are resized.
        public static Dictionary<UIElement, float> Scales = new Dictionary<UIElement, float>();
        
        // The following variables weren't necessary to store, but rather just to prevent me
        // from having to reassign the same variables between the constructor and the function call
        // The original height and widths of the two elements
        private double _h1;
        private double _h2;
        private double _w1;
        private double _w2;

        // The framework element typecast of _e1 and _e2
        private readonly FrameworkElement _fe1;
        private readonly FrameworkElement _fe2;

        // The scales of _e1 and _e2, taken from the dictionary
        private float _e1Scale;
        private float _e2Scale;

        //try adding scale field to store node scales- calc zoom

        public Connector(UIElement elt)
        {
            Linker = new Line();

            // Store the two elements being linked
            _e1 = LinkNode;
            _e2 = elt;

            // Get the absolute position of the nodes in relation to the canvas
            var trans1 = LinkNode.TransformToVisual(_workspace);
            var pos1 = trans1.TransformPoint(new Point());
            var trans2 = elt.TransformToVisual(_workspace);
            var pos2 = trans2.TransformPoint(new Point());

            // Typecast the UIElements to find actual height and actual width
            _fe1 = _e1 as FrameworkElement;
            _fe2 = _e2 as FrameworkElement;

            // Find the current scales of the elements after resizing
            _e1Scale = Scales[_e1];
            _e2Scale = Scales[_e2];

            // Store the original height and widths of the nodes
            if (_fe1 != null)
            {
                _h1 = _fe1.ActualHeight;
                _w1 = _fe1.ActualWidth;
            }

            if (_fe2 != null)
            {
                _h2 = _fe2.ActualHeight;
                _w2 = _fe2.ActualWidth;
            }

            // Create the Linking line
            Linker.Stroke = new SolidColorBrush(Windows.UI.Colors.Black);
            Linker.StrokeThickness = 10;
            // Set the line in the back
            Canvas.SetZIndex(Linker, -1);

            // Set the line's endpoints to be the centers of the nodes
            Linker.X1 = pos1.X + _w1  * _e1Scale/ 2;
            Linker.Y1 = pos1.Y + _h1  * _e1Scale/ 2;
            Linker.X2 = pos2.X + _w2  * _e2Scale/ 2;
            Linker.Y2 = pos2.Y + _h2  * _e2Scale/ 2;

            // Allow Touch so that the line can be colored and deleted
            AllowTouch(Linker);
        }

        /// <summary>
        /// Updates the position of the line, called when the user moves or resizes a node with 
        /// connected lines
        /// </summary>
        public void NewPosition()
        {
            // Get the new absolute position of the nodes in relation to the canvas
            var trans1 = _e1.TransformToVisual(_workspace);
            var pos1 = trans1.TransformPoint(new Point());
            var trans2 = _e2.TransformToVisual(_workspace);
            var pos2 = trans2.TransformPoint(new Point());

            // Find the current scale
            _e1Scale = Scales[_e1];
            _e2Scale = Scales[_e2];

            // Update the line's position
            Linker.X1 = pos1.X + _w1 * _e1Scale / 2;
            Linker.Y1 = pos1.Y + _h1 * _e1Scale/ 2;
            Linker.X2 = pos2.X + _w2 *_e2Scale/ 2;
            Linker.Y2 = pos2.Y + _h2 *_e2Scale/ 2;
        }

        /// <summary>
        /// Getter method for _e1
        /// </summary>
        /// <returns>The first element stored in the linking connector</returns>
        public UIElement GetE1()
        {
            return _e1;
        }

        /// <summary>
        /// Getter method for _e2
        /// </summary>
        /// <returns>The second element stored in the linking connector</returns>
        public UIElement GetE2()
        {
            return _e2;
        }

        // Changes the color of the linking line
        protected override void ChangeColor(Windows.UI.Color color)
        {
            Linker.Stroke = new SolidColorBrush(color);
        }
    }
}
