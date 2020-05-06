using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using SiteImprove.Umbraco8.Plugin.Services;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace SiteImprove.Umbraco8.Plugin.Controllers
{
    public class SiteImproveApiController : UmbracoAuthorizedApiController
    {
        //private SiteImproveSettingsHelper SettingsHelper { get; set; }
        private readonly IUmbracoContextFactory _context;
        private readonly SiteImproveSettingsService _siteImproveSettingsService;

        public SiteImproveApiController(IUmbracoContextFactory context, SiteImproveSettingsService service)
        {
            _context = context;
            _siteImproveSettingsService = service;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetSettings()
        {
            var model = new
            {
                token = await _siteImproveSettingsService.GetToken(),
                crawlingIds = GetCrawlIds()
            };

            return Request.CreateResponse(HttpStatusCode.OK, model);
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
        public HttpResponseMessage GetPageUrl(int pageId)
        {
            var node = Umbraco.Content(pageId);

            var model = new
            {
                success = node != null,
                status = node != null ? "OK" : "No published page with that id",
                url = node != null ? node.Url(mode: UrlMode.Absolute) : ""
            };

            return Request.CreateResponse(HttpStatusCode.OK, model);
        }
    }
}
