using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AiForms.Effects.Droid
{
    // References:
    // http://www.zoftino.com/android-seekbar-and-custom-seekbar-examples
    [Android.Runtime.Preserve(AllMembers = true)]
    public class AlterColorSlider : IAiEffectDroid
    {
        SeekBar _seekbar;
        Element _element;

        LayerDrawable _orgProgress;
        Drawable _orgThumb;

        LayerDrawable _progress;
        Drawable _thumb;
        ColorStateList _orgProgressBackground;

        bool notSupported = false;

        public AlterColorSlider(SeekBar seekbar, Element element)
        {
            _seekbar = seekbar;
            _element = element;

            _orgProgress = _seekbar.ProgressDrawable.Current as LayerDrawable;
            if (_orgProgress == null) {
                notSupported = true;
                return;
            }

            _progress = (LayerDrawable)(_seekbar.ProgressDrawable.Current.GetConstantState().NewDrawable());

            _orgProgressBackground = _seekbar.ProgressBackgroundTintList;

            _orgThumb = _seekbar.Thumb;
            _thumb = _seekbar.Thumb.GetConstantState().NewDrawable();

            _seekbar.ProgressDrawable = _progress;
            _seekbar.SetThumb(_thumb);
        }

        public void OnDetachedIfNotDisposed()
        {
            if (notSupported) {
                return;
            }
            _seekbar.ProgressBackgroundTintList = _orgProgressBackground;
            _seekbar.ProgressDrawable = _orgProgress;
            _seekbar.SetThumb(_orgThumb);
        }

        public void OnDetached()
        {
            if (notSupported) {
                return;
            }

            _thumb.Dispose();
            _progress.Dispose();

            _thumb = null;
            _progress = null;
            _orgProgress = null;
            _orgThumb = null;
            _orgProgressBackground = null;
            _seekbar = null;
            _element = null;
        }

        public void Update()
        {
            if (notSupported) {
                return;
            }
            var color = AlterColor.GetAccent(_element).ToAndroid();

            _progress.SetColorFilter(color, PorterDuff.Mode.SrcIn);
            _seekbar.ProgressBackgroundTintList = new ColorStateList(
                new int[][]
                {
                    new int[]{}
                },
                new int[]
                {
                    color,
                });

            _thumb.SetTint(color);
        }


    }
}
