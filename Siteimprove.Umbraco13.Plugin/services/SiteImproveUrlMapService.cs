using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteImprove.Umbraco13.Plugin.Models;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Extensions;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Infrastructure.Migrations.Expressions.Insert;
using NPoco;

namespace SiteImprove.Umbraco13.Plugin.Services
{
    public class SiteimproveUrlMapService : ISiteImproveUrlMapService
    {
        private readonly IScopeProvider _scopeProvider;
        private readonly IUmbracoContextFactory _ctxFactory;

        public SiteimproveUrlMapService(IScopeProvider scopeProvider,
            IUmbracoContextFactory ctxFactory)
        {
            this._scopeProvider = scopeProvider;
            this._ctxFactory = ctxFactory;
        }

        public async Task<bool> SaveUrlMap(SiteImproveUrlMap row)
        {
            int response = -1;

            if (row.Id == -1)
                response = await Insert(row) != null ? 1 : -1;
            else
                response = await Update(row);

            return response == 1;
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

        public SiteImproveUrlMap GetUrlMap()
        {
            try
            {
                using (var scope = _scopeProvider.CreateScope(autoComplete: true))
                {
                    var sql = scope.Database.SqlContext.Sql().Select<SiteImproveUrlMap>().From<SiteImproveUrlMap>().SelectTop(1);
                    var result = scope.Database.Fetch<SiteImproveUrlMap>(sql);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {                
                return null;
            }
        }

        public string GetPageUrlByPageId(int pageId)
        {
            using (UmbracoContextReference umbracoContextReference = _ctxFactory.EnsureUmbracoContext())
            {
                var node = umbracoContextReference.UmbracoContext.Content?.GetById(pageId);
                var absoluteUrl = node != null ? node.Url(mode: UrlMode.Absolute) : "";                
                if (string.IsNullOrEmpty(absoluteUrl))
                {
                    return "";
                }

                var urlMap = GetUrlMap();
                if (urlMap == null || string.IsNullOrEmpty(urlMap.NewDomain)) 
                {
                    return absoluteUrl;
                }

                Uri currentUri = new Uri(absoluteUrl);
                var currentDomain = currentUri.GetLeftPart(UriPartial.Authority);
                
                var newDomain = urlMap.NewDomain;
                newDomain = newDomain[newDomain.Length - 1] == '/' ? 
                    newDomain.Substring(0, newDomain.Length - 1) : newDomain;

                var url = absoluteUrl.Replace(currentDomain, newDomain);
                return url;
            }
        }
    }
}