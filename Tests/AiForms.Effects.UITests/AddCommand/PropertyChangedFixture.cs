using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Xamarin.UITest;
namespace AiForms.Effects.UITests.AddCommand
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class PropertyChangedFixture:TestFixtureBase
    {
        public PropertyChangedFixture(Platform platform)
        {
            this.platform = platform;
        }

        [TestFixtureSetUp]
        public void StartApp()
        {
            app = AppInitializer.StartApp(platform);
            app.Tap("AddCommandPropTest");
            app.WaitForElement("AddCommandPropTest", "Timeout", TimeSpan.FromSeconds(10));
        }


        [Test]
        public async Task T01_ToggleEffectOnOff()
        {
            //OFF
            app.Tap("TestLabel");
            await Task.Delay(250);

            Assert.IsFalse(app.Query("ResultExecute")[0].Enabled);
            Assert.IsNullOrEmpty(app.Query("ParamText")[0].Text);

            //OFF->ON
            app.Tap("ToggleEffect");
            await Task.Delay(250);

            app.Tap("TestLabel");
            await Task.Delay(250);

            Assert.IsTrue(app.Query("ResultExecute")[0].Enabled);
            Assert.AreEqual("Hoge",app.Query("ParamText")[0].Text);

            app.Tap("ResetResult");
            await Task.Delay(250);

            //ON->OFF
            app.Tap("ToggleEffect");
            await Task.Delay(250);

            app.Tap("TestLabel");
            await Task.Delay(250);

            Assert.IsFalse(app.Query("ResultExecute")[0].Enabled);
            Assert.IsNullOrEmpty(app.Query("ParamText")[0].Text);


        }

        [Test]
        public async Task T02_CommandParameterChange()
        {
            //OFF->ON
            app.Tap("ToggleEffect");
            await Task.Delay(250);

            app.Tap("ChParam");
            await Task.Delay(250);

            app.Tap("TestLabel");
            await Task.Delay(250);

            Assert.IsTrue(app.Query("ResultExecute")[0].Enabled);
            Assert.AreEqual("Fuga", app.Query("ParamText")[0].Text);

            //ON->OFF
            app.Tap("ToggleEffect");
            await Task.Delay(250);
        }

        [Test]
        public async Task T03_EffectColorChange()
        {
            //OFF->ON
            app.Tap("ToggleEffect");
            await Task.Delay(250);

            app.Tap("ChColor");
            await Task.Delay(250);

            app.Tap("TestLabel");
            app.Screenshot("");
            app.Tap("TestLabel");
            app.Screenshot("");
            app.Tap("TestLabel");
            app.Screenshot("");
            app.Tap("TestLabel");
            app.Screenshot("");
        }
    }
}
