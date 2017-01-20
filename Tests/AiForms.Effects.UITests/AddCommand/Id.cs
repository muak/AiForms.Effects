using System;
using System.Collections.Generic;
using NUnit.Framework;
namespace AiForms.Effects.UITests.AddCommand
{
    internal static class Id
    {
        public static readonly string ActivityIndicator = nameof(ActivityIndicator);
        public static readonly string BoxView = nameof(BoxView);
        public static readonly string Button = nameof(Button);
        public static readonly string DatePicker = nameof(DatePicker);
        public static readonly string Editor = nameof(Editor);
        public static readonly string Entry = nameof(Entry);
        public static readonly string Image = nameof(Image);
        public static readonly string Label = nameof(Label);
        public static readonly string ListView = nameof(ListView);
        public static readonly string Picker = nameof(Picker);
        public static readonly string ProgressBar = nameof(ProgressBar);
        public static readonly string SearchBar = nameof(SearchBar);
        public static readonly string Slider = nameof(Slider);
        public static readonly string Stepper = nameof(Stepper);
        public static readonly string Switch = nameof(Switch);
        public static readonly string TableView = nameof(TableView);
        public static readonly string TimePicker = nameof(TimePicker);
        public static readonly string WebView = nameof(WebView);
        public static readonly string ContentPresenter = nameof(ContentPresenter);
        public static readonly string ContentView = nameof(ContentView);
        public static readonly string Frame = nameof(Frame);
        public static readonly string ScrollView = nameof(ScrollView);
        public static readonly string TemplatedView = nameof(TemplatedView);
        public static readonly string AbsoluteLayout = nameof(AbsoluteLayout);
        public static readonly string Grid = nameof(Grid);
        public static readonly string RelativeLayout = nameof(RelativeLayout);
        public static readonly string StackLayout = nameof(StackLayout);

        public static readonly List<string> Items;

        static Id()
        {
            Items = new List<string>() {
                nameof( ActivityIndicator   ),
                nameof( BoxView     ),
                nameof( Button  ),
                nameof( DatePicker  ),
                nameof( Editor  ),
                nameof( Entry   ),
                nameof( Image   ),
                nameof( Label   ),
                nameof( ListView    ),
                nameof( Picker  ),
                nameof( ProgressBar     ),
                nameof( SearchBar   ),
                nameof( Slider  ),
                nameof( Stepper     ),
                nameof( Switch  ),
                nameof( TableView   ),
                nameof( TimePicker  ),
                nameof( WebView     ),
                nameof( ContentPresenter    ),
                nameof( ContentView     ),
                nameof( Frame   ),
                nameof( ScrollView  ),
                nameof( TemplatedView   ),
                nameof( AbsoluteLayout  ),
                nameof( Grid    ),
                nameof( RelativeLayout  ),
                nameof( StackLayout     ),
            };
        }
    }


}
