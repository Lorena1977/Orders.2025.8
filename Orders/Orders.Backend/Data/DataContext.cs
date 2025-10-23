using Microsoft.EntityFrameworkCore;
using Orders.Shared.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Orders.Backend.Data
{
    public class DataContext : DbContext //La clase DataContext hereda de la clase DbContext.
    {
        //Creamos el constructor de la clase tal como se indica.
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        //Creamos una propiedad DbSet (que es un genérico) e indico la entidad que quiero mapear
        public DbSet<Country> Countries { get; set; }

        //Queremos que la tabla Country tenga un índice (en el campo Name) y que sea único para que no
        //se dupliquen los paises. Para ello, creamos el método siguiente
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique();
        }
    }

}
