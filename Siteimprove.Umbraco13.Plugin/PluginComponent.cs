using System.Net;
using SiteImprove.Umbraco8.Plugin.Migration;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Core.Migrations;
using Umbraco.Core.Migrations.Upgrade;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;

namespace SiteImprove.Umbraco8.Plugin
{
    public class PluginComponent : IComponent
    {
        private readonly IScopeProvider scopeProvider;
        private readonly IMigrationBuilder migrationBuilder;
        private readonly IKeyValueService keyValueService;
        private readonly ILogger logger;
        //private readonly IUmbracoContextFactory _context;
        public PluginComponent(
            IScopeProvider scopeProvider,
            IMigrationBuilder migrationBuilder,
            IKeyValueService keyValueService,
            ILogger logger)
        {
            this.scopeProvider = scopeProvider;
            this.migrationBuilder = migrationBuilder;
            this.keyValueService = keyValueService;
            this.logger = logger;
        }

        public void Initialize()
        {
            //force all outgoing connections to TLS 1.2 first
            //(it still falls back to 1.1 / 1.0 if the remote doesn't support 1.2).
            if (ServicePointManager.SecurityProtocol.HasFlag(SecurityProtocolType.Tls12) == false)
            {
                ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;
            }

            //TreeControllerBase.MenuRendering += TreeControllerBase_MenuRendering;

            var upgrader = new Upgrader(new MigrationPlanSiteImproveSettings());
            upgrader.Execute(scopeProvider, migrationBuilder, keyValueService, logger);
        }

        public void Terminate()
        {
        }

        //private void TreeControllerBase_MenuRendering(TreeControllerBase sender, MenuRenderingEventArgs e)
        //{
        //    var node = sender.Umbraco.Content(e.NodeId);

        //    if (sender.TreeAlias == "content" && node != null)
        //    {
        //        e.Menu.Items.Add(new SiteImproveRecheckMenuItem());
        //    }
        //}
    }
}