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

        protected override void OnAttachedOverride()
        {
            if (Element is Label) {
                _effect = new LineHeightForTextView(Container, Control, Element);
            }
            else if (Element is Editor) {
                _effect = new LineHeightForEditText(Container, Control, Element);
            }

            _effect?.Update();

        }

        protected override void OnDetachedOverride()
        {
            if (!IsDisposed) {
                _effect.OnDetachedIfNotDisposed();
                System.Diagnostics.Debug.WriteLine($"{this.GetType().FullName} Detached Disposing");
            }
            _effect?.OnDetached();
            _effect = null;
            System.Diagnostics.Debug.WriteLine($"{this.GetType().FullName} Detached completely");
        }


        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (!IsSupportedByApi)
                return;

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

