using SiteImprove.Umbraco13.Plugin.Models;

namespace SiteImprove.Umbraco13.Plugin.Services
{
    public interface ISiteimproveSettingsService
    {
        SiteImproveSettingsModel GetSettings();

        void Insert(SiteImproveSettingsModel row);

        void Update(SiteImproveSettingsModel row);
    }
}
