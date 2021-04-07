using System;

namespace EM.API.Models
{
    public class SubCategory
    {
        public Guid SubCategoryID { get; set; }
        public Guid CategoryID { get; set; }
        public string SubCategoryName { get; set; }
        public int Sequence { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}