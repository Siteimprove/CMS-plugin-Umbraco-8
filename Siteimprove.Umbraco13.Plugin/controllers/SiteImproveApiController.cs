using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using SiteImprove.Umbraco8.Plugin.Models;
using SiteImprove.Umbraco8.Plugin.Services;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace Siteimprove.Umbraco13.Plugin.controllers
{
    public class SiteImproveApiController : UmbracoAuthorizedApiController
    {
        //private SiteImproveSettingsHelper SettingsHelper { get; set; }
        private readonly IUmbracoContextFactory _context;
        private readonly SiteImproveSettingsService _siteImproveSettingsService;
        private readonly SiteImproveUrlMapService _siteImproveUrlMapService;

        public SiteImproveApiController(IUmbracoContextFactory context,
            SiteImproveSettingsService service,
            SiteImproveUrlMapService siteImproveUrlMapService)
        {
            _context = context;
            _siteImproveSettingsService = service;
            _siteImproveUrlMapService = siteImproveUrlMapService;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetSettings(int pageId)
        {
            var urlMap = await _siteImproveUrlMapService.GetByPageId(pageId);
            var model = new
            {
                token = await _siteImproveSettingsService.GetToken(),
                crawlingIds = GetCrawlIds(),
                currentUrlPart = urlMap?.CurrentUrlPart ?? string.Empty,
                newUrlPart = urlMap?.NewUrlPart ?? string.Empty
            };

            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SaveUrlMap([FromBody] SaveUrlMapParams saveUrlMapParams)
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
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                var model = new
                {
                    success = false,
                    message = ex.Message
                };
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
        }

        public class SaveUrlMapParams
        {
            public int PageId { get; set; }
            public string CurrentUrlPart { get; set; }
            public string NewUrlPart { get; set; }
        }


        /// <summary>
        /// Get node id's that will execute the Siteimprove recrawling method
        /// </summary>
        /// <returns></returns>
        public string GetCrawlIds()
        {
            var publishedRootPages = Umbraco.ContentAtRoot().ToList();
            return publishedRootPages.Any() ? publishedRootPages.First().Id.ToString() : null;
        }


        [HttpGet]
        public async Task<HttpResponseMessage> GetToken()
        {
            return Request.CreateResponse(
                HttpStatusCode.OK,
                await _siteImproveSettingsService.GetToken());
        }

        [HttpGet]
        public async Task<HttpResponseMessage> RequestNewToken()
        {
            return Request.CreateResponse(
                HttpStatusCode.OK,
                await _siteImproveSettingsService.GetNewToken());
        }

        [HttpGet]
        public HttpResponseMessage GetCrawlingIds()
        {
            return Request.CreateResponse(
                HttpStatusCode.OK,
                GetCrawlIds());
        }

        //[HttpPost]
        //public HttpResponseMessage SetCrawlingIds([FromUri] string ids)
        //{
        //    SetCrawlIds(ids);
        //    return Request.CreateResponse(HttpStatusCode.OK);
        //}

        [HttpGet]
        public async Task<HttpResponseMessage> GetPageUrl(int pageId)
        {
            var node = Umbraco.Content(pageId);
            var originalUrl = node != null ? node.Url(mode: UrlMode.Absolute) : "";
            var urlMaps = await _siteImproveUrlMapService.GetAll();
            var currentMapping = GetMapping(pageId, urlMaps);
            var url = currentMapping == default
                ? originalUrl
                : originalUrl.Replace(currentMapping.CurrentUrlPart, currentMapping.NewUrlPart);

            var model = new
            {
                success = node != null,
                status = node != null ? "OK" : "No published page with that id",
                url
            };

            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

        private SiteImproveUrlMap GetMapping(int pageId, List<SiteImproveUrlMap> maps)
        {
            if (maps.Any(m => m.PageId == pageId && !string.IsNullOrWhiteSpace(m.CurrentUrlPart)))
            {
                return maps.First(m => m.PageId == pageId);
            }
            var node = Umbraco.Content(pageId);
            return node.Parent != null ? GetMapping(node.Parent.Id, maps) : default;
        }
    }
}
