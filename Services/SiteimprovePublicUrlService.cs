using Umbraco.Extensions;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;

namespace Siteimprove.Umbraco13.Plugin.Services;

public class SiteimprovePublicUrlService : ISiteimprovePublicUrlService
{
	private const string SiteimprovePublicUrlKey = "SiteimprovePublicUrl";
	private readonly IUmbracoContextFactory _ctxFactory;
	private readonly IKeyValueService _keyValueService;

	public SiteimprovePublicUrlService(IUmbracoContextFactory ctxFactory,
		IKeyValueService keyValueService)
	{
		_ctxFactory = ctxFactory;
		_keyValueService = keyValueService;
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
		using var umbracoContextReference = _ctxFactory.EnsureUmbracoContext();
		var node = umbracoContextReference.UmbracoContext.Content?.GetById(pageId);
		var absoluteUrl = node != null ? node.Url(mode: UrlMode.Absolute) : string.Empty;
		if (string.IsNullOrEmpty(absoluteUrl))
		{
			return string.Empty;
		}

		// Gets the public url (the new domain)
		var newDomain = Get();
		if (string.IsNullOrEmpty(newDomain))
		{
			return absoluteUrl;
		}

		var currentUri = new Uri(absoluteUrl);
		var newDomainUri = new Uri(newDomain.TrimEnd('/'));

		var uriBuilder = new UriBuilder(currentUri)
		{
			Scheme = newDomainUri.Scheme,
			Host = newDomainUri.Host,
			Port = newDomainUri.IsDefaultPort ? -1 : newDomainUri.Port
		};

		return uriBuilder.ToString();
	}
}
