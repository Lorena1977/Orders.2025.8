using Orders.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Shared.Entities
{
    public class Country : IEntityWithName //Implementa la interfaz IEntityWithName
    {
        public int Id { get; set; }

        [Display(Name = "País")] //Cuando cree una entidad que muestre el literal País
        //La longitud del campo será como máximo 100 y si da error, que muestre el mensaje
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]//Para que el campo sea obligatorio
        public string Name { get; set; } = null!;

        //Indicamos que un Country tiene muchos estados
        /// y creamos una propiedad de lectura StatesNumber que me devuelva el número de estados (si no es nulo).
        public ICollection<State>? States { get; set; }

        [Display(Name = "Estados/Departamentos")]
        public int StatesNumber => States == null || States.Count == 0 ? 0 : States.Count;

    }

}
