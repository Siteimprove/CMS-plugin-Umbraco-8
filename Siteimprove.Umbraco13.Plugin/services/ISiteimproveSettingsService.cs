using Siteimprove.Umbraco13.Plugin.Models;

namespace Siteimprove.Umbraco13.Plugin.Services
{
    public interface ISiteimproveSettingsService
    {
        SiteimproveSettings GetSettings();

        void Insert(SiteimproveSettings row);

        void Update(SiteimproveSettings row);
    }
}
