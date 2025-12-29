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
            Database.SetCommandTimeout(600); //Aumentamos el tiempo de espera de las consultas a 600 segundos (10 minutos)
        }

        //Creamos una propiedad DbSet (que es un genérico) e indico la entidad/es que quiero mapear
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<TemporalOrder> TemporalOrders { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }


        //Queremos que en las tablas que vayamos a crear tengan un índice (en el campo Name) y que sea único para que no
        //se dupliquen los paises. Para ello, creamos el método siguiente
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); //EntityFramework aplica configuraciones internas y automáticas
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
