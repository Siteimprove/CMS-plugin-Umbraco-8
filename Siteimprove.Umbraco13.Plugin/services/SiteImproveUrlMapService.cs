using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteImprove.Umbraco8.Plugin.Models;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Extensions;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace SiteImprove.Umbraco8.Plugin.Services
{
    public class SiteImproveUrlMapService : ISiteImproveUrlMapService
    {
        private readonly IScopeProvider _scopeProvider;
        private readonly IUmbracoContextFactory _ctxFactory;

        public SiteImproveUrlMapService(IScopeProvider scopeProvider,
            IUmbracoContextFactory ctxFactory)
        {
            this._scopeProvider = scopeProvider;
            this._ctxFactory = ctxFactory;
        }

        public Task<object> Insert(SiteImproveUrlMap row)
        {
            using (var scope = _scopeProvider.CreateScope(autoComplete: true))
            {
                return scope.Database.InsertAsync(row);
            }
        }

        public Task<int> Update(SiteImproveUrlMap row)
        {
            using (var scope = _scopeProvider.CreateScope(autoComplete: true))
            {
                return scope.Database.UpdateAsync(row);
            }
        }

        public Task<List<SiteImproveUrlMap>> GetAll()
        {
            using (var scope = _scopeProvider.CreateScope(autoComplete: true))
            {
                var sql = scope.SqlContext.Sql().SelectAll().From<SiteImproveUrlMap>();
                var selectResult = scope.Database.FetchAsync<SiteImproveUrlMap>(sql);
                return selectResult;
            }
        }

        public Task<SiteImproveUrlMap> GetByPageId(int pageId)
        {
            using (var scope = _scopeProvider.CreateScope(autoComplete: true))
            {
                var sql = scope.SqlContext.Sql().SelectAll().From<SiteImproveUrlMap>().Where<SiteImproveUrlMap>(m => m.PageId == pageId);
                var selectResult = scope.Database.FirstOrDefaultAsync<SiteImproveUrlMap>(sql);
                return selectResult;
            }
        }

        public async Task<string> GetPageUrlByPageId(int pageId)
        {
            using (UmbracoContextReference umbracoContextReference = _ctxFactory.EnsureUmbracoContext())
            {
                var node = umbracoContextReference.UmbracoContext.Content?.GetById(pageId);
                var originalUrl = node != null ? node.Url(mode: UrlMode.Absolute) : "";
                var urlMaps = await GetAll();
                SiteImproveUrlMap currentMapping = GetMapping(pageId, urlMaps);
                var url = currentMapping == default
                    ? originalUrl
                    : originalUrl.Replace(currentMapping.CurrentUrlPart, currentMapping.NewUrlPart);
                return url;
            }
        }

        private SiteImproveUrlMap GetMapping(int pageId, List<SiteImproveUrlMap> maps)
        {
            if (maps.Any(m => m.PageId == pageId && !string.IsNullOrWhiteSpace(m.CurrentUrlPart)))
            {
                return maps.First(m => m.PageId == pageId);
            }

            using (UmbracoContextReference ctxReference = _ctxFactory.EnsureUmbracoContext())
            {
                var node = ctxReference.UmbracoContext.Content?.GetById(pageId);
                return node.Parent != null ? GetMapping(node.Parent.Id, maps) : default;
            }
        }
    }
}