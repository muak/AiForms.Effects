using System;
using System.Linq;
using Xamarin.Forms;

namespace AiForms.Effects.Droid
{
    public static class ElementExtensions
    {
        public static bool IsFastRenderers = global::Xamarin.Forms.Forms.Flags.Any(x => x == "FastRenderers_Experimental");

        // whether Element is FastRenderer.(Exept Button)
        public static bool IsFastRenderer(this Element elm)
        {
            return (IsFastRenderers && (elm is Label || elm is Image));
        }

        // whether Element can add ClickListener.
        public static bool IsClickable(this Element elm)
        {
            return !(elm.IsFastRenderer() || elm is Layout || elm is BoxView);
        }

        // whether Element is a Button of FastRenderer.
        public static bool IsFastRendererButton(this Element elm)
        {
            return (IsFastRenderers && (elm is Button));
        }
    }
}
