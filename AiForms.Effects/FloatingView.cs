using System;
using Xamarin.Forms;

namespace AiForms.Effects
{
    public class FloatingView : ContentView
    {
        public FloatingView()
        {
            CompressedLayout.SetIsHeadless(this, true);
        }

        public static BindableProperty VerticalLayoutAlignmentProperty =
            BindableProperty.Create(
                nameof(VerticalLayoutAlignment),
                typeof(LayoutAlignment),
                typeof(FloatingView),
                LayoutAlignment.Center,
                defaultBindingMode: BindingMode.OneWay
            );

        public LayoutAlignment VerticalLayoutAlignment
        {
            get { return (LayoutAlignment)GetValue(VerticalLayoutAlignmentProperty); }
            set { SetValue(VerticalLayoutAlignmentProperty, value); }
        }

        public static BindableProperty HorizontalLayoutAlignmentProperty =
            BindableProperty.Create(
                nameof(HorizontalLayoutAlignment),
                typeof(LayoutAlignment),
                typeof(FloatingView),
                LayoutAlignment.Center,
                defaultBindingMode: BindingMode.OneWay
            );

        public LayoutAlignment HorizontalLayoutAlignment
        {
            get { return (LayoutAlignment)GetValue(HorizontalLayoutAlignmentProperty); }
            set { SetValue(HorizontalLayoutAlignmentProperty, value); }
        }

        public static BindableProperty OffsetXProperty =
            BindableProperty.Create(
                nameof(OffsetX),
                typeof(int),
                typeof(FloatingView),
                default(int),
                defaultBindingMode: BindingMode.OneWay
            );

        public int OffsetX
        {
            get { return (int)GetValue(OffsetXProperty); }
            set { SetValue(OffsetXProperty, value); }
        }

        public static BindableProperty OffsetYProperty =
            BindableProperty.Create(
                nameof(OffsetY),
                typeof(int),
                typeof(FloatingView),
                default(int),
                defaultBindingMode: BindingMode.OneWay
            );

        public int OffsetY
        {
            get { return (int)GetValue(OffsetYProperty); }
            set { SetValue(OffsetYProperty, value); }
        }

        public static BindableProperty HiddenProperty =
            BindableProperty.Create(
                nameof(Hidden),
                typeof(bool),
                typeof(FloatingView),
                default(bool),
                defaultBindingMode: BindingMode.OneWay,
                propertyChanged: (bindable, oldValue, newValue) => {
                    bindable.SetValue(OpacityProperty, (bool)newValue ? 0 : 1);
                    bindable.SetValue(InputTransparentProperty, (bool)newValue);
                }
            );

        public bool Hidden
        {
            get { return (bool)GetValue(HiddenProperty); }
            set { SetValue(HiddenProperty, value); }
        }

        // kill exists properties.
        private new LayoutOptions VerticalOptions { get; set; }
        private new LayoutOptions HorizontalOptions { get; set; }
    }
}
