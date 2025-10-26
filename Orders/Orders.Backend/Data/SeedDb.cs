using Orders.Shared.Entities;

namespace Orders.Backend.Data
{
    public class SeedDb
    {
        private readonly DataContext _context; //Campo de la clase para que permanezca siempre y no solo en el constructor.

        public SeedDb(DataContext context) //Inyectamos la conexión a la base de datos mediante el constructor
        {
            _context = context;
        }

        //Siempre que arranque la aplicación, va a pasar por este método SeedAsync().
        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();//Si no existe la base de datos la crea
            //si existe la base de datos y tiene asiganciones pendiente, las hace, si no tiene actualizaciones
            //pendietes no hace nada.
            await CheckCountriesAsync();//Método para que garantice que tenemos Countries.
            await CheckCategoriesAsync();//Método para que se garantice que tenemos entidades.
        }

        //Implementamos los métodos
        //private async Task CheckCountriesAsync()
        //{
        //    if (!_context.Countries.Any())//Si no hay countries, me crea dos.
        //    {
        //        _context.Countries.Add(new Country { Name = "Colombia" });
        //        _context.Countries.Add(new Country { Name = "Estados Unidos" });
        //    }

        //    await _context.SaveChangesAsync();
        //}

        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country
                {
                    Name = "Colombia",
                    States = [
                        new State()
                {
                    Name = "Antioquia",
                    Cities = [
                        new City() { Name = "Medellín" },
                        new City() { Name = "Itagüí" },
                        new City() { Name = "Envigado" },
                        new City() { Name = "Bello" },
                        new City() { Name = "Rionegro" },
                    ]
                },
                new State()
                {
                    Name = "Bogotá",
                    Cities = [
                        new City() { Name = "Usaquen" },
                        new City() { Name = "Champinero" },
                        new City() { Name = "Santa fe" },
                        new City() { Name = "Useme" },
                        new City() { Name = "Bosa" },
                    ]
                },
            ]
                });
                _context.Countries.Add(new Country
                {
                    Name = "Estados Unidos",
                    States = [
                        new State()
                {
                    Name = "Florida",
                    Cities = [
                        new City() { Name = "Orlando" },
                        new City() { Name = "Miami" },
                        new City() { Name = "Tampa" },
                        new City() { Name = "Fort Lauderdale" },
                        new City() { Name = "Key West" },
                    ]
                },
                new State()
                    {
                        Name = "Texas",
                        Cities = [
                            new City() { Name = "Houston" },
                            new City() { Name = "San Antonio" },
                            new City() { Name = "Dallas" },
                            new City() { Name = "Austin" },
                            new City() { Name = "El Paso" },
                        ]
                    },
                ]
                });
            }
            await _context.SaveChangesAsync();
        }

        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categories.Any()) //Si no hay categorías, me crea dos.
            {
                _context.Categories.Add(new Category { Name = "Calzado" });
                _context.Categories.Add(new Category { Name = "Tecnología" });
            }

            await _context.SaveChangesAsync();
        }
    }

}
