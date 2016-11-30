using System;
using UIKit;
using Xamarin.Forms;
using System.ComponentModel;
using Foundation;

namespace AiForms.Effects.iOS
{
    public class LineHeightForLabel : ILineHeightEffect
    {
        private UIView _container;
        private UILabel _nativeLabel;
        private Label _formsLabel;
        private bool _isFixedHeight;

        public LineHeightForLabel(UIView container, UIView control, Element element)
        {
            _container = container;
            _nativeLabel = control as UILabel;
            _formsLabel = element as Label;
            //最初からHeightRequestが設定されているか
            _isFixedHeight = _formsLabel.HeightRequest >= 0d;
        }

        public void OnDetached()
        {
            _nativeLabel.AttributedText = null;
            _nativeLabel.Text = _formsLabel.Text;
            if (!_isFixedHeight) {
                var size = _nativeLabel.SizeThatFits(_container.Frame.Size);
                _formsLabel.HeightRequest = size.Height;
                _formsLabel.HeightRequest = -1;   //再Attacheされた時の為に初期値に戻しておく
            }
            _container = null;
            _nativeLabel = null;
            _formsLabel = null;
        }

        public void Update()
        {
            var text = _formsLabel.Text;
            if (text == null)
                return;

            var multiple = (float)AlterLineHeight.GetMultiple(_formsLabel);
            var fontSize = (float)(_formsLabel).FontSize;
            var lineSpacing = (fontSize * multiple) - fontSize;
            var pStyle = new NSMutableParagraphStyle() { LineSpacing = lineSpacing };
            var attrString = new NSMutableAttributedString(text);

            attrString.AddAttribute(UIStringAttributeKey.ParagraphStyle,
                                    pStyle,
                                    new NSRange(0, attrString.Length));

            _nativeLabel.AttributedText = attrString;

            if (!_isFixedHeight && _formsLabel.Height >= 0) {
                var size = _nativeLabel.SizeThatFits(_container.Frame.Size);
                _formsLabel.HeightRequest = size.Height;
            }

        }
    }
}
