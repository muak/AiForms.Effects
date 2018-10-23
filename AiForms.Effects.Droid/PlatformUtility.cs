using System;
using Xamarin.Forms.Platform.Android;
using XF = Xamarin.Forms;
using Android.Content;
using Xamarin.Forms;

namespace AiForms.Effects.Droid
{
    public static class PlatformUtility
    {
        static BindableProperty RendererProperty = (BindableProperty)typeof(Platform).GetField("RendererProperty", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).GetValue(null);

        internal static IVisualElementRenderer GetOrCreateNativeView(XF.View view,Context context)
        {
            IVisualElementRenderer renderer = Platform.GetRenderer(view);
            if (renderer == null)
            {
                renderer = Platform.CreateRendererWithContext(view, context);
                Platform.SetRenderer(view, renderer);
            }

            return renderer;
        }

        // From internal Platform class
        internal static void DisposeModelAndChildrenRenderers(XF.Element view)
        {
            IVisualElementRenderer renderer;
            foreach (XF.VisualElement child in view.Descendants())
            {
                renderer = Platform.GetRenderer(child);
                child.ClearValue(RendererProperty);

                if (renderer != null)
                {
                    renderer.View.RemoveFromParent();
                    renderer.Dispose();
                }
            }

            renderer = Platform.GetRenderer((XF.VisualElement)view);
            if (renderer != null)
            {
                renderer.View.RemoveFromParent();
                renderer.Dispose();
            }

            view.ClearValue(RendererProperty);
        }
    }
}
