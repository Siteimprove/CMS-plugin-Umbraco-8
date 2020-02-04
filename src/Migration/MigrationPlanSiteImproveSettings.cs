using Umbraco.Core.Migrations;

namespace SiteImprove.Umbraco8.Plugin.Migration
{
    public class MigrationPlanSiteImproveSettings : MigrationPlan
    {
        public MigrationPlanSiteImproveSettings() : base("SiteImprovePlugin")
        {
            From(string.Empty).To<MigrationSiteImproveSettings>("MigrationSiteImproveSettings");
        }
    }
}
