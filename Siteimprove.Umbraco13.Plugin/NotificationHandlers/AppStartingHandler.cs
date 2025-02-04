using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core;
using SiteImprove.Umbraco8.Plugin.Migration;

namespace SiteImprove.Umbraco8.Plugin
{
    public class AppStartingHandler : INotificationHandler<UmbracoApplicationStartingNotification>
    {
        private readonly IMigrationPlanExecutor _migrationPlanExecuter;
        private readonly IScopeProvider _scopeProvider;
        private readonly IKeyValueService _keyValueService;

        public AppStartingHandler(IMigrationPlanExecutor migrationPlanExecuter, IScopeProvider scopeProvider, IKeyValueService keyValueService)
        {
            _migrationPlanExecuter = migrationPlanExecuter;
            _scopeProvider = scopeProvider;
            _keyValueService = keyValueService;
        }

        public void Handle(UmbracoApplicationStartingNotification notification)
        {
            if (notification.RuntimeLevel >= RuntimeLevel.Run)
            {
                MigrationSiteimproveSettings.ExecuteMigrationPlan(_migrationPlanExecuter, _scopeProvider, _keyValueService);
                MigrationSiteimproveUrlMap.ExecuteMigrationPlan(_migrationPlanExecuter, _scopeProvider, _keyValueService);
            }
        }
    }
}