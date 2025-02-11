using SiteImprove.Umbraco13.Plugin.Models;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Configuration;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Extensions;

namespace SiteImprove.Umbraco13.Plugin.Services
{
    public class SiteimproveSettingsService : ISiteimproveSettingsService
    {
        private readonly IScopeProvider _scopeProvider;

        public SiteimproveSettingsService(IScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
        }

        public SiteImproveSettingsModel GetSettings()
        {
            try
            {
                using (var scope = _scopeProvider.CreateScope(autoComplete: true))
                {
                    var sql = scope.Database.SqlContext.Sql().Select<SiteImproveSettingsModel>().From<SiteImproveSettingsModel>().SelectTop(1);
                    var resultList = scope.Database.Fetch<SiteImproveSettingsModel>(sql);
                    return resultList.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {                
                return null;
            }
        }

        public void Insert(SiteImproveSettingsModel row)
        {
            using (var scope = _scopeProvider.CreateScope(autoComplete: true))
            {
                var sql = scope.Database.Insert(row);
            }
        }

        public void Update(SiteImproveSettingsModel row)
        {
            using (var scope = _scopeProvider.CreateScope(autoComplete: true))
            {
                var sql = scope.Database.Update(row);
            }
        }
    }
}
