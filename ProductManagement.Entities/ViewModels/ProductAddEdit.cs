using Microsoft.AspNetCore.Http;
using ProductManagement.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Entities.ViewModels
{
    public class ProductAddEdit
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string Category {  get; set; }
        public int Price { get; set; }
        public string SupplierName { get; set; }
        public string SupplierEmail { get; set; }
        public string SupplierPhoneNo { get; set; }
        public string ProductDescription { get; set; }
        public DateTime AvailableFrom { get; set; }
        public string ProductWebsite { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string AvailableAt { get; set; }
        public string AvailableAtIds { get; set; }
        public List<int> AvailableAtcity { get; set; }
        public IFormFile ProductImage { get; set; }
        public string FilePath { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<ProductAddEdit> Products { get; set; }

        public bool AcendingSort { get; set; }

    }
}
