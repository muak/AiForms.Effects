using System;
using Xamarin.Forms;

namespace AiForms.Effects
{
    /// <summary>
    /// Floating view.
    /// </summary>
    public class FloatingView : ContentView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Effects.FloatingView"/> class.
        /// </summary>
        public FloatingView()
        {
            CompressedLayout.SetIsHeadless(this, true);
        }

        /// <summary>
        /// The vertical layout alignment property.
        /// </summary>
        public static BindableProperty VerticalLayoutAlignmentProperty =
            BindableProperty.Create(
                nameof(VerticalLayoutAlignment),
                typeof(LayoutAlignment),
                typeof(FloatingView),
                LayoutAlignment.Center,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the vertical layout alignment.
        /// </summary>
        /// <value>The vertical layout alignment.</value>
        public LayoutAlignment VerticalLayoutAlignment
        {
            get { return (LayoutAlignment)GetValue(VerticalLayoutAlignmentProperty); }
            set { SetValue(VerticalLayoutAlignmentProperty, value); }
        }

        /// <summary>
        /// The horizontal layout alignment property.
        /// </summary>
        public static BindableProperty HorizontalLayoutAlignmentProperty =
            BindableProperty.Create(
                nameof(HorizontalLayoutAlignment),
                typeof(LayoutAlignment),
                typeof(FloatingView),
                LayoutAlignment.Center,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the horizontal layout alignment.
        /// </summary>
        /// <value>The horizontal layout alignment.</value>
        public LayoutAlignment HorizontalLayoutAlignment
        {
            get { return (LayoutAlignment)GetValue(HorizontalLayoutAlignmentProperty); }
            set { SetValue(HorizontalLayoutAlignmentProperty, value); }
        }

        /// <summary>
        /// The offset XP roperty.
        /// </summary>
        public static BindableProperty OffsetXProperty =
            BindableProperty.Create(
                nameof(OffsetX),
                typeof(int),
                typeof(FloatingView),
                default(int),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the offset x.
        /// </summary>
        /// <value>The offset x.</value>
        public int OffsetX
        {
            get { return (int)GetValue(OffsetXProperty); }
            set { SetValue(OffsetXProperty, value); }
        }

        /// <summary>
        /// The offset YP roperty.
        /// </summary>
        public static BindableProperty OffsetYProperty =
            BindableProperty.Create(
                nameof(OffsetY),
                typeof(int),
                typeof(FloatingView),
                default(int),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the offset y.
        /// </summary>
        /// <value>The offset y.</value>
        public int OffsetY
        {
            get { return (int)GetValue(OffsetYProperty); }
            set { SetValue(OffsetYProperty, value); }
        }

        /// <summary>
        /// The hidden property.
        /// </summary>
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

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:AiForms.Effects.FloatingView"/> is hidden.
        /// </summary>
        /// <value><c>true</c> if hidden; otherwise, <c>false</c>.</value>
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
