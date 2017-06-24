﻿using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using UIKit;
using AiForms.Effects;
using AiForms.Effects.iOS;

[assembly: ExportEffect(typeof(AlterColorPlatformEffect), nameof(AlterColor))]
namespace AiForms.Effects.iOS
{
    public class AlterColorPlatformEffect : PlatformEffect
    {
        IAlterColorEffect _effect;

        protected override void OnAttached()
        {
            if (Element is Slider) {
                _effect = new AlterColorSlider(Control as UISlider, Element);
            }
            else if (Element is Switch) {
                _effect = new AlterColorSwitch(Control as UISwitch, Element);
            }
            else {
                return;
            }

            _effect?.Update();
        }

        protected override void OnDetached()
        {
            _effect?.OnDetached();
            _effect = null;
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
            if (args.PropertyName == AlterColor.AccentProperty.PropertyName) {
                _effect?.Update();
            }
        }
    }
}
