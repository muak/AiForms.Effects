using System.Linq;
using System.Reflection;
using Microsoft.Practices.ObjectBuilder2;
using Prism.Unity;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Xaml.XamlCompilation(Xamarin.Forms.Xaml.XamlCompilationOptions.Compile)]
namespace AiEffects.TestApp
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            if (Device.OS == TargetPlatform.iOS) {
                NavigationService.NavigateAsync("NaviA/MainPage");
            }
            else if (Device.OS == TargetPlatform.Android) {
                NavigationService.NavigateAsync("NaviA/MainPage");
            }
        }

        protected override void RegisterTypes()
        {
            this.GetType().GetTypeInfo().Assembly
            .DefinedTypes
            .Where(t => t.Namespace.EndsWith(".Views", System.StringComparison.Ordinal))
            .ForEach(t => {
                Container.RegisterTypeForNavigation(t.AsType(), t.Name);
            });

        }
    }
}

