using SiteImprove.Umbraco8.Plugin.Models;
using SiteImprove.Umbraco8.Plugin.Services;
using Umbraco.Core.Migrations;

namespace SiteImprove.Umbraco8.Plugin.Migration
{
    public class MigrationSiteImproveSettings : MigrationBase
    {
        private readonly SiteImproveSettingsService _siteImproveSettingsService;
        //private readonly IMigrationContext _context;

        public MigrationSiteImproveSettings(IMigrationContext context, SiteImproveSettingsService service)
            : base(context)
        {
            //_context = context;
            _siteImproveSettingsService = service;
        }

        //public void InitializeModel()
        //{
        //    var row = _siteImproveSettingsService.SelectTopRow();
        //    if (row == null)
        //    {
        //        row = GenerateDefaultModel();
        //        _siteImproveSettingsService.Insert(row);
        //    }
        //}

        public override void Migrate()
        {
            if (!TableExists(Constants.SiteImproveDbTable))
            {
                Create.Table<SiteImproveSettingsModel>(false).Do();
                _siteImproveSettingsService.Insert(GenerateDefaultModel());
                return;
            }

            var row = _siteImproveSettingsService.SelectTopRow();
            if (row != null && !row.Installed)
            {
                Create.Table<SiteImproveSettingsModel>(true).Do();
            }

            if (row == null)
            {
                _siteImproveSettingsService.Insert(GenerateDefaultModel());
            }
        }

        //public void AddDbTable(ApplicationContext applicationContext)
        //{
        //    var db = applicationContext.DatabaseContext.Database;

        //    if (!db.TableExist(Constants.SiteImproveDbTable))
        //    {
        //        db.CreateTable<SiteImproveSettingsModel>(false);
        //        return;
        //    }

        //    // Handle legacy
        //    var row = db
        //        .Query<SiteImproveSettingsModel>(
        //            SiteImproveSettingsHelper.SelectTopQuery(
        //                applicationContext.DatabaseContext.DatabaseProvider, 1, Constants.SiteImproveDbTable))
        //        .FirstOrDefault();

        //    if (row != null && !row.Installed)
        //    {
        //        db.CreateTable<SiteImproveSettingsModel>(true);
        //    }
        //}

        //public string SelectTopQuery(global::Umbraco.Core.Constants.DatabaseProviders databaseProviders, int number, string table)
        //{
        //    switch (databaseProviders)
        //    {
        //        case global::Umbraco.Core.Constants.DatabaseProviders.SqlAzure:
        //        case global::Umbraco.Core.Constants.DatabaseProviders.SqlServerCE:
        //        case global::Umbraco.Core.Constants.DatabaseProviders.SqlServer:
        //            return string.Format("SELECT TOP {0} * FROM {1}", number, table);

        //        case global::Umbraco.Core.Constants.DatabaseProviders.PostgreSQL:
        //        case global::Umbraco.Core.Constants.DatabaseProviders.SQLite:
        //        case global::Umbraco.Core.Constants.DatabaseProviders.MySql:
        //            return string.Format("SELECT * FROM {1} LIMIT {0}", number, table);

        //        case global::Umbraco.Core.Constants.DatabaseProviders.Oracle:
        //            return string.Format("SELECT * FROM {1} WHERE ROWNUM<={0}", number, table);

        //        default:
        //            return string.Format("SELECT TOP {0} * FROM {1}", number, table);
        //    }
        //}

        /// <summary>
        /// Get node id's that will execute the Siteimprove recrawling mehtod
        /// </summary>
        /// <returns></returns>
        //public string GetCrawlIds()
        //{
        //    var row = GetFirstRow<SiteImproveSettingsModel>(Constants.SiteImproveDbTable);

        //    // Handle legacy
        //    if (row.Installed == false)
        //    {
        //        row.Installed = true;

        //        var publishedRootPages = this.Umbraco.ContentAtRoot();
        //        row.CrawlIds = publishedRootPages.Any() ? publishedRootPages.First().Id.ToString() : null;
        //        Ctx.Database.Update(row);
        //    }

        //    return row.CrawlIds;
        //}

        //public void SetCrawlIds(string ids)
        //{
        //    ids = ids ?? "";

        //    var row = GetFirstRow<SiteImproveSettingsModel>(Constants.SiteImproveDbTable);
        //    if (row == null)
        //    {
        //        row = GenerateDefaultModel(ids);
        //        Ctx.Database.Insert(row);
        //        return;
        //    }

        //    row.CrawlIds = ids;
        //    Ctx.Database.Update(row);
        //}


        private SiteImproveSettingsModel GenerateDefaultModel()
        {
            //var publishedRootPages = this.Umbraco.ContentAtRoot();

            return new SiteImproveSettingsModel
            {
                Installed = true,
                Token = _siteImproveSettingsService.RequestToken()
                //CrawlIds = publishedRootPages.Any() ? publishedRootPages.First().Id.ToString() : null
            };
        }

        //private SiteImproveSettingsModel GenerateDefaultModel(string crawlingIds)
        //{
        //    var model = GenerateDefaultModel();
        //    model.CrawlIds = crawlingIds;

        //    return model;
        //}

        //private T GetFirstRow<T>(string table) where T : class
        //{
        //    var query = Ctx.Database.Query<T>(SelectTopQuery(Ctx.DatabaseProvider, 1, table));
        //    return query.Any() ? query.First() : null;
        //}
    }
}