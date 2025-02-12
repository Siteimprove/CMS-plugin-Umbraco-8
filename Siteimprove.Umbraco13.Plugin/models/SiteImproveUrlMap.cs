using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace Siteimprove.Umbraco13.Plugin.Models
{
    [TableName(Constants.SiteimproveUrlMapDbTable)]
    [PrimaryKey("id", AutoIncrement = true)]
    public class SiteimproveUrlMap
    {
        public SiteimproveUrlMap()
        {
            NewDomain = string.Empty;
        }

        [PrimaryKeyColumn(AutoIncrement = true)]
        [Column("id")]
        public int Id { get; set; }    

        [Column("NewDomain")]
        [Length(1024)]
        public string NewDomain { get; set; }
    }
}