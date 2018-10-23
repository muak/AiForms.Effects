using System;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;
using Xamarin.Forms;
using Android.Content.Res;

namespace AiForms.Effects.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class AlterColorTextView : IAiEffectDroid
    {
        TextView _textview;
        Element _element;

        Drawable _orgBackground;
        Drawable _background;

        public AlterColorTextView(TextView textview, Element element)
        {
            _textview = textview;
            _element = element;

            _orgBackground = _textview.Background;

            _background = _textview.Background.GetConstantState().NewDrawable();
            _textview.Background = _background;
        }

        public void OnDetachedIfNotDisposed()
        {
            _textview.Background = _orgBackground;
        }

        public void OnDetached()
        {
            _background.Dispose();
            _orgBackground = null;
            _textview = null;
            _element = null;
        }

        public void Update()
        {
            var color = AlterColor.GetAccent(_element).ToAndroid();
            var colorlist = new ColorStateList(new int[][]
            {
                new int[]{global::Android.Resource.Attribute.StateFocused},
                new int[]{-global::Android.Resource.Attribute.StateFocused},
            },
                new int[] {
                    Android.Graphics.Color.Argb(255,color.R,color.G,color.B),
                    Android.Graphics.Color.Argb(255, 200, 200, 200)
            });
            _background.SetTintList(colorlist);
        }
    }
}
