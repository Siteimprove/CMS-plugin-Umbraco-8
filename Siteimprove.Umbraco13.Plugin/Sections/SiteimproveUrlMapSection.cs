using Umbraco.Cms.Core.Sections;

namespace Siteimprove.Umbraco13.Plugin.Sections
{
    public class SiteimproveUrlMapSection : ISection
    {
        public string Alias => "siteimproveUrlMapSection";
        public string Name => "URL Map";
        public string Icon => "icon-calculator";
    }
}
