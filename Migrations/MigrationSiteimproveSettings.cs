using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Migrations.Upgrade;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Core.Services;
using Siteimprove.Umbraco13.Plugin.Services;
using Siteimprove.Umbraco13.Plugin.Models;

namespace Siteimprove.Umbraco13.Plugin.Migration
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
			if (!TableExists(Constants.SiteimproveDbTable))
			{
				Create.Table<SiteimproveSettings>(false).Do();
				_siteimproveSettingsService.Insert(GenerateDefaultModel());
				return;
			}

			var row = _siteimproveSettingsService.GetSettings();
			if (row != null && !row.Installed)
			{
				_siteimproveSettingsService.Insert(GenerateDefaultModel());
			}

			if (row == null)
			{
				Create.Table<SiteimproveSettings>(true).Do();
			}
		}

		private SiteimproveSettings GenerateDefaultModel()
		{
			return new SiteimproveSettings
			{
				Installed = true,
			};
		}

		public static void ExecuteMigrationPlan(IMigrationPlanExecutor migrationPlanExecutor, IScopeProvider scopeProvider, IKeyValueService keyValueService)
		{
			var plan = new MigrationPlan("MigrationSiteimproveSettings");
			plan.From(string.Empty)
				.To<MigrationSiteimproveSettings>("13.0.0");
			var upgrader = new Upgrader(plan);
			upgrader.Execute(migrationPlanExecutor, scopeProvider, keyValueService);
		}
	}
}
