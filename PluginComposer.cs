using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Siteimprove.Umbraco13.Plugin.Dashboards;
using Siteimprove.Umbraco13.Plugin.Middlewares;
using Siteimprove.Umbraco13.Plugin.Sections;
using Siteimprove.Umbraco13.Plugin.Services;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Sections;
using Umbraco.Cms.Web.Common.ApplicationBuilder;

namespace Siteimprove.Umbraco13.Plugin
{
	public class PluginComposer : IComposer
	{
		public void Compose(IUmbracoBuilder builder)
		{
			// Adds siteimprove services
			builder.Services.AddTransient<ISiteimproveSettingsService, SiteimproveSettingsService>();
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
}
