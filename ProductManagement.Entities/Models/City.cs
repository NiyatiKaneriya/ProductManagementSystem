using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagement.Entities.Models;

[Table("City")]
public partial class City
{
    [Key]
    public int CityId { get; set; }

    [Column(TypeName = "character varying")]
    public string CityName { get; set; } = null!;
}
