using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Xamarin.UITest;
using System.IO.IsolatedStorage;

namespace AiForms.Effects.UITests.AddCommand
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class TapFixture:TestFixtureBase
    {

        public TapFixture(Platform platform)
        {
            this.platform = platform;
        }

        [TestFixtureSetUp]
        public void StartApp()
        {
            app = AppInitializer.StartApp(platform);
            //menu -> AddCommandPage
            app.Tap("AddCommandTapTest");
            app.WaitForElement("AddCommandTapTest", "Timeout", TimeSpan.FromSeconds(10));
        }

        [Test]
        public async Task T01_ActivityIndicator()
        {
            await ExecutedAssert("ActivityIndicator", true);
        }

        [Test]
        public async Task T02_BoxView()
        {
            await ExecutedAssert("BoxView", true);
        }

        [Test]
        public async Task T03_Button()
        {
            await ExecutedAssert("Button", true);
        }

        [Test]
        public async Task T04_DatePicker()
        {
            //iOS : DatePicker is not supported 
            //Android:Default DatePicker function overwritten 
            await ExecutedAssert("DatePicker", OnPlatform(false,true));   
        }

        [Test]
        public async Task T05_Editor()
        {
            //Android: not supported
            await ExecutedAssert("Editor", OnPlatform(true,false));
        }

        [Test]
        public async Task T06_Entry()
        {
            await ExecutedAssert("Entry", false);       //Entry is not supported
        }

        [Test]
        public async Task T07_Image()
        {
            await ExecutedAssert("Image", true);
        }

        [Test]
        public async Task T08_Label()
        {
            await ExecutedAssert("Label", true);
        }

        [Test]
        public async Task T09_ListView()
        {
            //Android:not supported
            await ExecutedAssert("ListView", OnPlatform(true,false));
        }

        [Test]
        public async Task T10_Picker()
        {
            //iOS:Picker is not supported
            //Android:Default DatePicker function overwritten 
            await ExecutedAssert("Picker", OnPlatform(false,true));      
        }

        [Test]
        public async Task T11_ProgressBar()
        {
            await ExecutedAssert("ProgressBar", true);
        }

        [Test]
        public async Task T12_SearchBar()
        {
            await ExecutedAssert("SearchBar", false);     //SearchBar is not supported
        }

        [Test]
        public async Task T13_Slider()
        {
            //Android:not supported
            await ExecutedAssert("Slider", OnPlatform(true,false));
        }

        [Test]
        public async Task T14_Stepper()
        {
            await ExecutedAssert("Stepper", true);
        }

        [Test]
        public async Task T15_Switch()
        {
            //iOS:Switch is not supported
            await ExecutedAssert("Switch", OnPlatform(false,true));      
        }

        [Test]
        public async Task T16_TableView()
        {
            await ExecutedAssert("TableView", false);   //TableView is not supported
        }

        [Test]
        public async Task T17_TimePicker()
        {
            //iOS:TimePicker is not supported
            //Android:Default DatePicker function overwritten 
            await ExecutedAssert("TimePicker",OnPlatform(false,true));    
        }

        [Test]
        public async Task T18_WebView()
        {
            await ExecutedAssert("WebView", false);    //WebView is not supported
        }

        [Test]
        public async Task T19_ContentPresenter()
        {
            await ExecutedAssert("ContentPresenter", true);
        }

        [Test]
        public async Task T20_ContentView()
        {
            await ExecutedAssert("ContentView", true);
        }

        [Test]
        public async Task T21_Frame()
        {
            //Android:not supported
            await ExecutedAssert("Frame", OnPlatform(true,false));
        }

        [Test]
        public async Task T22_ScrollView()
        {
            await ExecutedAssert("ScrollView", false);  //ScrollView is not supported
        }

        [Test]
        public async Task T23_TemplatedView()
        {
            await ExecutedAssert("TemplatedView", true);
        }

        [Test]
        public async Task T24_AbsoluteLayout()
        {
            await ExecutedAssert("AbsoluteLayout", true);
        }

        [Test]
        public async Task T25_Grid()
        {
            await ExecutedAssert("Grid", true);
        }

        [Test]
        public async Task T26_RelativeLayout()
        {
            await ExecutedAssert("RelativeLayout", true);
        }

        [Test]
        public async Task T27_StackLayout()
        {
            await ExecutedAssert("StackLayout", true);
        }

        async Task ExecutedAssert(string view, bool expected)
        {
            app.ScrollDownTo(view);
            app.Tap(view);

            await Task.Delay(250);
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
        }
    }
}
