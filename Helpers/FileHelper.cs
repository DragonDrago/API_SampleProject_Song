using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace MusicApi.Helpers
{
    public static class FileHelper
    {
        //This method Uploads file to azure and creates path for the file
        public static async Task<string> UploadFile(IFormFile file)
        {
            string connectionString = @"DefaultEndpointsProtocol=https;AccountName=musicstoragedownloadable;AccountKey=HnTaLvzGWDA0JY8Re8yCRtwWfvGoQI/Tb+d1iupsVbzGT+BF7IuxG79KIclC7Yh8GlsxUgvbUq5U+ASt+S576Q==;EndpointSuffix=core.windows.net";
            string containerName = "containermfile";
            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(Guid.NewGuid().ToString()+"_"+file.FileName);
            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            await blobClient.UploadAsync(memoryStream);
            return blobClient.Uri.AbsoluteUri;
        }

        public static async Task<string> UploadAudio(IFormFile file)
        {
            string connectionString = @"DefaultEndpointsProtocol=https;AccountName=musicstoragedownloadable;AccountKey=HnTaLvzGWDA0JY8Re8yCRtwWfvGoQI/Tb+d1iupsVbzGT+BF7IuxG79KIclC7Yh8GlsxUgvbUq5U+ASt+S576Q==;EndpointSuffix=core.windows.net";
            string containerName = "containermaudiofile";
            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(Guid.NewGuid().ToString() + "_" + file.FileName);
            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            await blobClient.UploadAsync(memoryStream);
            return blobClient.Uri.AbsoluteUri;
        }
    }
}
