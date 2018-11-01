using System;
using Xamarin.Forms.Platform.iOS;
using AiForms.Effects;
using AiForms.Effects.iOS;
using Xamarin.Forms;
using UIKit;
using System.Linq;
using CoreGraphics;

[assembly: ExportEffect(typeof(AddTextPlatformEffect), nameof(AddText))]
namespace AiForms.Effects.iOS
{
    public class AddTextPlatformEffect : PlatformEffect
    {
        private PaddingLabel _textLabel;
        private NSLayoutConstraint[] _constraint;

        protected override void OnAttached()
        {
            _textLabel = new PaddingLabel();
            _textLabel.LineBreakMode = UILineBreakMode.Clip;
            _textLabel.Lines = 1;
            _textLabel.TintAdjustmentMode = UIViewTintAdjustmentMode.Automatic;
            _textLabel.AdjustsFontSizeToFitWidth = true;
            _textLabel.BaselineAdjustment = UIBaselineAdjustment.AlignCenters;
            _textLabel.AdjustsLetterSpacingToFitWidth = true;

            Container.AddSubview(_textLabel);

            _textLabel.TranslatesAutoresizingMaskIntoConstraints = false;

            UpdateText();
            UpdateFontSize();
            UpdateTextColor();
            UpdateBackgroundColor();
            UpdatePadding();
            UpdateConstraint();
        }

        protected override void OnDetached()
        {
            Container.RemoveConstraints(_constraint);
            _textLabel.RemoveFromSuperview();
            _textLabel.Dispose();
            _constraint = null;
            _textLabel = null;

            System.Diagnostics.Debug.WriteLine($"Detached {GetType().Name} from {Element.GetType().FullName}");
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
            if (args.PropertyName == AddText.TextProperty.PropertyName) {
                UpdateText();
            }
            else if (args.PropertyName == AddText.FontSizeProperty.PropertyName) {
                UpdateFontSize();
            }
            else if (args.PropertyName == AddText.TextColorProperty.PropertyName) {
                UpdateTextColor();
            }
            else if (args.PropertyName == AddText.BackgroundColorProperty.PropertyName) {
                UpdateBackgroundColor();
            }
            else if (args.PropertyName == AddText.PaddingProperty.PropertyName) {
                UpdatePadding();
            }
            else if (args.PropertyName == AddText.MarginProperty.PropertyName) {
                UpdateConstraint();
            }
            else if (args.PropertyName == AddText.HorizontalAlignProperty.PropertyName) {
                UpdateConstraint();
            }
            else if (args.PropertyName == AddText.VerticalAlignProperty.PropertyName) {
                UpdateConstraint();
            }
        }

        void UpdateText()
        {
            var text = AddText.GetText(Element);
            _textLabel.Text = text;
            _textLabel.Hidden = string.IsNullOrEmpty(text);
        }

        void UpdateFontSize()
        {
            _textLabel.Font = _textLabel.Font.WithSize((float)AddText.GetFontSize(Element));
        }

        void UpdateTextColor()
        {
            _textLabel.TextColor = AddText.GetTextColor(Element).ToUIColor();
        }

        void UpdateBackgroundColor()
        {
            _textLabel.BackgroundColor = AddText.GetBackgroundColor(Element).ToUIColor();
        }

        void UpdatePadding()
        {
            var padding = AddText.GetPadding(Element);
            _textLabel.Padding = new UIEdgeInsets((float)padding.Top, (float)padding.Left, (float)padding.Bottom, (float)padding.Right);
        }

        void UpdateConstraint()
        {
            
            if (_constraint != null) {
                Container.RemoveConstraints(_constraint);
            }
            _constraint = CreateConstraint();
            Container.AddConstraints(_constraint);
        }

        NSLayoutConstraint[] CreateConstraint()
        {
            var isLeft = AddText.GetHorizontalAlign(Element) == Xamarin.Forms.TextAlignment.Start;
            var isTop = AddText.GetVerticalAlign(Element) == Xamarin.Forms.TextAlignment.Start;
            var margin = AddText.GetMargin(Element);

            _textLabel.TextAlignment = isLeft ? UITextAlignment.Left : UITextAlignment.Right;

            var constraint = new NSLayoutConstraint[]{
                NSLayoutConstraint.Create(
                    _textLabel,
                    isLeft ? NSLayoutAttribute.Left : NSLayoutAttribute.Right,
                    NSLayoutRelation.Equal,
                    Container,
                    isLeft ? NSLayoutAttribute.Left : NSLayoutAttribute.Right,
                    1,
                    isLeft ? (nfloat)margin.Left : -(nfloat)margin.Right
                ),
                NSLayoutConstraint.Create(
                    _textLabel,
                    NSLayoutAttribute.Width,
                    NSLayoutRelation.LessThanOrEqual,
                    Container,
                    NSLayoutAttribute.Width,
                    1,
                    -(nfloat)(margin.Left + margin.Right)
                ),
                NSLayoutConstraint.Create(
                    _textLabel,
                    isTop ? NSLayoutAttribute.Top : NSLayoutAttribute.Bottom,
                    NSLayoutRelation.Equal,
                    Container,
                    isTop ? NSLayoutAttribute.Top : NSLayoutAttribute.Bottom,
                    1,
                    isTop ? (nfloat)margin.Top : -(nfloat)margin.Bottom
                )
            };

            return constraint;
        }
    }

    internal class PaddingLabel : UILabel
    {
        public UIEdgeInsets Padding { get; set; }

        public override void DrawText(CoreGraphics.CGRect rect)
        {
            base.DrawText(Padding.InsetRect(rect));
        }

        public override CGSize IntrinsicContentSize {
            get {
                return new CGSize(
                    base.IntrinsicContentSize.Width + Padding.Left + Padding.Right,
                    base.IntrinsicContentSize.Height + Padding.Top + Padding.Bottom
                );
            }
        }
    }

    internal static class AlignmentExtensions
    {
        internal static UITextAlignment ToNativeTextAlignment(this TextAlignment alignment)
        {
            switch (alignment) {
                case TextAlignment.Center:
                    return UITextAlignment.Center;
                case TextAlignment.End:
                    return UITextAlignment.Right;
                default:
                    return UITextAlignment.Left;
            }
        }
    }
}
