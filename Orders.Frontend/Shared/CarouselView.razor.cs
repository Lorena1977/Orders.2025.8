using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Orders.Frontend.Shared
{
    public partial class CarouselView
    {
        private bool arrows = true; //Fleclas que me dejan desplazarme
        private bool bullets = true; //Muestra imagenes que tiene mi carrusel
        private bool enableSwipeGesture = true; //Para desplazarnos de izquierda a derecha
        private bool autocycle = true;
        private Transition transition = Transition.Slide; //Como quiero que sea la transición.

        [Parameter, EditorRequired] public List<string> Images { get; set; } = null!;
    }

}