using Azure.Storage.Blobs;
using Infraestructura.Storage.Interfaces;
using Infraestructura.Transversal.Log.Enumeracion;
using Infraestructura.Transversal.Log.Implementacion;
using Infraestructura.Transversal.Log.Modelo;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Infraestructura.Storage.Impl
{
    public class HistoricosStorageInfraestructura : IHistoricosStorageInfraestructura
    {
        BlobServiceClient _blobServiceClient { get; }
        public HistoricosStorageInfraestructura(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<string> ObtenerActa(string rutaArchivo)
        {
            string acta = "No se encuentra el acta notarial";

            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient("bscaeuprdhistoricos");
            BlobClient blobClient = container.GetBlobClient(rutaArchivo);

            if (blobClient != null && blobClient.Exists().Value == true)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await blobClient.DownloadToAsync(memoryStream);

                    var bytes = memoryStream.ToArray();
                    var b64String = Convert.ToBase64String(bytes);

                    acta = b64String;
                }
            }

            return acta;
        }
    }
}