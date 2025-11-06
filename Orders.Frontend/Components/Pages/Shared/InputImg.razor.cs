using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Orders.Frontend.Components.Pages.Shared
{
    public partial class InputImg
    {
        private string? imageBase64; //Coge la foto y me la convierta a base64. Como queremos que sea un 
        //componente genérico le pasamos 3 parámetros.
        [Parameter] public string Label { get; set; } = "Imagén";
        [Parameter] public string? ImageURL { get; set; } //Ruta de mi imagen
        [Parameter] public EventCallback<string> ImageSelected { get; set; }//Cuando seleccionen la imagen
        //Vamos a lanzar una acción.

        private const long MaxFileSize = 10 * 1024 * 1024;

        //Método que sirva para seleccionar varias imágenes a la vez.(Convertimos la imagen)
        private async Task OnChange(InputFileChangeEventArgs e)
        {
            var imagenes = e.GetMultipleFiles();

            foreach (var imagen in imagenes)
            {
                try
                {
                    using var stream = imagen.OpenReadStream(MaxFileSize);
                    using var ms = new MemoryStream();
                    await stream.CopyToAsync(ms);

                    var arrBytes = ms.ToArray();
                    imageBase64 = Convert.ToBase64String(arrBytes);
                    ImageURL = null;

                    await ImageSelected.InvokeAsync(imageBase64);
                }
                catch (IOException)
                {
                    await ImageSelected.InvokeAsync(string.Empty);
                }
                catch (UnauthorizedAccessException)
                {
                    await ImageSelected.InvokeAsync(string.Empty);
                }
            }
        }
    }

}