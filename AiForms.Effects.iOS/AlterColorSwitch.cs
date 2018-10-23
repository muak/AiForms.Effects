using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
namespace AiForms.Effects.iOS
{
    [Foundation.Preserve(AllMembers = true)]
    public class AlterColorSwitch : IAiEffect
    {
        UISwitch _uiswitch;
        Element _element;
        UIColor _originalColor;

        public AlterColorSwitch(UISwitch uiswitch, Element element)
        {
            _uiswitch = uiswitch;
            _element = element;
            _originalColor = _uiswitch.OnTintColor;
        }

        public void OnDetached()
        {
            _uiswitch.OnTintColor = _originalColor;
            _uiswitch = null;
            _element = null;
        }

        public void Update()
        {
            var color = AlterColor.GetAccent(_element).ToUIColor();
            _uiswitch.OnTintColor = color;
        }
    }
}
