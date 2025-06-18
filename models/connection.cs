using Microsoft.EntityFrameworkCore;

namespace Product_Catalog.models
{
    public class connection : DbContext
    {
        public connection(DbContextOptions<connection> options) : base(options) { }
        public DbSet<product> products { get; set; }
    }
}
