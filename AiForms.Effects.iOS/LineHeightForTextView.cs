using System;
using UIKit;
using Xamarin.Forms;

namespace AiForms.Effects.iOS
{
    public class LineHeightForTextView : ILineHeightEffect
    {
        private UITextView _nativeTextView;
        private Editor _formsEditor;
        private LineHeightManager _manager;


        public LineHeightForTextView(UIView container, UIView control, Element element)
        {
            _nativeTextView = control as UITextView;
            _formsEditor = element as Editor;
            _manager = new LineHeightManager();
            _nativeTextView.LayoutManager.Delegate = _manager;
        }

        public void OnDetached()
        {
            _nativeTextView.LayoutManager.Delegate = null;

            _nativeTextView.Text = _nativeTextView.Text;

            _manager.Dispose();
            _manager = null;
            _nativeTextView = null;
            _formsEditor = null;
        }

        public void Update()
        {
            var multiple = AlterLineHeight.GetMultiple(_formsEditor);
            var fontSize = (float)(_formsEditor).FontSize;
            var lineSpacing = (fontSize * multiple) - fontSize;
            _manager.LineSpacing = (float)lineSpacing;

            _nativeTextView.Text = _nativeTextView.Text;
        }

    }

    internal class LineHeightManager : NSLayoutManagerDelegate
    {
        public float LineSpacing { get; set; }

        public override nfloat LineSpacingAfterGlyphAtIndex(NSLayoutManager layoutManager, nuint glyphIndex, CoreGraphics.CGRect rect)
        {
            return LineSpacing;
        }
    }
}
