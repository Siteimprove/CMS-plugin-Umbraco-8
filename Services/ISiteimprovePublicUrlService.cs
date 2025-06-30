namespace Siteimprove.Umbraco13.Plugin.Services;

public interface ISiteimprovePublicUrlService
{
	void Set(string row);
	string? Get();
	string GetPageUrlByPageId(int pageId);
	string GetPageUrlByPath(string path);
}
