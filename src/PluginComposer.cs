using SiteImprove.Umbraco8.Plugin.Services;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace SiteImprove.Umbraco8.Plugin
{
    // Register your own services by implementing IUserComposer
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class PluginComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.RegisterUnique<SiteImproveSettingsService>();
            composition.Components().Append<PluginComponent>();
        }
    }
}