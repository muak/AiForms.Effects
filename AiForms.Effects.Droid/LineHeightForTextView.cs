using Android.Widget;
using Xamarin.Forms;
using AView = Android.Views.View;

namespace AiForms.Effects.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class LineHeightForTextView : IAiEffectDroid
    {
        private Android.Views.View _container;
        private TextView _textView;
        private float _orgMultiple;
        private float _preMultiple;
        private VisualElement _formsElement;
        private float _multiple;
        private bool _isFixedHeight;

        public LineHeightForTextView(Android.Views.ViewGroup container, AView control, Element element)
        {
            _container = container ?? control;
            _textView = control as TextView;
            _orgMultiple = _textView.LineSpacingMultiplier;
            _preMultiple = _orgMultiple;
            _formsElement = element as VisualElement;

            //最初からHeightRequestが設定されているか
            _isFixedHeight = _formsElement.HeightRequest >= 0d;
        }

        public void OnDetachedIfNotDisposed()
        {
            _textView.SetLineSpacing(1f, _orgMultiple);
            if (!_isFixedHeight) {
                var size = _formsElement.Height * (_orgMultiple / _multiple);
                _formsElement.Layout(new Rectangle(_formsElement.Bounds.X, _formsElement.Bounds.Y, _formsElement.Bounds.Width, size));
            }
        }

        public void OnDetached()
        {
            _textView = null;
            _formsElement = null;
        }

        public void Update()
        {
            _multiple = (float)AlterLineHeight.GetMultiple(_formsElement);

            _textView.SetLineSpacing(1f, _multiple);

            if (!_isFixedHeight && _formsElement.Height >= 0) {
                var size = _formsElement.Height * (_multiple / _preMultiple);
                _formsElement.Layout(new Rectangle(_formsElement.Bounds.X, _formsElement.Bounds.Y, _formsElement.Bounds.Width, size));
            }

            _preMultiple = _multiple;
        }


    }
}
