using System;
using Xamarin.Forms;

namespace AiForms.Effects
{
    public class FloatingView:ContentView
    {
        public FloatingView()
        {
            CompressedLayout.SetIsHeadless(this, true);
        }

        public static BindableProperty ProportionalWidthProperty =
            BindableProperty.Create(
                nameof(ProportionalWidth),
                typeof(double),
                typeof(FloatingView),
                -1d,
                defaultBindingMode: BindingMode.OneWay
            );

        public double ProportionalWidth
        {
            get { return (double)GetValue(ProportionalWidthProperty); }
            set { SetValue(ProportionalWidthProperty, value); }
        }

        public static BindableProperty ProportionalHeightProperty =
            BindableProperty.Create(
                nameof(ProportionalHeight),
                typeof(double),
                typeof(FloatingView),
                -1d,
                defaultBindingMode: BindingMode.OneWay
            );

        public double ProportionalHeight
        {
            get { return (double)GetValue(ProportionalHeightProperty); }
            set { SetValue(ProportionalHeightProperty, value); }
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
    }
}
