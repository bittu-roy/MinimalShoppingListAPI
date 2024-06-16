//For Databse configuration we need DBContext and this file is where we will configure it

using Microsoft.EntityFrameworkCore;

namespace MinimalShoppingListAPI
{
    public class ApiDbContext: DbContext
    {

        public DbSet<Grocery> Groceries => Set<Grocery>();

        //options is used to configure database according to our own.
        public ApiDbContext(DbContextOptions<ApiDbContext> options) :base(options)
        {
            
        }
    }
}
