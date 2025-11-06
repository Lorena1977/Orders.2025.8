using Microsoft.AspNetCore.Components;

namespace Orders.Frontend.Components.Pages.Shared
{
    public partial class GenericList<Titem>
    {
        [Parameter] public RenderFragment? Loading { get; set; } //Lo que vemos cuando está cargando
        [Parameter] public RenderFragment? NoRecords { get; set; } //Lo que vemos cuando no hay registros
        [EditorRequired, Parameter] public RenderFragment? Body { get; set; } = null; //Body obligatorio

        [EditorRequired, Parameter] public List<Titem> MyList { get; set; } = null!; //Lista 
    }
}