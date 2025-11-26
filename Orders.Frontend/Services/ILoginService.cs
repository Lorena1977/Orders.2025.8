namespace Orders.Frontend.Services
{
    public interface ILoginService
    {
        Task LoginAsync(string token); //Le mandamos el Token y devuelve el método de login y registrar
        Task LogoutAsync();//
    }

}
