﻿
namespace SiteImprove.Umbraco8.Plugin
{
    public class Constants
    {
        public const string SiteImproveDbTable = "SiteImprove_settings";
        public const string SiteImproveUrlMapDbTable = "SiteImproveUrlMap";
        public const string SiteImproveMenuActionFactory = "SiteImproveMenuActions";
        public static readonly string SiteImproveTokenUrl = "https://my2.siteimprove.com/auth/token" + "?cms=" + Umbraco.Core.Configuration.UmbracoVersion.Current.ToString();
    }
}
