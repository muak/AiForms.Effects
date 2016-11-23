using System.Windows.Input;
using AiForms.Effects;
using AiForms.Effects.iOS;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("AiForms")]
[assembly: ExportEffect(typeof(AddCommandPlatformEffect), nameof(AddCommand))]
namespace AiForms.Effects.iOS
{
    public class AddCommandPlatformEffect:PlatformEffect
	{
		
		private ICommand command;
		private object commandParameter;
		private UITapGestureRecognizer tapGesture;
		private UIView view;
		private UIView layer;

		protected override void OnAttached() {
			view = Control ?? Container;

			tapGesture = new UITapGestureRecognizer(async(obj) => {
				if (layer != null) {
					layer.Frame = new CGRect(0, 0, view.Bounds.Width, view.Bounds.Height);
					view.AddSubview(layer);
					view.BringSubviewToFront(layer);
					layer.Alpha = 1;
					await UIView.AnimateAsync(0.3f, () => {
						layer.Alpha = 0;
					});
					layer.RemoveFromSuperview();
				}

				command?.Execute(commandParameter ?? Element);
			});

            view.UserInteractionEnabled = true;
			view.AddGestureRecognizer(tapGesture);

			UpdateCommand();
			UpdateCommandParameter();
			UpdateEffectColor();
		}

		protected override void OnDetached() {

			view.RemoveGestureRecognizer(tapGesture);
			tapGesture.Dispose();
			if (layer != null) {
				layer.Dispose();
				layer = null;
			}
		}

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e) {
            base.OnElementPropertyChanged(e);

            if (e.PropertyName == AddCommand.CommandProperty.PropertyName) {
                UpdateCommand();
            }
            else if (e.PropertyName == AddCommand.CommandParameterProperty.PropertyName) {
                UpdateCommandParameter();
            }
            else if (e.PropertyName == AddCommand.EffectColorProperty.PropertyName) {
                UpdateEffectColor();
            }
           
        }

		void UpdateCommand() {
			command = AddCommand.GetCommand(Element);
		}
		void UpdateCommandParameter() {
			commandParameter = AddCommand.GetCommandParameter(Element);
		}
		void UpdateEffectColor() {
			
			if (layer != null) {
				layer.Dispose();
				layer = null;
			}

			var color = AddCommand.GetEffectColor(Element);
			if (color == Xamarin.Forms.Color.Default) {
				return;
			}

			layer = new UIView();
			layer.BackgroundColor = color.ToUIColor();

		}
	}
}

