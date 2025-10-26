using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Shared.DTOs
{
    public class PaginationDTO
    {
        public int Id { get; set; }//Lo usaremos o no dependiendo del contexto 
        public int Page { get; set; } = 1; //Número de página que quiero que me traiga
        public int RecordsNumber { get; set; } = 10; //De a cuanto quiero paginar
        public string? Filter { get; set; }

    }

}
