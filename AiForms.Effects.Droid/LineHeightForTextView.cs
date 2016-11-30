using System;
using Android.Widget;
using AView = Android.Views.View;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AiForms.Effects.Droid
{
    public class LineHeightForTextView:ILineHeightEffect
    {
        private Android.Views.ViewGroup _container;
        private TextView _textView;
        private float _orgMultiple;
        private float _preMultiple;
        private VisualElement _formsElement;
        private float _multiple;
        private bool _isFixedHeight;

        public LineHeightForTextView(Android.Views.ViewGroup container,AView control,Element element)
        {
            _container = container;
            _textView = control as TextView;
            _orgMultiple = _textView.LineSpacingMultiplier;
            _preMultiple = _orgMultiple;
            _formsElement = element as VisualElement;

            //最初からHeightRequestが設定されているか
            _isFixedHeight = _formsElement.HeightRequest >= 0d;
        }

        public void OnDetached()
        {
            var renderer = _container as IVisualElementRenderer;
            if (renderer?.Element != null) {
                _textView.SetLineSpacing(1f, _orgMultiple);
                if (!_isFixedHeight) {
                    var size = _formsElement.Height * (_orgMultiple / _multiple);
                    _formsElement.HeightRequest = size;
                    _formsElement.HeightRequest = -1;   //再Attacheされた時の為に初期値に戻しておく
                }
            }
          
            _textView = null;
            _formsElement = null;
        }

        public void Update()
        {
            _multiple = (float)AlterLineHeight.GetMultiple(_formsElement);

            _textView.SetLineSpacing(1f, _multiple);

            if (!_isFixedHeight && _formsElement.Height >= 0) {
                var size = _formsElement.Height * (_multiple / _preMultiple);
                _formsElement.HeightRequest = size;
            }

            _preMultiple = _multiple;
        }


    }
}
