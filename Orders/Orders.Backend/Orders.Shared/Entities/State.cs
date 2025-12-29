using Orders.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Shared.Entities
{
    public class State : IEntityWithName //Implementa a la interfaz IEntityWithName
    {
        public int Id { get; set; }

        [Display(Name = "Estado")] //Cuando cree una entidad que muestre el literal Estado
        //La longitud será como máximo 100 y si dea error que muestre el mensaje
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")] //Para que el campo sea abligatorio.
        public string Name { get; set; } = null!;

        //Propiedad que me indica la entidad con la que la entidad State se relaciona
        public int CountryId { get; set; }

        public Country? Country { get; set; }

        //Indicamos que un estado tiene una colección de ciudades y que, si no es nulo, me devuelva el número
        public ICollection<City>? Cities { get; set; }

        [Display(Name = "Ciudades")]
        public int CitiesNumber => Cities == null || Cities.Count == 0 ? 0 : Cities.Count;

    }

}
