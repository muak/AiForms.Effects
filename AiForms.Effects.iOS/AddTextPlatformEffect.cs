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
        private Thickness _margin = 0;

        protected override void OnAttached()
        {
            _textLabel = new PaddingLabel();
            _textLabel.LineBreakMode = UILineBreakMode.Clip;
            _textLabel.Lines = 1;
            _textLabel.TintAdjustmentMode = UIViewTintAdjustmentMode.Automatic;
            _textLabel.AdjustsFontSizeToFitWidth = true;
            _textLabel.BaselineAdjustment = UIBaselineAdjustment.AlignCenters;
            _textLabel.AdjustsLetterSpacingToFitWidth = true;
            _textLabel.BackgroundColor = UIColor.FromRGBA(0, 0, 0, 0.5f);
            _textLabel.Padding = new UIEdgeInsets(4,4,4,4);


            Container.AddSubview(_textLabel);

            _textLabel.TranslatesAutoresizingMaskIntoConstraints = false;

            UpdateText();
            UpdateFontSize();
            UpdateTextColor();
            UpdateMargin();
            UpdateHorizontalAlign();
            UpdateVerticalAlign();
        }

        protected override void OnDetached()
        {
            Container.RemoveConstraints(_constraint);
            _textLabel.RemoveFromSuperview();
            _textLabel.Dispose();
            _constraint = null;
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
            else if (args.PropertyName == AddText.MarginProperty.PropertyName) {
                UpdateMargin();
            }
            else if (args.PropertyName == AddText.HorizontalAlignProperty.PropertyName) {
                UpdateHorizontalAlign();
            }
            else if (args.PropertyName == AddText.VerticalAlignProperty.PropertyName) {
                UpdateVerticalAlign();
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

        void UpdateMargin()
        {
            _margin = AddText.GetMargin(Element);
        }

        void UpdateHorizontalAlign()
        {
            //_textLabel.TextAlignment = AddText.GetHorizontalAlign(Element).ToNativeTextAlignment();
            //var align = AddText.GetHorizontalAlign(Element);
            //if (_constraint != null) {
            //    Container.RemoveConstraints(_constraint);
            //}
            //_constraint = CreateConstraint(_margin, align == TextAlignment.Start,);
            //Container.AddConstraints(_constraint);
            UpdateVerticalAlign();
        }

        void UpdateVerticalAlign()
        {
            var align = AddText.GetVerticalAlign(Element);
            if (_constraint != null) {
                Container.RemoveConstraints(_constraint);
            }
            _constraint = CreateConstraint(_margin, align == TextAlignment.Start);
            Container.AddConstraints(_constraint);
        }

        NSLayoutConstraint[] CreateConstraint(Thickness margin, bool isTop = true)
        {
            var isLeft = AddText.GetHorizontalAlign(Element) == Xamarin.Forms.TextAlignment.Start;
            var constraint = new NSLayoutConstraint[]{
                //NSLayoutConstraint.Create(
                //    _textLabel,
                //    NSLayoutAttribute.Left,
                //    NSLayoutRelation.Equal,
                //    Container,
                //    NSLayoutAttribute.Left,
                //    1,
                //    (nfloat)margin.Left
                //),
                //NSLayoutConstraint.Create(
                //    _textLabel,
                //    NSLayoutAttribute.Right,
                //    NSLayoutRelation.Equal,
                //    Container,
                //    NSLayoutAttribute.Right,
                //    1,
                //    -(nfloat)margin.Right
                //),
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
                //NSLayoutConstraint.Create(
                //    _textLabel,
                //    NSLayoutAttribute.Height,
                //    NSLayoutRelation.Equal,
                //    null,
                //    NSLayoutAttribute.Height,
                //    1,
                //    (float)AddText.GetFontSize(Element) + (float)margin.Top + (float)margin.Bottom
                //),

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
