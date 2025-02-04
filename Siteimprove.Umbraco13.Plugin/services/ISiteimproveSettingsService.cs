using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteImprove.Umbraco8.Plugin.Models;

namespace SiteImprove.Umbraco8.Plugin.Services
{
    public interface ISiteimproveSettingsService
    {
        SiteImproveSettingsModel SelectTopRow();

        void Insert(SiteImproveSettingsModel row);

        void Update(SiteImproveSettingsModel row);
    }
}
