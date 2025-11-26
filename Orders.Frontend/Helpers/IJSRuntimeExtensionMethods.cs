using Microsoft.JSInterop;

namespace Orders.Frontend.Helpers
{
    public static class IJSRuntimeExtensionMethods
    {

        //Método para grabar el Token en  el LocalStorage
        public static ValueTask<object> SetLocalStorage(this IJSRuntime js, string key, string content)
        {
            return js.InvokeAsync<object>("localStorage.setItem", key, content);
        }

        //Devuelve el objeto Token almacenado en el LocalStorage
        public static ValueTask<object> GetLocalStorage(this IJSRuntime js, string key)
        {
            return js.InvokeAsync<object>("localStorage.getItem", key);
        }

        //Borra el Token del LocalStorage
        public static ValueTask<object> RemoveLocalStorage(this IJSRuntime js, string key)
        {
            return js.InvokeAsync<object>("localStorage.removeItem", key);
        }
    }

}
