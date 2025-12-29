using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Shared.Responses
{
    public class ActionResponse<T> //Creo un genérico llamado T:Tile. Va a ser una acción response
    {
        public bool WasSuccess { get; set; }//Si la respuesta fue bien devuelve true sino false
        public string? Message { get; set; }//Mensaje que le vamos a devolver si falla
        public T? Result { get; set; }//Me va a devolver una propiedad tipo T a lo que yo le mande
                                      //esa propiedad T viene de lo que le ponga en ActionResponse                                      
    }

}
