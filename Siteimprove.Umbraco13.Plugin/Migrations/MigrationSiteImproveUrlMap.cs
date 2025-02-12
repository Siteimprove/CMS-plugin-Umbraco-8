using Siteimprove.Umbraco13.Plugin.Models;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Migrations.Upgrade;
using Umbraco.Cms.Infrastructure.Scoping;

namespace Siteimprove.Umbraco13.Plugin.Migration
{
    public class MigrationSiteimproveUrlMap : MigrationBase
    {
        public MigrationSiteimproveUrlMap(IMigrationContext context) : base(context) {}

        protected override void Migrate()
        {
            if (!TableExists(Constants.SiteimproveUrlMapDbTable))
            {
                Create.Table<SiteimproveUrlMap>(false).Do();
            }
        }

        public static void ExecuteMigrationPlan(IMigrationPlanExecutor migrationPlanExecutor, IScopeProvider scopeProvider, IKeyValueService keyValueService)
        {
            var plan = new MigrationPlan("MigrationSiteimproveUrlMap");
            plan.From(string.Empty)
                .To<MigrationSiteimproveUrlMap>("13.0.0");
            var upgrader = new Upgrader(plan);
            upgrader.Execute(migrationPlanExecutor, scopeProvider, keyValueService);
        }
    }
}