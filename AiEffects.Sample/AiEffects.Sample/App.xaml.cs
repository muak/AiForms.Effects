using Prism.Unity;
using AiEffects.Sample.Views;
using System.Reflection;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using Xamarin.Forms;

namespace AiEffects.Sample
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized() {
            InitializeComponent();

            if (Device.OS == TargetPlatform.iOS) {
                NavigationService.NavigateAsync("NaviA/AddCommandPage");
            }
            else if (Device.OS == TargetPlatform.Android) {
                NavigationService.NavigateAsync("NaviA/MyTabbed");
            }
        }

        protected override void RegisterTypes() {
            this.GetType().GetTypeInfo().Assembly
            .DefinedTypes
            .Where(t => t.Namespace.EndsWith(".Views", System.StringComparison.Ordinal))
            .ForEach(t => {
                Container.RegisterTypeForNavigation(t.AsType(), t.Name);
            });
        }
    }
}

