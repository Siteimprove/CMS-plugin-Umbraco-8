using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace Siteimprove.Umbraco13.Plugin.Models
{
    [TableName(Constants.SiteimproveDbTable)]
    [PrimaryKey("id", AutoIncrement = true)]
    public class SiteimproveSettings //: ColumnDefinition
    {
        [PrimaryKeyColumn(AutoIncrement = true)]
        [Column("id")]
        public int Id { get; set; }

        [Column("Installed")]
        public bool Installed { get; set; }
    }
}
