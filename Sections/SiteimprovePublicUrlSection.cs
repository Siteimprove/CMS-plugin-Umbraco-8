using Umbraco.Cms.Core.Sections;

namespace Siteimprove.Umbraco13.Plugin.Sections
{
	public class SiteimprovePublicUrlSection : ISection
	{
		public string Alias => "siteimprovePublicUrlSection";
		public string Name => "Public URL";
		public string Icon => "icon-calculator";
	}
}
