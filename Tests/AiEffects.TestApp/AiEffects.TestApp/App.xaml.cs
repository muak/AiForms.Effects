using System.Linq;
using System.Reflection;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

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

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            this.GetType().GetTypeInfo().Assembly
            .DefinedTypes
            .Where(t => t.Namespace.EndsWith(".Views", System.StringComparison.Ordinal))
            .ForEach(t => {
                containerRegistry.RegisterForNavigation(t.AsType(), t.Name);
            });

        }
    }
}

