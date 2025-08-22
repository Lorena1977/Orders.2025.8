using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Share.Entities
{
    public class Country
    {
        public int Id { get; set; }

        [Display(Name = "País")] //Cuando cree una entidad que muestre el literal País
        //La longitud del campo será como máximo 100 y si da error, que muestre el mensaje.
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")] //Para que el campo sea obligatorio
        public string Name { get; set; } = null!;
    }
}