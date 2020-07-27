using System;
using System.IO;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.FileProviders;

namespace Templates.BlobTemplates
{
    public class BlobFileInfo : IFileInfo
    {
        private readonly BlobClient _blobClient;
        private readonly BlobProperties _blobProperties;
        public BlobFileInfo(BlobClient blobClient) 
        {
            _blobClient = blobClient ?? throw new ArgumentNullException(nameof(blobClient));
            _blobProperties = _blobClient
                                        .GetPropertiesAsync()
                                        .ConfigureAwait(false)
                                        .GetAwaiter()
                                        .GetResult().Value;
        }

        public bool Exists => true;
        public bool IsDirectory => false;
        public DateTimeOffset LastModified => _blobProperties.LastModified;
        public long Length => _blobProperties.ContentLength;
        public string Name => _blobClient.Name;
        public string PhysicalPath => null;
        public Stream CreateReadStream() 
        {
            var stream = new MemoryStream();
            _blobClient.Download().Value.Content.CopyTo(stream);
            return stream;
        }
    }
}
