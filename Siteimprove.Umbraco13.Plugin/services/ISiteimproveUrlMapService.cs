using SiteImprove.Umbraco13.Plugin.Models;

namespace SiteImprove.Umbraco13.Plugin.Services
{
    public interface ISiteImproveUrlMapService
    {
        Task<bool> SaveUrlMap(SiteImproveUrlMap row);

        string GetPageUrlByPageId(int pageId);

        SiteImproveUrlMap GetUrlMap();
    }
}
