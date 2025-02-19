using Siteimprove.Umbraco13.Plugin.Models;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Configuration;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Extensions;

namespace Siteimprove.Umbraco13.Plugin.Services
{
	public class SiteimproveSettingsService : ISiteimproveSettingsService
	{
		private readonly IScopeProvider _scopeProvider;

		public SiteimproveSettingsService(IScopeProvider scopeProvider)
		{
			_scopeProvider = scopeProvider;
		}

		public SiteimproveSettings GetSettings()
		{
			try
			{
				using (var scope = _scopeProvider.CreateScope(autoComplete: true))
				{
					var sql = scope.Database.SqlContext.Sql().Select<SiteimproveSettings>().From<SiteimproveSettings>().SelectTop(1);
					var resultList = scope.Database.Fetch<SiteimproveSettings>(sql);
					return resultList.FirstOrDefault();
				}
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public void Insert(SiteimproveSettings row)
		{
			using (var scope = _scopeProvider.CreateScope(autoComplete: true))
			{
				var sql = scope.Database.Insert(row);
			}
		}

		public void Update(SiteimproveSettings row)
		{
			using (var scope = _scopeProvider.CreateScope(autoComplete: true))
			{
				var sql = scope.Database.Update(row);
			}
		}
	}
}
