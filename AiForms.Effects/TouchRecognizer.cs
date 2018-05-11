using System;
using Xamarin.Forms;

namespace AiForms.Effects
{
    public class TouchRecognizer
    {
        public event EventHandler<TouchEventArgs> TouchBegin;
        public event EventHandler<TouchEventArgs> TouchMove;
        public event EventHandler<TouchEventArgs> TouchEnd;
        public event EventHandler<TouchEventArgs> TouchCancel;

        public TouchRecognizer()
        {
        }

        public virtual void OnTouchBegin(TouchEventArgs ev)
        {
            TouchBegin?.Invoke(this, ev);
        }

        public virtual void OnTouchMove(TouchEventArgs ev)
        {
            TouchMove?.Invoke(this, ev);
        }

        public virtual void OnTouchEnd(TouchEventArgs ev)
        {
            TouchEnd?.Invoke(this, ev);
        }

        public virtual void OnTouchCancel(TouchEventArgs ev)
        {
            TouchCancel?.Invoke(this, ev);
        }

    }

    public class TouchEventArgs:EventArgs
    {
        public Point Location { get; set; }
    }
}
