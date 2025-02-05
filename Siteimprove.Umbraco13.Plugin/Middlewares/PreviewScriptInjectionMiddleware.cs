using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.Text;
using Umbraco.Cms.Core.Configuration;
using SiteImprove.Umbraco13.Plugin;
using SiteImprove.Umbraco13.Plugin.Services;

namespace Siteimprove.Umbraco13.Plugin.Middlewares
{
    public class PreviewScriptInjectionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ISiteImproveUrlMapService _siteImproveUrlMapService;
        private readonly IUmbracoVersion _umbracoVersion;

        public PreviewScriptInjectionMiddleware(RequestDelegate next, 
            ISiteImproveUrlMapService siteImproveUrlMapService,
            IUmbracoVersion umbracoVersion)
        {
            _next = next;
            _siteImproveUrlMapService = siteImproveUrlMapService;
            _umbracoVersion = umbracoVersion;
        }

        public async Task Invoke(HttpContext context)
        {                       
            if (context.Request.GetDisplayUrl().Contains("preview") && context.Request.Query.ContainsKey("id"))
            {
                var originalBodyStream = context.Response.Body;

                using (var newBodyStream = new MemoryStream())
                {
                    // Sets the new stream as the response body, so we can read and modify the response body after calling _next
                    context.Response.Body = newBodyStream;
                    await _next(context);
                    // Reset the stream position to read the response body
                    newBodyStream.Seek(0, SeekOrigin.Begin);
                    var responseBody = new StreamReader(newBodyStream).ReadToEnd();
                    
                    if (responseBody.Contains("</body>"))
                    {                       
                        var pageUrl = "";
                        if (Int32.TryParse(context.Request.Query["id"], out var pageId)) 
                        {
                            pageUrl = await _siteImproveUrlMapService.GetPageUrlByPageId(pageId);
                        }
                        // In the script below, we need to access the iframe that contains the preview frame
                        var script = $@"
<script>
    window.onload = function () {{
        const resultFrame = document.getElementById('resultFrame');
        if (!resultFrame || !resultFrame.contentDocument) {{
            console.error(""No frame found. Content Assistant will not be loaded."");            
        }}

        const script = document.createElement('script');
        script.src = '{Constants.OverlayUrl}';
        script.onload = function () {{
            const si = resultFrame.contentWindow._si;
            if (!si) {{
                console.log(""Content Assistant did not load correctly."");
                return;
            }}
            si.push(['setSession', null, null, 'umbraco-{_umbracoVersion.Version}']);
            si.push(['input', '{pageUrl}']);
            si.push(['registerPrepublishCallback', onPrepublish]);
            si.push(['onHighlight', onHighlight]);
        }};       
        resultFrame.contentDocument.body.appendChild(script);

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
            }
            else
            {
                await _next(context);
            }
        }
    }
}

