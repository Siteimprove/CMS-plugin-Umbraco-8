using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Core;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Extensions;
using Newtonsoft.Json;
using Umbraco.Cms.Core.Configuration;
using SiteImprove.Umbraco8.Plugin.Models;
using SiteImprove.Umbraco8.Plugin.Services;

namespace Siteimprove.Umbraco13.Plugin.Controllers
{
    public class SiteImproveApiController : UmbracoAuthorizedApiController
    {
        private readonly ISiteImproveUrlMapService _siteImproveUrlMapService;
        private readonly IUmbracoVersion _umbracoVersion;

        public SiteImproveApiController(ISiteImproveUrlMapService siteImproveUrlMapService,
            IUmbracoVersion umbracoVersion)
        {
            _siteImproveUrlMapService = siteImproveUrlMapService;
            _umbracoVersion = umbracoVersion;
        }

        [HttpGet]
        public async Task<ActionResult> GetSettings(int pageId)
        {
            var urlMap = await _siteImproveUrlMapService.GetByPageId(pageId);
            var model = new
            {
                currentUrlPart = urlMap?.CurrentUrlPart ?? string.Empty,
                newUrlPart = urlMap?.NewUrlPart ?? string.Empty
            };
            return Content(JsonConvert.SerializeObject(model), "application/json");
        }

        [HttpGet]
        public ActionResult GetUmbracoVersion()
        {
            return Content(_umbracoVersion.Version.ToString());
        }

        [HttpPost]
        public async Task<ActionResult> SaveUrlMap([FromBody] SaveUrlMapParams saveUrlMapParams)
        {
            try
            {
                var current = await _siteImproveUrlMapService.GetByPageId(saveUrlMapParams.PageId);
                if (current == null)
                {
                    current = new SiteImproveUrlMap
                    {
                        PageId = saveUrlMapParams.PageId,
                        CurrentUrlPart = saveUrlMapParams.CurrentUrlPart,
                        NewUrlPart = saveUrlMapParams.NewUrlPart
                    };
                    await _siteImproveUrlMapService.Insert(current);
                }
                else
                {
                    current.CurrentUrlPart = saveUrlMapParams.CurrentUrlPart;
                    current.NewUrlPart = saveUrlMapParams.NewUrlPart;
                    await _siteImproveUrlMapService.Update(current);
                }

                var model = new
                {
                    success = true,
                    message = "Saved"
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

        public class SaveUrlMapParams
        {
            public int PageId { get; set; }
            public string CurrentUrlPart { get; set; }
            public string NewUrlPart { get; set; }
        }

        [HttpGet]
        public async Task<ActionResult> GetPageUrl(int pageId)
        {
            var url = await _siteImproveUrlMapService.GetPageUrlByPageId(pageId);
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
