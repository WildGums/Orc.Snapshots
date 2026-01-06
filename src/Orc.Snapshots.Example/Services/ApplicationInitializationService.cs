namespace Orc.Snapshots.Example.Services;

using System;
using System.Threading.Tasks;
using System.Windows.Media;
using Theming;
using Orchestra;

public class ApplicationInitializationService : ApplicationInitializationServiceBase
{
    public ApplicationInitializationService(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {

    }

    public override Task InitializeBeforeCreatingShellAsync()
    {
        InitializeFonts();

        return Task.CompletedTask;
    }

    private void InitializeFonts()
    {
        FontImage.RegisterFont("FontAwesome", new FontFamily(new Uri("pack://application:,,,/Orc.Snapshots.Example;component/Resources/Fonts/", UriKind.RelativeOrAbsolute), "./#FontAwesome"));

        FontImage.DefaultBrush = new SolidColorBrush(Color.FromArgb(255, 87, 87, 87));
        FontImage.DefaultFontFamily = "FontAwesome";
    }
}
