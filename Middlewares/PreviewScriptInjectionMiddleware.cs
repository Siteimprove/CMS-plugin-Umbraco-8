using System.Text;
using System.Web;
using Microsoft.AspNetCore.Http;
using Siteimprove.Umbraco13.Plugin.Services;
using Umbraco.Cms.Core.Configuration;

namespace Siteimprove.Umbraco13.Plugin.Middlewares;

public class PreviewScriptInjectionMiddleware
{
	private const string OverlayUrl = "https://cdn.siteimprove.net/cms/overlay-latest.js";
	private readonly RequestDelegate _next;
	private readonly ISiteimprovePublicUrlService _siteImprovePublicUrlService;
	private readonly IUmbracoVersion _umbracoVersion;

	public PreviewScriptInjectionMiddleware(RequestDelegate next,
		ISiteimprovePublicUrlService siteImprovePublicUrlService,
		IUmbracoVersion umbracoVersion)
	{
		_next = next;
		_siteImprovePublicUrlService = siteImprovePublicUrlService;
		_umbracoVersion = umbracoVersion;
	}

	public async Task Invoke(HttpContext context)
	{
		var acceptHtml = context.Request.Headers["Accept"]
			.Any(v => v.Contains("text/html", StringComparison.OrdinalIgnoreCase));

		// Check if the request is for a preview, if it accepts HTML (which indicates that it is the request for the
		// content of the preview page), and if the request is made from an iframe (where the content of the preview page is rendered)
		if (context.Request.Cookies["UMB_PREVIEW"] == "preview" && acceptHtml && context.Request.Headers["sec-fetch-dest"].Contains("iframe"))
		{
			var originalBodyStream = context.Response.Body;
			using var newBodyStream = new MemoryStream();

			// Sets the new stream as the response body, so we can read and modify the response body after calling _next
			context.Response.Body = newBodyStream;
			await _next(context);

			// Reset the stream position to read the response body
			newBodyStream.Seek(0, SeekOrigin.Begin);
			var responseBody = new StreamReader(newBodyStream).ReadToEnd();

			// If the response body contains the closing </body> tag, it indicates that it is the HTML of the page
			// and then we can inject our script
			if (responseBody.Contains("</body>"))
			{
				var pageUrl = "";

				var referer = context.Request.Headers["referer"].FirstOrDefault();
				var uri = new Uri(referer ?? string.Empty);
				var query = HttpUtility.ParseQueryString(uri.Query);
				string? id = query["id"];

				// When we first click the preview button, the request referer contains a query string with the id of the page.
				// In the other requests, when navigating inside the preview, the Path of the request contains the exact path that
				// we can directly use to retrieve the obj of the page. So depending on each case, we can either use the page id
				// or the path to get the obj of the page and then the URL of the page.
				if (int.TryParse(id, out var pageId))
				{
					pageUrl = _siteImprovePublicUrlService.GetPageUrlByPageId(pageId);
				}
				else
				{
					pageUrl = _siteImprovePublicUrlService.GetPageUrlByPath(context.Request.Path);
				}

				var script = $@"
<script>
    window.onload = function () {{
		loadSmallbox();
    }};

	function loadSmallbox() {{
		console.log('Loading Siteimprove Content Assistant...');
		const script = document.createElement('script');
		script.src = '{OverlayUrl}';
		script.onload = function () {{
			const si = window._si;
			if (!si) {{
				console.log('Content Assistant did not load correctly.');
				return;
			}}
			si.push(['setSession', null, null, 'Umbraco {_umbracoVersion.Version}']);
			si.push(['input', '{pageUrl}']);
			si.push(['registerPrepublishCallback', onPrepublish]);
			si.push(['onHighlight', onHighlight]);
		}};
		document.body.appendChild(script);
	}}

	function onPrepublish() {{
		return [
			document,
			() => console.log('FlatOM pull upload finished.'),
			'Full page'
		];
	}}

	function onHighlight(highlightInfo) {{
		const si = window._si;
		if (!si) {{
			console.log('Content Assistant did not load correctly.');
			return;
		}}
		si.push(['applyDefaultHighlighting', highlightInfo, document]);
	}}
</script>
";
				responseBody = responseBody.Replace("</body>", script + "</body>");
			}
			// Writes the new reponse body, with the injected script, to the original stream
			var modifiedBytes = Encoding.UTF8.GetBytes(responseBody);
			await originalBodyStream.WriteAsync(modifiedBytes, 0, modifiedBytes.Length);
		}
		else
		{
			await _next(context);
		}
	}
}
