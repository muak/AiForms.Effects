using System;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using AiForms.Effects;
using AiForms.Effects.iOS;
using Xamarin.Forms;

[assembly: ExportEffect(typeof(PlaceholderPlatformEffect), nameof(Placeholder))]
namespace AiForms.Effects.iOS
{
    [Foundation.Preserve(AllMembers = true)]
    public class PlaceholderPlatformEffect : PlatformEffect
    {
        UITextView _textView;
        UILabel _placeholderLabel;

        protected override void OnAttached()
        {
            _textView = Control as UITextView;
            _placeholderLabel = new UILabel();

            _placeholderLabel.LineBreakMode = UILineBreakMode.WordWrap;
            _placeholderLabel.Lines = 0;
            _placeholderLabel.Font = _textView.Font;
            _placeholderLabel.BackgroundColor = UIColor.Clear;
            _placeholderLabel.Alpha = 0;

            UpdateText();
            UpdateColor();

            _textView.AddSubview(_placeholderLabel);

            _placeholderLabel.TranslatesAutoresizingMaskIntoConstraints = false;

            _placeholderLabel.TopAnchor.ConstraintEqualTo(_textView.TopAnchor,8).Active = true;
            _placeholderLabel.LeftAnchor.ConstraintEqualTo(_textView.LeftAnchor,4).Active = true;
            _placeholderLabel.RightAnchor.ConstraintEqualTo(_textView.RightAnchor,4).Active = true;
            _placeholderLabel.WidthAnchor.ConstraintEqualTo(_textView.WidthAnchor,1,-8).Active = true;

            _textView.SendSubviewToBack(_placeholderLabel);

            if (_textView.Text.Length == 0 && _placeholderLabel.Text.Length > 0) {
                _placeholderLabel.Alpha = 1;
            }

            _textView.Changed += _textView_Changed;  
        }

        protected override void OnDetached()
        {
            _textView.Changed -= _textView_Changed;
            _placeholderLabel.RemoveFromSuperview();
            _placeholderLabel.Dispose();
            _placeholderLabel = null;
            _textView = null;

            System.Diagnostics.Debug.WriteLine($"Detached {GetType().Name} from {Element.GetType().FullName}");
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (e.PropertyName == Placeholder.TextProperty.PropertyName) {
                UpdateText();
                //avoid breaking change. 
                //There exists same property in attached element's property. I should have named TextProperty other name.
                _textView_Changed(_textView,EventArgs.Empty);
            }
            else if (e.PropertyName == Placeholder.ColorProperty.PropertyName) {
                UpdateColor();
            }
        }

        void _textView_Changed(object sender, EventArgs e)
        {
            if (_placeholderLabel.Text.Length == 0) {
                return;
            }
            if (_textView.Text.Length == 0) {
                _placeholderLabel.Alpha = 1;
            }
            else {
                _placeholderLabel.Alpha = 0;
            }
        }

        void UpdateText()
        {
            _placeholderLabel.Text = Placeholder.GetText(Element);
        }

        void UpdateColor()
        {
            _placeholderLabel.TextColor = Placeholder.GetColor(Element).ToUIColor();
        }
    }
}
