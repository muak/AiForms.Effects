using System;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;

namespace AiForms.Effects.UITests
{
    public abstract class TestFixtureBase
    {
        protected virtual IApp app { get; set; }
        protected virtual Platform platform { get; set; }

        [TestFixtureSetUp]
        protected virtual void StartApp()
        {
            app = AppInitializer.StartApp(platform);
        }

        protected T OnPlatform<T>(T ios, T android)
        {
            if (platform == Platform.iOS) {
                return ios;
            }
            else {
                return android;
            }
        }

        protected void ScrollUpTo(string view)
        {
            if (platform == Platform.iOS) {
                app.ScrollUpTo(view);
            }
            else {
                app.ScrollTo(view);
            }
        }

        protected void ScrollDownTo(string view)
        {
            if (platform == Platform.iOS) {
                app.ScrollDownTo(view);
            }
            else {
                app.ScrollTo(view);
            }
        }


    }
}
