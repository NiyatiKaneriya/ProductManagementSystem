namespace ProductManagement.Entities.ViewModels
{
    public class ProductListParams
    {
        public int Categoryfilter {  get; set; }
        public string GeneralSearch {  get; set; }
        public string Multiselectlist {  get; set; }
        public string ColumnName { get; set; }
        public string Sorttype { get; set; }
        public int Page {  get; set; } = 1;
        public int PageSize { get; set; } 
    }
}
