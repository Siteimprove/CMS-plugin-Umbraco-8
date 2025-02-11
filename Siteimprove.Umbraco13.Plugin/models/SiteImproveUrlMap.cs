using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace SiteImprove.Umbraco13.Plugin.Models
{
    [TableName(Constants.SiteImproveUrlMapDbTable)]
    [PrimaryKey("id", AutoIncrement = true)]
    public class SiteImproveUrlMap
    {
        public SiteImproveUrlMap()
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