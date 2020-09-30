using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;
using Android.Content.Res;
using AndroidX.AppCompat.Widget;

namespace AiForms.Effects.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class AlterColorSwitch : IAiEffectDroid
    {
        SwitchCompat _aSwitch;
        Element _element;

        Drawable _orgTrack;
        Drawable _orgThumb;
        RippleDrawable _orgBackground;
        Drawable _track;
        Drawable _thumb;
        RippleDrawable _background;

        public AlterColorSwitch(SwitchCompat aswitch, Element element)
        {
            _aSwitch = aswitch;
            _element = element;

            _orgBackground = _aSwitch.Background as RippleDrawable;

            _orgTrack = _aSwitch.TrackDrawable;
            _orgThumb = _aSwitch.ThumbDrawable;

            _background = _orgBackground.GetConstantState().NewDrawable() as RippleDrawable;
            _track = _aSwitch.TrackDrawable.GetConstantState().NewDrawable();
            _thumb = _aSwitch.ThumbDrawable.GetConstantState().NewDrawable();

            _track.SetState(_orgTrack.GetState());
            _thumb.SetState(_orgThumb.GetState());

            _aSwitch.Background = _background;
            _aSwitch.TrackDrawable = _track;
            _aSwitch.ThumbDrawable = _thumb;
        }

        public void OnDetachedIfNotDisposed()
        {
            _track.SetTintList(null);
            _thumb.SetTintList(null);

            //restore like default color but not completely.
            var color = Xamarin.Forms.Color.Accent.ToAndroid();
            var trackColors = new ColorStateList(new int[][]
            {
                new int[]{global::Android.Resource.Attribute.StateChecked},
                new int[]{-global::Android.Resource.Attribute.StateChecked},
            },
                new int[] {
                    Android.Graphics.Color.Argb(76,color.R,color.G,color.B),
                    Android.Graphics.Color.Argb(76, 50, 50, 50)
            });

            _orgTrack.SetTintList(trackColors);
            _orgTrack.SetState(_track.GetState());
            _orgThumb.SetState(_thumb.GetState());
            _orgBackground.SetState(_background.GetState());

            _aSwitch.TrackDrawable = _orgTrack;
            _aSwitch.ThumbDrawable = _orgThumb;
            _aSwitch.Background = _orgBackground;
        }

        public void OnDetached()
        {
            _track.Dispose();
            _thumb.Dispose();
            _background.Dispose();
            _background = null;
            _orgBackground = null;
            _orgThumb = null;
            _orgTrack = null;
            _track = null;
            _thumb = null;
            _aSwitch = null;
            _element = null;
        }

        public void Update()
        {
            var color = AlterColor.GetAccent(_element).ToAndroid();

            var trackColors = new ColorStateList(new int[][]
            {
                new int[]{global::Android.Resource.Attribute.StateChecked},
                new int[]{-global::Android.Resource.Attribute.StateChecked},
            },
                new int[] {
                    Android.Graphics.Color.Argb(76,color.R,color.G,color.B),
                    Android.Graphics.Color.Argb(76, 50, 50, 50)
                }
            );

            _track.SetTintList(trackColors);

            var thumbColors = new ColorStateList(new int[][]
            {
                new int[]{global::Android.Resource.Attribute.StateChecked},
                new int[]{-global::Android.Resource.Attribute.StateChecked},
            },
                new int[] {
                    color,
                    Android.Graphics.Color.Argb(255, 244, 244, 244)
                }
            );

            _thumb.SetTintList(thumbColors);

            _background.SetColor(trackColors);
        }
    }
}
