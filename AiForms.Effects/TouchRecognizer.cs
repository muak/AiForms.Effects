using System;
using Xamarin.Forms;

namespace AiForms.Effects
{
    /// <summary>
    /// Touch recognizer.
    /// </summary>
    public class TouchRecognizer
    {
        /// <summary>
        /// Occurs when touch begin.
        /// </summary>
        public event EventHandler<TouchEventArgs> TouchBegin;
        /// <summary>
        /// Occurs when touch move.
        /// </summary>
        public event EventHandler<TouchEventArgs> TouchMove;
        /// <summary>
        /// Occurs when touch end.
        /// </summary>
        public event EventHandler<TouchEventArgs> TouchEnd;
        /// <summary>
        /// Occurs when touch cancel.
        /// </summary>
        public event EventHandler<TouchEventArgs> TouchCancel;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Effects.TouchRecognizer"/> class.
        /// </summary>
        public TouchRecognizer()
        {
        }

        /// <summary>
        /// Ons the touch begin.
        /// </summary>
        /// <param name="ev">Ev.</param>
        public virtual void OnTouchBegin(TouchEventArgs ev)
        {
            TouchBegin?.Invoke(this, ev);
        }

        /// <summary>
        /// Ons the touch move.
        /// </summary>
        /// <param name="ev">Ev.</param>
        public virtual void OnTouchMove(TouchEventArgs ev)
        {
            TouchMove?.Invoke(this, ev);
        }

        /// <summary>
        /// Ons the touch end.
        /// </summary>
        /// <param name="ev">Ev.</param>
        public virtual void OnTouchEnd(TouchEventArgs ev)
        {
            TouchEnd?.Invoke(this, ev);
        }

        /// <summary>
        /// Ons the touch cancel.
        /// </summary>
        /// <param name="ev">Ev.</param>
        public virtual void OnTouchCancel(TouchEventArgs ev)
        {
            TouchCancel?.Invoke(this, ev);
        }

    }

    /// <summary>
    /// Touch event arguments.
    /// </summary>
    public class TouchEventArgs:EventArgs
    {
        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public Point Location { get; set; }
    }
}
