using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ProductManagement.Entities.Models;

[Table("City")]
public partial class City
{
    [Key]
    public int CityId { get; set; }

    [Column(TypeName = "character varying")]
    public string CityName { get; set; } = null!;
}
