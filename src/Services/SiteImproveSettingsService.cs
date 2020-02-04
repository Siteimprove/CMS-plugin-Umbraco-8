using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SiteImprove.Umbraco8.Plugin.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Scoping;

namespace SiteImprove.Umbraco8.Plugin.Services
{
    public class SiteImproveSettingsService
    {
        private readonly IScopeProvider _scopeProvider;

        public SiteImproveSettingsService(IScopeProvider scopeProvider)
        {
            this._scopeProvider = scopeProvider;
        }

        public SiteImproveSettingsModel SelectTopRow()
        {
            try
            {
                using (var scope = _scopeProvider.CreateScope(autoComplete: true))
                {
                    var sql = scope.SqlContext.Sql().SelectTop(1).From<SiteImproveSettingsModel>();
                    var selectResult = scope.Database.ExecuteScalar<SiteImproveSettingsModel>(sql);
                    return selectResult;
                }
            }
            catch (Exception e)
            {

                return null;
            }
 
        }

        public void Insert(SiteImproveSettingsModel row)
        {
            using (var scope = _scopeProvider.CreateScope(autoComplete: true))
            {
                var sql = scope.Database.Insert(row);
            }
        }

        public void Update(SiteImproveSettingsModel row)
        {
            using (var scope = _scopeProvider.CreateScope(autoComplete: true))
            {
                var sql = scope.Database.Update(row);
            }
        }

        /// <summary>
        /// Returns the token that exist in the first row
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public async Task<string> GetToken()
        {
            var result = SelectTopRow();
            if (result == null)
            {
                // Token did not exist in database, fetch from SiteImprove
                string token = await RequestTokenAsync();

                var row = new SiteImproveSettingsModel { Token = token };
                Insert(row);

                return token;
            }

            return result.Token;
        }

        /// <summary>
        /// Updates the token in the first row, if row not created => create it
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public async Task<string> GetNewToken()
        {
            var row = SelectTopRow();
            if (row == null)
            {
                return await GetToken();
            }

            row.Token = await RequestTokenAsync();
            Update(row);

            return row.Token;
        }

        private async Task<string> RequestTokenAsync()
        {
            using (var client = new HttpClient())
            {
                string response = await client.GetStringAsync(Constants.SiteImproveTokenUrl);
                return JObject.Parse(response).GetValue("token").ToString();
            }
        }

        public string RequestToken()
        {
            using (var client = new HttpClient())
            {
                string response = client.GetStringAsync(Constants.SiteImproveTokenUrl).Result;
                return JObject.Parse(response).GetValue("token").ToString();
            }
        }


    }
}
