using Umbraco.Cms.Core.Dashboards;

namespace Siteimprove.Umbraco13.Plugin.Dashboards
{
	public class SiteimprovePublicUrlDashboard : IDashboard
	{
		public string[] Sections => new[] { "siteimprovePublicUrlSection" };

		public IAccessRule[] AccessRules => new[] { new AccessRule { Type = AccessRuleType.Grant, Value = "10" } };

		public string? Alias => "siteimprovePublicUrlDashboard";

		public string? View => "/App_Plugins/Siteimprove/views/publicUrlSection.html";
	}
}
