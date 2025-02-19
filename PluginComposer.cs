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
			// Adds application starting notification
			builder.AddNotificationHandler<UmbracoApplicationStartingNotification, AppStartingHandler>();
			// Adds siteimprove services
			builder.Services.AddTransient<ISiteimproveSettingsService, SiteimproveSettingsService>();
			builder.Services.AddTransient<ISiteimproveUrlMapService, SiteimproveUrlMapService>();
			// Adds Siteimprove section on the top menu
			builder.AddSection<SiteimproveUrlMapSection>();
			builder.AddDashboard<SiteimproveUrlMapDashboard>();
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
