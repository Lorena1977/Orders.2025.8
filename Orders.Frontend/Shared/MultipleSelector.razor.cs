using Microsoft.AspNetCore.Components;
using Orders.Frontend.Helpers;

namespace Orders.Frontend.Shared
{
    public partial class MultipleSelector
    {
        private string addAllText = ">>"; //Seleccionar todas (lo utilizaremos en un botón)
        private string removeAllText = "<<"; //Deseleccionar todas(lo utilizaremos en un botón)

        [Parameter]
        public List<MultipleSelectorModel> NonSelected { get; set; } = new();//Lista de categorias no seleccionadas

        [Parameter]
        public List<MultipleSelectorModel> Selected { get; set; } = new(); //Lista de categorias seleccionadas



        //Metodo para agregar una lista
        private void Select(MultipleSelectorModel item)
        {
            NonSelected.Remove(item); //Quitamos de las noseleccionadas
            Selected.Add(item);//la añadimos en las seleccionadas
        }

        //Metodo para quitar de una lista
        private void Unselect(MultipleSelectorModel item)
        {
            Selected.Remove(item); //Quitamos de las seleccionadas
            NonSelected.Add(item); //Las añadimos en la NO seleccionadas
        }

        //Metodo que selecciona todas
        private void SelectAll()
        {
            Selected.AddRange(NonSelected);
            NonSelected.Clear();
        }

        //Método que deseleccina todas
        private void UnselectAll()
        {
            NonSelected.AddRange(Selected);
            Selected.Clear();
        }
        private void TestClick()
        {
            Console.WriteLine("¡Botón presionado!");
        }

    }

}