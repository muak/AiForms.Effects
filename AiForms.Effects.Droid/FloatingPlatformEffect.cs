using System;
using XF = Xamarin.Forms;
using Android.Views;
using Xamarin.Forms.Platform.Android;
using System.Runtime.InteropServices;
using AiForms.Effects.Droid;
using AiForms.Effects;
using Android.Widget;

[assembly: XF.ExportEffect(typeof(FloatingPlatformEffect), nameof(Floating))]
namespace AiForms.Effects.Droid
{
    [Android.Runtime.Preserve(AllMembers =true)]
    public class FloatingPlatformEffect : AiEffectBase
    {
        protected override void OnAttached()
        {
            if (!IsSupportedByApi)
                return;

            if (!(Element is XF.Page)) return;

            var layout = Floating.GetContent(Element);
            layout.Parent = Element;

            // All following process is done on the side of FloatingLayoutRenderer.
            Platform.CreateRendererWithContext(layout, Container.Context);          
        }

        protected override void OnDetached()
        {
            // The FloatingLayout renderer is automatically disposed when the parent page is popped.
        }
    }


}
