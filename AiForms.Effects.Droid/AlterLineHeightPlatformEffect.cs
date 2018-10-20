using AiForms.Effects;
using AiForms.Effects.Droid;
using Xamarin.Forms;

[assembly: ExportEffect(typeof(AlterLineHeightPlatformEffect), nameof(AlterLineHeight))]
namespace AiForms.Effects.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class AlterLineHeightPlatformEffect : AiEffectBase
    {
        private IAiEffectDroid _effect;

        protected override void OnAttached()
        {
            if (Element is Label) {
                _effect = new LineHeightForTextView(Container, Control, Element);
            }
            else if (Element is Editor) {
                _effect = new LineHeightForEditText(Container, Control, Element);
            }

            _effect?.Update();

        }

        protected override void OnDetached()
        {
            if (!IsDisposed) {
                _effect.OnDetachedIfNotDisposed();
            }
            _effect?.OnDetached();
            _effect = null;
        }


        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (IsDisposed) {
                return;
            }

            if (e.PropertyName == AlterLineHeight.MultipleProperty.PropertyName) {
                _effect?.Update();
            }

            else if (e.PropertyName == Label.FontSizeProperty.PropertyName) {
                _effect?.Update();
            }

        }
    }
}

