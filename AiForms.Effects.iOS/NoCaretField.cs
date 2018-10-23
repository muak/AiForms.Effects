using System;
using CoreGraphics;
using UIKit;

namespace AiForms.Effects.iOS
{
    [Foundation.Preserve(AllMembers = true)]
    internal class NoCaretField : UITextField
    {
        public NoCaretField() : base(new CGRect())
        {
        }

        public override CGRect GetCaretRectForPosition(UITextPosition position)
        {
            return new CGRect();
        }

    }
}
