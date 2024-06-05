using ProductManagement.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Entities.ViewModels
{
    public class CategoryAddEdit
    {
        public int? CategoryId { get; set; }
        [Required]
        public  string CategoryName { get; set; }
        [Required]
        public int Sequence { get; set; }
        public int? TotalProducts { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set;}
        public bool? IsDeleted { get; set; }
        public List<CategoryAddEdit>? Categories { get; set; }
    }
}
