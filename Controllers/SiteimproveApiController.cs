using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Extensions;
using Newtonsoft.Json;
using Umbraco.Cms.Core.Configuration;
using Siteimprove.Umbraco13.Plugin.Services;

namespace Siteimprove.Umbraco13.Plugin.Controllers
{
	public class SiteimproveApiController : UmbracoAuthorizedApiController
	{
		private readonly ISiteimprovePublicUrlService _siteImprovePublicUrlService;
		private readonly IUmbracoVersion _umbracoVersion;

		public SiteimproveApiController(ISiteimprovePublicUrlService siteImprovePublicUrlService,
			IUmbracoVersion umbracoVersion)
		{
			_siteImprovePublicUrlService = siteImprovePublicUrlService;
			_umbracoVersion = umbracoVersion;
		}

		[HttpGet]
		public ActionResult GetPublicUrl(int pageId)
		{
			try
			{
				var model = new
				{
					success = true,
					publicUrl = _siteImprovePublicUrlService.Get() ?? ""
				};
				return Content(JsonConvert.SerializeObject(model), "application/json");
			}
			catch
			{
				var model = new
				{
					success = false,
					message = $"Error trying to retrieve public url"
				};
				return Content(JsonConvert.SerializeObject(model), "application/json");
			}
		}

		[HttpGet]
		public ActionResult GetUmbracoVersion()
		{
			return Content(_umbracoVersion.Version.ToString());
		}

		[HttpPost]
		public async Task<ActionResult> SavePublicUrl([FromBody] string publicUrl)
		{
			try
			{
				_siteImprovePublicUrlService.Set(publicUrl);
				var model = new
				{
					success = true,
					message = "Saved!"
				};
				return Content(JsonConvert.SerializeObject(model), "application/json");
			}
			catch (Exception ex)
			{
				var model = new
				{
					success = false,
					message = $"Error: {ex.Message}"
				};
				return Content(JsonConvert.SerializeObject(model), "application/json");
			}
		}

		[HttpGet]
		public ActionResult GetPageUrl(int pageId)
		{
			var url = _siteImprovePublicUrlService.GetPageUrlByPageId(pageId);
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
