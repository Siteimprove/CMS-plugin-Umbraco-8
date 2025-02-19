using Umbraco.Cms.Core.Dashboards;

namespace Siteimprove.Umbraco13.Plugin.Dashboards
{
	public class SiteimproveUrlMapDashboard : IDashboard
	{
		public string[] Sections => new[] { "siteimproveUrlMapSection" };

		public IAccessRule[] AccessRules => new[] { new AccessRule { Type = AccessRuleType.Grant, Value = "10" } };

		public string? Alias => "siteimproveUrlMapDashboard";

		public string? View => "/App_Plugins/Siteimprove/views/urlMapSection.html";
	}
}
