using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Common.Options;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Templates.BlobTemplates
{
    public class BlobFileProvider : IFileProvider
    {
        private readonly string _connectionString;
        public BlobFileProvider(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));
            _connectionString = connectionString;
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            throw new NotImplementedException();
        }

        public IFileInfo GetFileInfo(string subpath) =>
                                                        _fileInfoGetter(subpath)
                                                        .Map(x =>
                                                        {
                                                            _modifiedDates[subpath] = x.LastModified;
                                                            return x;
                                                        })
                                                        .Reduce(() => new NotFoundFileInfo(subpath));

        public IChangeToken Watch(string filter) => new VirtualChangeToken(() => GetModificationDate(filter), () => _fileInfoGetter(filter));

        private readonly ConcurrentDictionary<string, DateTimeOffset> _modifiedDates = new ConcurrentDictionary<string, DateTimeOffset>();
        private Option<DateTimeOffset> GetModificationDate(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            if (_modifiedDates.TryGetValue(fileName, out var dateTimeOffset))
                return dateTimeOffset;

            return Nothing.Value;
        }

        private Option<IFileInfo> _fileInfoGetter(string blobName) => GetFileInfoAsync(blobName).ConfigureAwait(false).GetAwaiter().GetResult();
        private async Task<Option<IFileInfo>> GetFileInfoAsync(string blobName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(blobName))
                throw new ArgumentNullException(nameof(blobName));

            if (blobName.StartsWith("/"))
                blobName = blobName.Substring(1);

            var blobServiceClient = new BlobServiceClient(_connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient("templates");
            await containerClient.CreateIfNotExistsAsync().ConfigureAwait(false);

            var blobClient = containerClient.GetBlobClient(blobName);

            if (!await blobClient.ExistsAsync(cancellationToken).ConfigureAwait(false))
                return Nothing.Value;

            return new BlobFileInfo(blobClient);
        }
    }
}
