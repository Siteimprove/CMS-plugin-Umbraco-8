using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Extensions;
using Newtonsoft.Json;
using Umbraco.Cms.Core.Configuration;
using Siteimprove.Umbraco13.Plugin.Services;
using Siteimprove.Umbraco13.Plugin.Models;

namespace Siteimprove.Umbraco13.Plugin.Controllers
{
    public class SiteimproveApiController : UmbracoAuthorizedApiController
    {
        private readonly ISiteimproveUrlMapService _siteImproveUrlMapService;
        private readonly IUmbracoVersion _umbracoVersion;

        public SiteimproveApiController(ISiteimproveUrlMapService siteImproveUrlMapService,
            IUmbracoVersion umbracoVersion)
        {
            _siteImproveUrlMapService = siteImproveUrlMapService;
            _umbracoVersion = umbracoVersion;
        }

        [HttpGet]
        public async Task<ActionResult> GetUrlMap(int pageId)
        {
           var urlMap = _siteImproveUrlMapService.GetUrlMap();
           var model = new
           {
               id = urlMap?.Id ?? -1,
               newDomain = urlMap?.NewDomain ?? string.Empty
           };
           return Content(JsonConvert.SerializeObject(model), "application/json");
        }

        [HttpGet]
        public ActionResult GetUmbracoVersion()
        {
            return Content(_umbracoVersion.Version.ToString());
        }

        [HttpPost]
        public async Task<ActionResult> SaveUrlMap([FromBody] SiteimproveUrlMap saveUrlMapParams)
        {
            try
            {
                var success = await _siteImproveUrlMapService.SaveUrlMap(saveUrlMapParams);
                var model = new
                {
                    success,
                    message = success ? "Saved" : "Error"
                };
                return Content(JsonConvert.SerializeObject(model), "application/json");
            }
            catch (Exception ex)
            {
                var model = new
                {
                    success = false,
                    message = ex.Message
                };
                return Content(JsonConvert.SerializeObject(model), "application/json");
            }
        }

        [HttpGet]
        public ActionResult GetPageUrl(int pageId)
        {
            var url = _siteImproveUrlMapService.GetPageUrlByPageId(pageId);
            var urlWasFound = !string.IsNullOrEmpty(url);
            var model = new
            {
                success = urlWasFound,
                status = urlWasFound ? "OK" : "No published page with that id",
                url
            };
            return Content(JsonConvert.SerializeObject(model), "application/json");
        }        
    }
}
