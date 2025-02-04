using SiteImprove.Umbraco8.Plugin.Models;
using SiteImprove.Umbraco8.Plugin.Services;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Migrations.Upgrade;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Core.Services;


namespace SiteImprove.Umbraco8.Plugin.Migration
{
    public class MigrationSiteimproveSettings : MigrationBase
    {
        private readonly ISiteimproveSettingsService _siteimproveSettingsService;

        public MigrationSiteimproveSettings(IMigrationContext context, ISiteimproveSettingsService service)
            : base(context)
        {
            _siteimproveSettingsService = service;
        }        

        protected override void Migrate()
        {
            if (!TableExists(Constants.SiteImproveDbTable))
            {
                Create.Table<SiteImproveSettingsModel>(false).Do();
                _siteimproveSettingsService.Insert(GenerateDefaultModel());
                return;
            }

            var row = _siteimproveSettingsService.SelectTopRow();
            if (row != null && !row.Installed)
            {
                _siteimproveSettingsService.Insert(GenerateDefaultModel());
            }

            if (row == null)
            {
                Create.Table<SiteImproveSettingsModel>(true).Do();                
            }
        }

        private SiteImproveSettingsModel GenerateDefaultModel()
        {
            return new SiteImproveSettingsModel
            {
                Installed = true,
            };
        }

        public static void ExecuteMigrationPlan(IMigrationPlanExecutor migrationPlanExecutor, IScopeProvider scopeProvider, IKeyValueService keyValueService)
        {
            var plan = new MigrationPlan("MigrationSiteimproveSettings");
            plan.From(string.Empty)
                .To<MigrationSiteimproveSettings>("MigrationSiteimproveSettings");
            var upgrader = new Upgrader(plan);
            upgrader.Execute(migrationPlanExecutor, scopeProvider, keyValueService);
        }
    }
}