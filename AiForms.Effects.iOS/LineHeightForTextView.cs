using System;
using Foundation;
using UIKit;
using Xamarin.Forms;

namespace AiForms.Effects.iOS
{
    [Foundation.Preserve(AllMembers = true)]
    public class LineHeightForTextView : IAiEffect
    {
        private UITextView _nativeTextView;
        private Editor _formsEditor;
        private NSAttributedString _orgString;


        public LineHeightForTextView(UIView container, UIView control, Element element)
        {
            _nativeTextView = control as UITextView;
            _formsEditor = element as Editor;
            _orgString = _nativeTextView.AttributedText;
        }

        public void OnDetached()
        {
            _nativeTextView.LayoutManager.Delegate = null;


            _nativeTextView.AttributedText = _orgString;
            _nativeTextView.Text = _formsEditor.Text;

            _orgString?.Dispose();
            _orgString = null;
            _nativeTextView = null;
            _formsEditor = null;
        }

        public void Update()
        {
            var multiple = AlterLineHeight.GetMultiple(_formsEditor);
            var fontSize = (float)(_formsEditor).FontSize;
            var lineSpacing = (fontSize * multiple) - fontSize;

            var text = _formsEditor.Text;

            var pStyle = new NSMutableParagraphStyle() {
                LineSpacing = (float)lineSpacing
            };
            var attrString = new NSMutableAttributedString(text);

            attrString.AddAttribute(UIStringAttributeKey.ParagraphStyle,
                                    pStyle,
                                    new NSRange(0, attrString.Length));

            attrString.AddAttribute(UIStringAttributeKey.Font, _nativeTextView.Font, new NSRange(0, attrString.Length));
            attrString.AddAttribute(UIStringAttributeKey.ForegroundColor, _nativeTextView.TextColor, new NSRange(0, attrString.Length));

            _nativeTextView.AttributedText = attrString;
        }
    }
}
