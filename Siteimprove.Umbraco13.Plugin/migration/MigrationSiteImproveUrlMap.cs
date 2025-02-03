using SiteImprove.Umbraco8.Plugin.Models;
using SiteImprove.Umbraco8.Plugin.Services;
using Umbraco.Core.Migrations;

namespace SiteImprove.Umbraco8.Plugin.Migration
{
    public class MigrationSiteImproveUrlMap : MigrationBase
    {
        private readonly SiteImproveUrlMapService _siteImproveUrlMapService;

        public MigrationSiteImproveUrlMap(IMigrationContext context, SiteImproveUrlMapService service) : base(context)
        {
            _siteImproveUrlMapService = service;
        }

        public override void Migrate()
        {
            if (!TableExists(Constants.SiteImproveUrlMapDbTable))
            {
                Create.Table<SiteImproveUrlMap>(false).Do();
            }
        }
    }
}