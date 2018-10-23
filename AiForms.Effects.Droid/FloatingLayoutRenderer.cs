using System;
using XF = Xamarin.Forms;
using AiForms.Effects;
using AiForms.Effects.Droid;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using Android.Content;
using Android.Views;
using Xamarin.Forms;
using System.Threading.Tasks;

[assembly: XF.ExportRenderer(typeof(FloatingLayout), typeof(FloatingLayoutRenderer))]
namespace AiForms.Effects.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class FloatingLayoutRenderer:ViewRenderer<FloatingLayout,FrameLayout>
    {
        XF.Page _page;
        Action OnceInitializeAction;

        public FloatingLayoutRenderer(Context context):base(context){}

        protected override void OnElementChanged(ElementChangedEventArgs<FloatingLayout> e)
        {
            base.OnElementChanged(e);
            if(e.NewElement != null)
            {
                OnceInitializeAction = Initialize;

                var layout = new FrameLayout(Context);
                layout.SetClipChildren(false);
                layout.SetClipToPadding(false);

                SetNativeControl(layout);

                _page = Element.Parent as XF.Page;
                _page.SizeChanged += PageSizeChanged;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                Element.Parent = null;
                _page.SizeChanged -= PageSizeChanged;
                foreach (var child in Element)
                {
                    PlatformUtility.DisposeModelAndChildrenRenderers(child);
                }
                RootView.RemoveFromParent();
                _page = null;
            }
            base.Dispose(disposing);
        }

        void PageSizeChanged(object sender, EventArgs e)
        {
            if (OnceInitializeAction == null)
            {
                Element.Layout(_page.Bounds);
                Element.LayoutChildren();
                Layout(0, 0, (int)Context.ToPixels(_page.Bounds.Width), (int)Context.ToPixels(_page.Bounds.Height));
            }
            else
            {
                OnceInitializeAction.Invoke();
            }
        }

        void Initialize()
        {
            OnceInitializeAction = null;

            Element.Layout(_page.Bounds);

            foreach (var child in Element)
            {
                SetChildLayout(child);
            }

            Layout(0, 0, (int)Context.ToPixels(_page.Bounds.Width), (int)Context.ToPixels(_page.Bounds.Height));

            var pageView = Platform.GetRenderer(_page)?.View as ViewGroup;
            using (var param = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent))
            {
                pageView.AddView(RootView, param);
            }
        }

        void SetChildLayout(FloatingView child)
        {
            var renderer = PlatformUtility.GetOrCreateNativeView(child, Context);
            
            var nativeChild = renderer.View;
            
            Element.LayoutChild(child);

            int width = -1;
            int height = -1;

            if (child.HorizontalLayoutAlignment != XF.LayoutAlignment.Fill)
            {
                width = (int)Context.ToPixels(child.Bounds.Width);
            }

            if (child.VerticalLayoutAlignment != XF.LayoutAlignment.Fill)
            {
                height = (int)Context.ToPixels(child.Bounds.Height);
            }


            using (var param = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
            {
                Width = width,
                Height = height,
                Gravity = GetGravity(child),
            })
            {
                SetOffsetMargin(param, child);
                Control.AddView(nativeChild, param);
            }
        }

        internal void SetOffsetMargin(FrameLayout.LayoutParams layoutParams, FloatingView view)
        {
            var offsetX = (int)Context.ToPixels(view.OffsetX);
            if(view.HorizontalLayoutAlignment == LayoutAlignment.Fill)
            {
                offsetX = 0;
            }
            var offsetY = (int)Context.ToPixels(view.OffsetY);
            if(view.VerticalLayoutAlignment == LayoutAlignment.Fill)
            {
                offsetY = 0;
            }

            // the offset direction is reversed when GravityFlags contains Left or Bottom.
            if (view.HorizontalLayoutAlignment == XF.LayoutAlignment.End)
            {
                layoutParams.RightMargin = offsetX * -1;
            }
            else
            {
                layoutParams.LeftMargin = offsetX;
            }

            if (view.VerticalLayoutAlignment == XF.LayoutAlignment.End)
            {
                layoutParams.BottomMargin = offsetY * -1;
            }
            else
            {
                layoutParams.TopMargin = offsetY;
            }
        }

        internal GravityFlags GetGravity(FloatingView view)
        {
            GravityFlags gravity = GravityFlags.NoGravity;
            switch (view.VerticalLayoutAlignment)
            {
                case XF.LayoutAlignment.Start:
                    gravity |= GravityFlags.Top;
                    break;
                case XF.LayoutAlignment.End:
                    gravity |= GravityFlags.Bottom;
                    break;
                case XF.LayoutAlignment.Center:
                    gravity |= GravityFlags.CenterVertical;
                    break;
                default:
                    gravity |= GravityFlags.FillVertical;
                    break;
            }

            switch (view.HorizontalLayoutAlignment)
            {
                case XF.LayoutAlignment.Start:
                    gravity |= GravityFlags.Left;
                    break;
                case XF.LayoutAlignment.End:
                    gravity |= GravityFlags.Right;
                    break;
                case XF.LayoutAlignment.Center:
                    gravity |= GravityFlags.CenterHorizontal;
                    break;
                default:
                    gravity |= GravityFlags.FillHorizontal;
                    break;
            }

            return gravity;
        }
    }
}
