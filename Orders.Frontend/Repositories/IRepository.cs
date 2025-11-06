namespace Orders.Frontend.Repositories
{
    public interface IRepository
    {
        Task<HttpResponseWrapper<T>> GetAsync<T>(string url);

        Task<HttpResponseWrapper<object>> PostAsync<T>(string url, T model);
        
        Task<HttpResponseWrapper<TActionResponse>> PostAsync<T, TActionResponse>(string url, T model);//Metodo sobrecargado del Post
        //le mando un modelo y el me manda otra cosa, no siempre lo que mando es lo que recibo. (esto se usará en los productos)

        Task<HttpResponseWrapper<object>> DeleteAsync(string url);

        Task<HttpResponseWrapper<object>> PutAsync<T>(string url, T model);

        Task<HttpResponseWrapper<TActionResponse>> PutAsync<T, TActionResponse>(string url, T model);//Método sobrecargado del Put.
        //Lo usaré cuando lo que yo mando no es igual que lo que yo recibo
        Task<HttpResponseWrapper<object>> GetAsync(string url);
    }
}
