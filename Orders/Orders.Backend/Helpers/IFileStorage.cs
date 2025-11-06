namespace Orders.Backend.Helpers
{
    public interface IFileStorage
    {
        Task<string> SaveFileAsync(byte[] content, string extention, string containerName);//Método que guarda un archvio en el blob Storage
        Task RemoveFileAsync(string path, string containerName);//Método que elemina un archivo del Blob storate.

        //Método que permite Modificar un fichero de Imagen.
        async Task<string> EditFileAsync(byte[] content, string extention, string containerName, string path)
        {
            if (path is not null)
            {
                await RemoveFileAsync(path, containerName);
            }
            return await SaveFileAsync(content, extention, containerName);
        }
    }

}
