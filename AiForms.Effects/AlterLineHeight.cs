using System;
using System.Linq;
using Xamarin.Forms;

namespace AiForms.Effects
{
	public static class AlterLineHeight
	{
        public static readonly BindableProperty OnProperty =
		BindableProperty.CreateAttached(
				"On",
				typeof(bool),
				typeof(AlterLineHeight),
				false,
				propertyChanged: OnOffChanged
			);

        public static void SetOn(BindableObject view, bool value) {
			view.SetValue(OnProperty, value);
		}

        public static bool GetOn(BindableObject view) {
			return (bool)view.GetValue(OnProperty);
		}

        private static void OnOffChanged(BindableObject bindable, object oldValue, object newValue) {
			var view = bindable as View;
			if (view == null)
				return;
			
			if ((bool)newValue) {
				view.Effects.Add(new LineHeightEffectRoutingEffect());
			}
			else {
				var toRemove = view.Effects.FirstOrDefault(e => e is LineHeightEffectRoutingEffect);
				if (toRemove != null)
					view.Effects.Remove(toRemove);
			}
		}

        public static readonly BindableProperty MultipleProperty =
            BindableProperty.CreateAttached(
                    "Multiple",
                    typeof(double),
                    typeof(AlterLineHeight),
                    default(double)
                );

        public static void SetMultiple(BindableObject view, double value) {
            view.SetValue(MultipleProperty, value);
        }

        public static double GetMultiple(BindableObject view) {
            return (double)view.GetValue(MultipleProperty);
        }


		class LineHeightEffectRoutingEffect : RoutingEffect
		{
			public LineHeightEffectRoutingEffect() : base("AiForms." + nameof(AlterLineHeight)) {

			}
		}
	}
}

