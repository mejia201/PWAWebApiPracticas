using Microsoft.EntityFrameworkCore;

namespace P01WebApi.Models
{
    public class equiposContext: DbContext
    {
        public equiposContext(DbContextOptions<equiposContext> options) : base(options)
        {

        }


        //Se presenta la tabla
        public DbSet<equipos> equipos { get; set; }
    }
}
