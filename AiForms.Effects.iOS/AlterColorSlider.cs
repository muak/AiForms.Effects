using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
namespace AiForms.Effects.iOS
{
    [Foundation.Preserve(AllMembers = true)]
    public class AlterColorSlider : IAiEffect
    {
        UISlider _slider;
        Element _element;

        UIColor _originalMinColor;
        UIColor _originalMaxColor;

        public AlterColorSlider(UISlider slider, Element element)
        {
            _slider = slider;
            _element = element;
            _originalMinColor = _slider.MinimumTrackTintColor;
            _originalMaxColor = _slider.MaximumTrackTintColor;
        }

        public void OnDetached()
        {
            _slider.MinimumTrackTintColor = _originalMinColor;
            _slider.MaximumTrackTintColor = _originalMaxColor;
            _slider = null;
            _element = null;
        }

        public void Update()
        {
            var color = AlterColor.GetAccent(_element).ToUIColor();
            _slider.MinimumTrackTintColor = color;
            _slider.MaximumTrackTintColor = color.ColorWithAlpha(0.3f);
        }
    }
}
