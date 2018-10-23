using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace AiForms.Effects.iOS
{
    [Foundation.Preserve(AllMembers = true)]
    public class LineHeightForLabel : IAiEffect
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

            ChangeSize();

            _container = null;
            _nativeLabel = null;
            _formsLabel = null;
        }

        public void Update()
        {
            var text = _formsLabel.Text;
            if (text == null) {
                return;
            }

            var multiple = (float)AlterLineHeight.GetMultiple(_formsLabel);
            var fontSize = (float)(_formsLabel).FontSize;
            var lineSpacing = (fontSize * multiple) - fontSize;
            var pStyle = new NSMutableParagraphStyle() {
                LineSpacing = lineSpacing,
                Alignment = _formsLabel.HorizontalTextAlignment.ToNativeTextAlignment()
            };
            var attrString = new NSMutableAttributedString(text);

            attrString.AddAttribute(UIStringAttributeKey.ParagraphStyle,
                                    pStyle,
                                    new NSRange(0, attrString.Length));

            _nativeLabel.AttributedText = attrString;

            ChangeSize();

        }

        void ChangeSize()
        {
            if (_formsLabel.Height < 0) {
                return;
            }

            if (NeedToChangeSize()) {
                var size = _nativeLabel.SizeThatFits(_container.Frame.Size);
                _formsLabel.Layout(new Rectangle(_formsLabel.Bounds.X, _formsLabel.Bounds.Y, size.Width, size.Height));
            }
            else {
                var render = Platform.GetRenderer(_formsLabel) as LabelRenderer;
                render?.LayoutSubviews();
            }
        }

        bool NeedToChangeSize()
        {
            //FillAndExpandじゃなくてかつHeightRequestが未指定の時はサイズ変更
            return !_isFixedHeight &&
                    !(_formsLabel.VerticalOptions.Alignment == LayoutAlignment.Fill &&
                      _formsLabel.VerticalOptions.Expands);
        }
    }
}
