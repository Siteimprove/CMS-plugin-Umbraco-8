using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Siteimprove.Umbraco13.Plugin.Middlewares;
using Siteimprove.Umbraco13.Plugin.Services;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Sections;
using Umbraco.Cms.Web.Common.ApplicationBuilder;
using Umbraco.Cms.Core.Dashboards;

namespace Siteimprove.Umbraco13.Plugin
{
	public class PluginComposer : IComposer
	{
		public void Compose(IUmbracoBuilder builder)
		{
			// Adds siteimprove services
			builder.Services.AddTransient<ISiteimprovePublicUrlService, SiteimprovePublicUrlService>();
			// Adds Siteimprove section on the top menu
			builder.AddSection<SiteimprovePublicUrlSection>();
			builder.AddDashboard<SiteimprovePublicUrlDashboard>();
			// Adds siteimprove middleware
			builder.Services.Configure<UmbracoPipelineOptions>(
				options => options.AddFilter(
					new UmbracoPipelineFilter(
						"ScriptInjectionFilter",
						postPipeline: app => app.UseMiddleware<PreviewScriptInjectionMiddleware>()
			)));
		}
	}

	public class SiteimprovePublicUrlDashboard : IDashboard
	{
		public string[] Sections => new[] { "siteimprovePublicUrlSection" };

		public IAccessRule[] AccessRules => new[] { new AccessRule { Type = AccessRuleType.Grant, Value = "10" } };

		public string? Alias => "siteimprovePublicUrlDashboard";

		public string? View => "/App_Plugins/Siteimprove/views/publicUrlSection.html";
	}

	public class SiteimprovePublicUrlSection : ISection
	{
		public string Alias => "siteimprovePublicUrlSection";
		public string Name => "Public URL";
		public string Icon => "icon-calculator";
	}
}
