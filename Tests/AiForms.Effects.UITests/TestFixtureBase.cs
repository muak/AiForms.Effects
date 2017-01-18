using System;
using Xamarin.UITest;

namespace AiForms.Effects.UITests
{
    public abstract class TestFixtureBase
    {
        protected virtual IApp app { get; set; }
        protected virtual Platform platform { get; set; }

        protected T OnPlatform<T>(T ios, T android)
        {
            if (platform == Platform.iOS) {
                return ios;
            }
            else {
                return android;
            }
        }
    }
}
