using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Siteimprove.Umbraco13.Plugin.Middlewares;
using Siteimprove.Umbraco13.Plugin.Services;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Sections;
using Umbraco.Cms.Web.Common.ApplicationBuilder;
using Umbraco.Cms.Core.Dashboards;

namespace Siteimprove.Umbraco13.Plugin;

public class PluginComposer : IComposer
{
	public void Compose(IUmbracoBuilder builder)
	{
		// Adds siteimprove services
		builder.Services.AddTransient<ISiteimprovePublicUrlService, SiteimprovePublicUrlService>();
		// Adds Siteimprove section on the top menu
		builder.AddSection<SiteimprovePublicUrlSection>();
		// Adds siteimprove middleware
		builder.Services.Configure<UmbracoPipelineOptions>(
			options => options.AddFilter(
				new UmbracoPipelineFilter(
					"ScriptInjectionFilter",
					postPipeline: app => app.UseMiddleware<PreviewScriptInjectionMiddleware>()
		)));
	}
}

public class SiteimprovePublicUrlSection : ISection
{
	public string Alias => "siteimprovePublicUrlSection";
	public string Name => "Public URL";
	public string Icon => "icon-calculator";
}
