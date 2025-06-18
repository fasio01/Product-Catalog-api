using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product_Catalog.models
{
    public class product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public IFormFile? File { get; set; }
      public string ImagePath { get; set; }
    }
}
