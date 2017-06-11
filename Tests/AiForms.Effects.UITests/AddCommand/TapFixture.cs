using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Xamarin.UITest;
using System.IO.IsolatedStorage;
using System.Linq;

namespace AiForms.Effects.UITests.AddCommand
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class TapFixture : TestFixtureBase
    {

        public TapFixture(Platform platform)
        {
            this.platform = platform;
        }

        [TestFixtureSetUp]
        protected override void StartApp()
        {
            base.StartApp();
            //menu -> AddCommandPage
            app.Tap("AddCommandTapTest");
            app.WaitForElement("AddCommandTapTest", "Timeout", TimeSpan.FromSeconds(10));
        }

        [Test]
        public async Task T01_ActivityIndicator()
        {
            await ExecutedAssert(Id.ActivityIndicator, true, true);
        }

        [Test]
        public async Task T02_BoxView()
        {
            await ExecutedAssert(Id.BoxView, true, true);
        }

        [Test]
        public async Task T03_Button()
        {
            await ExecutedAssert(Id.Button, true, true);
        }

        [Test]
        public async Task T04_DatePicker()
        {
            //iOS : DatePicker is not supported 動作したりしなかったり不安定
            //Android:Default DatePicker function overwritten 
            if (platform == Platform.iOS) {
                //タイミングによってできたりできなかったりなのでiOSはこのテストはスキップする
                return;
            }
            await ExecutedAssert(Id.DatePicker, OnPlatform(false, true), OnPlatform(false, true));
        }

        [Test]
        public async Task T05_Editor()
        {
            //iOS:最初だけ動くけど一回フォーカスが当たると動作しない
            //Android: not supported. long tap ok
            if (platform == Platform.iOS) {
                //タイミングによってできたりできなかったりなのでiOSはこのテストはスキップする
                return;
            }
            await ExecutedAssert(Id.Editor, false, true);
        }

        [Test]
        public async Task T06_Entry()
        {
            await ExecutedAssert(Id.Entry, false, OnPlatform(false, true));       //Entry is not supported
        }

        [Test]
        public async Task T07_Image()
        {
            await ExecutedAssert(Id.Image, true, true);
        }

        [Test]
        public async Task T08_Label()
        {
            await ExecutedAssert(Id.Label, true, true);
        }

        [Test]
        public async Task T09_ListView()
        {
            //Android:not supported
            await ExecutedAssert(Id.ListView, OnPlatform(true, false), OnPlatform(true, false));
        }

        [Test]
        public async Task T10_Picker()
        {
            //iOS:Picker is not supported
            //Android:Default DatePicker function overwritten 
            if (platform == Platform.iOS) {
                //タイミングによってできたりできなかったりなのでiOSはこのテストはスキップする
                return;
            }
            await ExecutedAssert(Id.Picker, OnPlatform(false, true), OnPlatform(false, true));
        }

        [Test]
        public async Task T11_ProgressBar()
        {
            await ExecutedAssert(Id.ProgressBar, true, true);
        }

        [Test]
        public async Task T12_SearchBar()
        {
            await ExecutedAssert(Id.SearchBar, false, false);     //SearchBar is not supported
        }

        [Test]
        public async Task T13_Slider()
        {
            //Android:not supported
            await ExecutedAssert(Id.Slider, OnPlatform(true, false), OnPlatform(true, false));
        }

        [Test]
        public async Task T14_Stepper()
        {
            //AndroidはStepperそのものをタップしても反応せず、余白タップで反応するので実質機能しない
            await ExecutedAssert(Id.Stepper, true, true);
        }

        [Test]
        public async Task T15_Switch()
        {
            //iOS:Switch is not supported
            await ExecutedAssert(Id.Switch, OnPlatform(false, true), OnPlatform(false, true));
        }

        [Test]
        public async Task T16_TableView()
        {
            await ExecutedAssert(Id.TableView, false, OnPlatform(true, false));   //TableView is not supported
        }

        [Test]
        public async Task T17_TimePicker()
        {
            //iOS:TimePicker is not supported
            //Android:Default DatePicker function overwritten 
            if (platform == Platform.iOS) {
                //タイミングによってできたりできなかったりなのでiOSはこのテストはスキップする
                return;
            }
            await ExecutedAssert(Id.TimePicker, OnPlatform(false, true), OnPlatform(false, true));
        }

        [Test]
        public async Task T18_WebView()
        {
            //iOS: not supported
            //Android:long tap only
            if (platform == Platform.iOS) {
                //タイミングによってできたりできなかったりなのでiOSはこのテストはスキップする
                return;
            }
            await ExecutedAssert(Id.WebView, false, OnPlatform(false, true));
        }

        [Test]
        public async Task T19_ContentPresenter()
        {
            await ExecutedAssert(Id.ContentPresenter, true, true);
        }

        [Test]
        public async Task T20_ContentView()
        {
            await ExecutedAssert(Id.ContentView, true, true);
        }

        [Test]
        public async Task T21_Frame()
        {
            //Android:not supported
            await ExecutedAssert(Id.Frame, OnPlatform(true, false), OnPlatform(true, false));
        }

        [Test]
        public async Task T22_ScrollView()
        {
            // XF2.3.4 iOS supported
            await ExecutedAssert(Id.ScrollView, OnPlatform(true, false),OnPlatform(true, false));  //ScrollView is not supported
        }

        [Test]
        public async Task T23_TemplatedView()
        {
            await ExecutedAssert(Id.TemplatedView, true, true);
        }

        [Test]
        public async Task T24_AbsoluteLayout()
        {
            await ExecutedAssert(Id.AbsoluteLayout, true, true);
        }

        [Test]
        public async Task T25_Grid()
        {
            await ExecutedAssert(Id.Grid, true, true);
        }

        [Test]
        public async Task T26_RelativeLayout()
        {
            await ExecutedAssert(Id.RelativeLayout, true, true);
        }

        [Test]
        public async Task T27_StackLayout()
        {
            await ExecutedAssert(Id.StackLayout, true, true);
        }


        [Test]
        public async Task T40_ToggleEffectOnOff()
        {
            ScrollDownTo("PropTestView");
            app.WaitForElement("PropTestView");

            //ON->OFF
            app.Tap("ToggleEffect");
            await Task.Delay(250);

            app.Tap("PropTestView");
            await Task.Delay(250);

            Assert.IsFalse(app.Query("ResultExecute")[0].Enabled);
            Assert.IsNullOrEmpty(app.Query("ResultText")[0].Text);

            app.TouchAndHold("PropTestView");
            await Task.Delay(250);

            Assert.IsFalse(app.Query("ResultLong")[0].Enabled);
            Assert.IsNullOrEmpty(app.Query("ResultText")[0].Text);

            app.Tap("ResetResult");
            await Task.Delay(250);

            //OFF->ON
            app.Tap("ToggleEffect");
            await Task.Delay(250);

            app.Tap("PropTestView");
            await Task.Delay(250);

            Assert.IsTrue(app.Query("ResultExecute")[0].Enabled);
            Assert.AreEqual("Hoge", app.Query("ResultText")[0].Text);

            app.TouchAndHold("PropTestView");
            await Task.Delay(250);

            Assert.IsTrue(app.Query("ResultLong")[0].Enabled);
            Assert.AreEqual("LongHoge", app.Query("ResultText")[0].Text);

            app.Tap("ResetResult");
            await Task.Delay(250);

        }

        [Test]
        public async Task T41_CommandParameterChange()
        {
            ScrollDownTo("PropTestView");
            app.WaitForElement("PropTestView");

            app.Tap("ChParam");
            await Task.Delay(250);

            app.Tap("PropTestView");
            await Task.Delay(250);

            Assert.IsTrue(app.Query("ResultExecute")[0].Enabled);
            Assert.AreEqual("Fuga", app.Query("ResultText")[0].Text);

            app.Tap("ResetResult");
            await Task.Delay(250);

            app.Tap("ChLongParam");
            await Task.Delay(250);

            app.TouchAndHold("PropTestView");
            await Task.Delay(250);

            Assert.IsTrue(app.Query("ResultLong")[0].Enabled);
            Assert.AreEqual("LongFuga", app.Query("ResultText")[0].Text);

            app.Tap("ResetResult");
            await Task.Delay(250);
        }

        [Test]
        public async Task T42_CommandChange()
        {
            ScrollDownTo("PropTestView");
            app.WaitForElement("PropTestView");

            //Command null LongCommand not null
            app.Tap("ChCommand");
            await Task.Delay(250);
            await CommandTest(false, true);

            //Command null LongCommand null
            app.Tap("ChLongCommand");
            await Task.Delay(250);
            await CommandTest(false, false);

            //Command not null LongCommand null
            app.Tap("ChCommand");
            await Task.Delay(250);
            await CommandTest(true, false);

        }

        async Task CommandTest(bool command, bool longCommand)
        {
            app.Tap("PropTestView");
            await Task.Delay(250);

            Assert.AreEqual(command, app.Query("ResultExecute")[0].Enabled);

            app.Tap("ResetResult");
            await Task.Delay(250);

            app.TouchAndHold("PropTestView");
            await Task.Delay(250);

            Assert.AreEqual(longCommand, app.Query("ResultLong")[0].Enabled);

            app.Tap("ResetResult");
            await Task.Delay(250);
        }

        [Test]
        public async Task T43_EffectColorChange()
        {
            ScrollDownTo("PropTestView");
            app.WaitForElement("PropTestView");

            //Command null LongCommand not null
            app.Tap("ChColor");
            await Task.Delay(250);

            app.Tap("PropTestView");
            app.TouchAndHold("PropTestView");
        }

        [Test]
        public async Task T50_RippleOffAll()
        {
            ScrollUpTo(Id.ActivityIndicator);
            app.WaitForElement(Id.ActivityIndicator);

            app.Tap("ChRipple");
            await Task.Delay(250);

            foreach (var v in Id.Items) {
                ScrollDownTo(v);
                app.WaitForElement(v);
                app.Tap(v);
                app.Tap("ResetResult");
                app.TouchAndHold(v);
                app.Tap("ResetResult");
            }

            app.Tap("ChRipple");
            await Task.Delay(250);
        }

        [Test]
        public async Task T51_EffectOffAll()
        {
            ScrollUpTo(Id.ActivityIndicator);
            app.WaitForElement(Id.ActivityIndicator);

            app.Tap("ToggleEffect");
            await Task.Delay(250);

            foreach (var v in Id.Items) {
                ScrollDownTo(v);
                app.WaitForElement(v);
                app.Tap(v);
                app.Tap("ResetResult");
                app.TouchAndHold(v);
                app.Tap("ResetResult");
            }

            app.Tap("ToggleEffect");
            await Task.Delay(250);
        }

        [Test]
        public void T99_GoBackTest()
        {
            app.Back();
        }


        async Task ExecutedAssert(string view, bool expected, bool longExpected)
        {
            ScrollDownTo(view);
            app.WaitForElement(view);
            //Tap
            app.Tap(view);

            await Task.Delay(500);
            var ret = app.Query("ResultExecute")[0];
            var label = app.Query("ResultText")[0];

            Assert.AreEqual(expected, ret.Enabled, $"{view} command error");

            if (expected) {
                label.Text.Is(view, $"{view} parameter error");
            }
            else {
                string.IsNullOrEmpty(label.Text).IsTrue();
            }

            app.Tap("ResetResult");
            await Task.Delay(250);

            //LongTap
            app.TouchAndHold(view);
            await Task.Delay(500);

            ret = app.Query("ResultLong")[0];
            label = app.Query("ResultText")[0];

            Assert.AreEqual(longExpected, ret.Enabled, $"{view} long command error");

            if (longExpected) {
                label.Text.Is("Long" + view, $"{view} long parameter error");
            }
            else {
                string.IsNullOrEmpty(label.Text).IsTrue();
            }

            app.Tap("ResetResult");
            await Task.Delay(250);

            if(!expected){
                return;
            }

            //CanExecute false
            app.Tap("ChCanExecute");
            await Task.Delay(250);

            //Tap
            app.Tap(view);

            await Task.Delay(250);
            ret = app.Query("ResultExecute")[0];
            label = app.Query("ResultText")[0];

            Assert.AreEqual(!expected, ret.Enabled, $"{view} command error");

            string.IsNullOrEmpty(label.Text).IsTrue();

            app.Tap("ResetResult");
            await Task.Delay(250);

            //LongTap
            app.TouchAndHold(view);
            await Task.Delay(500);

            ret = app.Query("ResultLong")[0];
            label = app.Query("ResultText")[0];

            Assert.AreEqual(!longExpected, ret.Enabled, $"{view} long command error");

            string.IsNullOrEmpty(label.Text).IsTrue();

            app.Tap("ResetResult");
            await Task.Delay(250);

            //CanExecute true
            app.Tap("ChCanExecute");
            await Task.Delay(250);
        }

    }
}
