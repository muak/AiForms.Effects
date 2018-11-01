using AiForms.Effects;
using AiForms.Effects.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(AlterLineHeightPlatformEffect), nameof(AlterLineHeight))]
namespace AiForms.Effects.iOS
{
    [Foundation.Preserve(AllMembers = true)]
    public class AlterLineHeightPlatformEffect : PlatformEffect
    {

        private IAiEffect _effect;

        protected override void OnAttached()
        {
            if (Element is Label) {
                _effect = new LineHeightForLabel(Container, Control, Element);
            }
            else if (Element is Editor) {
                _effect = new LineHeightForTextView(Container, Control, Element);
            }

            _effect?.Update();
        }

        protected override void OnDetached()
        {
            _effect?.OnDetached();
            _effect = null;

            System.Diagnostics.Debug.WriteLine($"Detached {GetType().Name} from {Element.GetType().FullName}");
        }


        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (e.PropertyName == AlterLineHeight.MultipleProperty.PropertyName) {
                _effect?.Update();
            }
            else if (e.PropertyName == Label.FontSizeProperty.PropertyName) {
                _effect?.Update();
            }
            else if (e.PropertyName == Label.TextProperty.PropertyName && _effect is LineHeightForLabel) {
                _effect?.Update();
            }
            else if (e.PropertyName == Label.HorizontalTextAlignmentProperty.PropertyName && _effect is LineHeightForLabel) {
                _effect?.Update();
            }

        }


    }
}

