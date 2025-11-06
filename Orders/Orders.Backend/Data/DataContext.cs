using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Orders.Shared.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Orders.Backend.Data
{
    public class DataContext : IdentityDbContext<User>//DbContext //La clase DataContext hereda de IdentityDbContext basandose en la definición de usuarios
    {
        //Creamos el constructor de la clase tal como se indica.
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        //Creamos una propiedad DbSet (que es un genérico) e indico la entidad/es que quiero mapear
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }



        //Queremos en las tablas que vayaos a crear tengan un índice (en el campo Name) y que sea único para que no
        //se dupliquen los paises. Para ello, creamos el método siguiente
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();//Indicamos que la tabla categoría tiene un índice único
            modelBuilder.Entity<City>().HasIndex(c => new { c.StateId, c.Name }).IsUnique();
            modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique(); //Indicamos que la tabla Country tiene un índice único.
            modelBuilder.Entity<State>().HasIndex(s => new { s.CountryId, s.Name }).IsUnique();
            modelBuilder.Entity<Product>().HasIndex(x => x.Name).IsUnique();
            DisableCascadingDelete(modelBuilder); 
        }
        //Método que deshabilita el borrado en cascada.(Si se borra un estado que no lo borre para todos los paises)
        private void DisableCascadingDelete(ModelBuilder modelBuilder)
        {
            var relationships = modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());
            foreach (var relationship in relationships)
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

    }

}
