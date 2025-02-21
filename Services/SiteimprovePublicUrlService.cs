using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Extensions;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Infrastructure.Migrations.Expressions.Insert;
using NPoco;
using Umbraco.Cms.Core.Services;

namespace Siteimprove.Umbraco13.Plugin.Services
{
	public class SiteimprovePublicUrlService : ISiteimprovePublicUrlService
	{
		private const string SiteimprovePublicUrlKey = "SiteimprovePublicUrl";
		private readonly IUmbracoContextFactory _ctxFactory;
		private readonly IKeyValueService _keyValueService;

		public SiteimprovePublicUrlService(IUmbracoContextFactory ctxFactory,
			IKeyValueService keyValueService)
		{
			this._ctxFactory = ctxFactory;
			this._keyValueService = keyValueService;
		}

		public void Set(string publicUrl)
		{
			_keyValueService.SetValue(SiteimprovePublicUrlKey, publicUrl);
		}

		public string? Get()
		{
			return _keyValueService.GetValue(SiteimprovePublicUrlKey);
		}

		public string GetPageUrlByPageId(int pageId)
		{
			using (UmbracoContextReference umbracoContextReference = _ctxFactory.EnsureUmbracoContext())
			{
				var node = umbracoContextReference.UmbracoContext.Content?.GetById(pageId);
				var absoluteUrl = node != null ? node.Url(mode: UrlMode.Absolute) : "";
				if (string.IsNullOrEmpty(absoluteUrl))
				{
					return "";
				}

				// Gets the public url (the new domain)
				var newDomain = Get();
				if (string.IsNullOrEmpty(newDomain))
				{
					return absoluteUrl;
				}

				Uri currentUri = new Uri(absoluteUrl);
				var currentDomain = currentUri.GetLeftPart(UriPartial.Authority);

				newDomain = newDomain[newDomain.Length - 1] == '/' ?
					newDomain.Substring(0, newDomain.Length - 1) : newDomain;

				var url = absoluteUrl.Replace(currentDomain, newDomain);
				return url;
			}
		}
	}
}
