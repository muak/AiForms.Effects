using System;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using System.ComponentModel;
using Android.Views;

namespace AiForms.Effects.Droid
{
    public class FloatingViewRenderer:ViewRenderer<FloatingView,FormsViewGroup>
    {
        public FloatingViewRenderer(Context context):base(context)
        {
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if(e.PropertyName == "IsVisible")
            {
            }
        }
    }
}
