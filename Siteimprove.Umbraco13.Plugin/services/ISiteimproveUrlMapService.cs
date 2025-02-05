using SiteImprove.Umbraco13.Plugin.Models;

namespace SiteImprove.Umbraco13.Plugin.Services
{
    public interface ISiteImproveUrlMapService
    {
        Task<object> Insert(SiteImproveUrlMap row);

        Task<int> Update(SiteImproveUrlMap row);

        Task<List<SiteImproveUrlMap>> GetAll();

        Task<SiteImproveUrlMap> GetByPageId(int pageId);
        Task<string> GetPageUrlByPageId(int pageId);
    }
}
