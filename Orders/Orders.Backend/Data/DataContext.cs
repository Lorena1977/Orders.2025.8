
using Microsoft.EntityFrameworkCore;
using Orders.Share.Entities;

namespace Orders.Backend.Data
{
    public class DataContext: DbContext //Esta insturcción indica que la clase DataContext Hereda de la clase llamada DbContext (clase que da el entityFramework)
    {
        //Creamos un constructor a la clase
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        
        protected DataContext()
        {
        }

        public DbSet<Country> Countries { get; set; } //Countries hace referencia a la coleccion que estoy creando (Countries representa a todos los paises que tenga en la base de datos)

        //Queremos que la tabla country tenga un indice para que no se duplique los paises.
        //Se trata de una validación en base de datos
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique();//Indicamos que la tabla Country tiene un índice único
        }



    }
}
