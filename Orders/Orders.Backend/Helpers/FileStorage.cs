using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Orders.Backend.Helpers
{
    public class FileStorage : IFileStorage //Hereda del IFileStorage
    {
        private readonly string _connectionString;

        //Creamos un constructor que acceda a la configuración que lea la cadena de conexión.
        public FileStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AzureStorage")!;
        }

        //Método que elimina un archivo del BlobStorage.
        public async Task RemoveFileAsync(string path, string containerName)
        {
            var client = new BlobContainerClient(_connectionString, containerName);//Creamos un objeto con la cadena de conexión y el contenedor. 
            await client.CreateIfNotExistsAsync();
            var fileName = Path.GetFileName(path);
            var blob = client.GetBlobClient(fileName);//creo un objeto del blob y obtengo el nombre del archivo
            await blob.DeleteIfExistsAsync();//Borra el archivo si existe.
        }

        //Método que guarda un archivo en el blobStorage.
        public async Task<string> SaveFileAsync(byte[] content, string extention, string containerName)
        {
            var client = new BlobContainerClient(_connectionString, containerName);
            await client.CreateIfNotExistsAsync();
            client.SetAccessPolicy(PublicAccessType.Blob);
            var fileName = $"{Guid.NewGuid()}{extention}"; //Le da un nombre al archivo irrepetible.
            var blob = client.GetBlobClient(fileName);

            using (var ms = new MemoryStream(content))
            {
                await blob.UploadAsync(ms);
            }
            return blob.Uri.ToString();//Devuelve la ruta de como almacenamos la ruta.
        }
    }

}
