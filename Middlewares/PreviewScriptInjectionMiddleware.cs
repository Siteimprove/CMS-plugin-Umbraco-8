using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
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
		if (context.Request.GetDisplayUrl().Contains("preview") && context.Request.Query.ContainsKey("id"))
		{
			var originalBodyStream = context.Response.Body;
			using var newBodyStream = new MemoryStream();

			// Sets the new stream as the response body, so we can read and modify the response body after calling _next
			context.Response.Body = newBodyStream;
			await _next(context);

			// Reset the stream position to read the response body
			newBodyStream.Seek(0, SeekOrigin.Begin);
			var responseBody = new StreamReader(newBodyStream).ReadToEnd();

			if (responseBody.Contains("</body>"))
			{
				var pageUrl = "";
				if (int.TryParse(context.Request.Query["id"], out var pageId))
				{
					pageUrl = _siteImprovePublicUrlService.GetPageUrlByPageId(pageId);
				}
				// In the script below, we need to access the iframe that contains the preview frame
				var script = $@"
<script>
    window.onload = function () {{

		observeIframe();

		function observeIframe() {{
			const observer = new MutationObserver((mutations, obs) => {{
				const resultFrame = document.getElementById(""resultFrame"");				
				if (resultFrame) {{
					obs.disconnect();
					handleIframe(resultFrame);
				}}
			}});

			observer.observe(document.body, {{ childList: true, subtree: true }});
		}}

		function handleIframe(resultFrame) {{			
			if (resultFrame.contentDocument && resultFrame.contentDocument.readyState === ""complete"" && resultFrame.contentWindow.location.href !== ""about:blank"") {{				
				loadSmallbox(resultFrame);
			}} else {{				
				resultFrame.addEventListener(""load"", function () {{
					loadSmallbox(resultFrame);
				}});
			}}
		}}

		function loadSmallbox(resultFrame) {{
			console.log(""Loading Siteimprove Content Assistant..."");
			const script = document.createElement('script');
			script.src = '{OverlayUrl}';
			script.onload = function () {{
				const si = resultFrame.contentWindow._si;
				if (!si) {{
					console.log(""Content Assistant did not load correctly."");
					return;
				}}
				si.push(['setSession', null, null, 'Umbraco {_umbracoVersion.Version}']);
				si.push(['input', '{pageUrl}']);
				si.push(['registerPrepublishCallback', onPrepublish]);
				si.push(['onHighlight', onHighlight]);
			}};			
			resultFrame.contentDocument.body.appendChild(script);
		}}		

        function onPrepublish() {{
            return [
                resultFrame.contentDocument,
                () => console.log('FlatOM pull upload finished.'),
                'Full page'
            ];
        }}

        function onHighlight(highlightInfo) {{
            const si = resultFrame.contentWindow._si;
            if (!si) {{
                console.log(""Content Assistant did not load correctly."");
                return;
            }}
            si.push(['applyDefaultHighlighting', highlightInfo, resultFrame.contentDocument]);
        }}
    }};
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
