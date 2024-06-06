using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Entities.ViewModels
{
    public class CategoryDetails
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
        public List<CategoryDetails>? Categories { get; set; }
    }
}
