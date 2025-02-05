using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace SiteImprove.Umbraco13.Plugin.Models
{
    [TableName(Constants.SiteImproveDbTable)]
    [PrimaryKey("id", AutoIncrement = true)]
    public class SiteImproveSettingsModel //: ColumnDefinition
    {
        [PrimaryKeyColumn(AutoIncrement = true)]
        [Column("id")]
        public int Id { get; set; }

        [Column("Installed")]
        public bool Installed { get; set; }
    }
}
