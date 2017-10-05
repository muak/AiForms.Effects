using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AiForms.Effects.Droid
{
    public class AlterColorSlider : IAiEffectDroid
    {
        SeekBar _seekbar;
        Element _element;

        LayerDrawable _orgProgress;
        Drawable _orgThumb;

        LayerDrawable _progress;
        Drawable _minDrawable;
        Drawable _maxDrawable;
        Drawable _thumb;

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

            _minDrawable = _progress.GetDrawable(2);
            _maxDrawable = _progress.GetDrawable(0);

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
            _seekbar.ProgressDrawable = _orgProgress;
            _seekbar.SetThumb(_orgThumb);
        }

        public void OnDetached()
        {
            if (notSupported) {
                return;
            }

            _minDrawable.ClearColorFilter();
            _maxDrawable.ClearColorFilter();

            _minDrawable.Dispose();
            _maxDrawable.Dispose();
            _thumb.Dispose();
            _progress.Dispose();

            _minDrawable = null;
            _maxDrawable = null;
            _thumb = null;
            _progress = null;
            _orgProgress = null;
            _orgThumb = null;
            _seekbar = null;
            _element = null;
        }

        public void Update()
        {
            if (notSupported) {
                return;
            }
            var color = AlterColor.GetAccent(_element).ToAndroid();
            var altColor = Android.Graphics.Color.Argb(76, color.R, color.G, color.B);

            //if use SetTint,it cannot restore.
            _minDrawable.SetColorFilter(color, PorterDuff.Mode.SrcIn);
            _maxDrawable.SetColorFilter(altColor, PorterDuff.Mode.SrcIn);
            _thumb.SetTint(color);
        }


    }
}
