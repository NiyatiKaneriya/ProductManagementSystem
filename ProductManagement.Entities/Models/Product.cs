using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductManagement.Entities.Models;

public partial class Product
{
    [Key]
    public int ProductId { get; set; }

    [Column(TypeName = "character varying")]
    public string ProductName { get; set; } = null!;

    public int CategoryId { get; set; }

    public int Price { get; set; }

    [Column(TypeName = "character varying")]
    public string? SupplierName { get; set; }

    [Column(TypeName = "character varying")]
    public string? SupplierEmail { get; set; }

    [Column(TypeName = "character varying")]
    public string? SupplierPhoneNo { get; set; }

    [Column(TypeName = "character varying")]
    public string? ProductDescription { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? AvailableFrom { get; set; }

    [Column(TypeName = "character varying")]
    public string? ProductWebsite { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "character varying")]
    public string AvailableAt { get; set; } = null!;

    [Column(TypeName = "character varying")]
    public string? FilePath { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? ModifiedDate { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category Category { get; set; } = null!;
}
