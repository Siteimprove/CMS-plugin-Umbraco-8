using Siteimprove.Umbraco13.Plugin.Models;

namespace Siteimprove.Umbraco13.Plugin.Services
{
    public interface ISiteimproveUrlMapService
    {
        Task<bool> SaveUrlMap(SiteimproveUrlMap row);

        string GetPageUrlByPageId(int pageId);

        SiteimproveUrlMap GetUrlMap();
    }
}
