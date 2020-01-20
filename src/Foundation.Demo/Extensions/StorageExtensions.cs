using Foundation.Demo.ViewModels;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Demo.Extensions
{
    public static class StorageExtensions
    {
        public static async Task<List<IListBlobItem>> GetItemsAsync(this CloudBlobDirectory directory, int maxResults = 100)
        {
            var blobContinuationToken = new BlobContinuationToken();
            var blobRequestOptions = new BlobRequestOptions();
            var operationContext = new OperationContext();
            var results = new List<IListBlobItem>();
            do
            {
                var result = await directory.ListBlobsSegmentedAsync(false, BlobListingDetails.Metadata, maxResults, blobContinuationToken, blobRequestOptions, operationContext);
                blobContinuationToken = result.ContinuationToken;
                results.AddRange(result.Results);
            }
            while (blobContinuationToken != null);
            return results;
        }

        public static async Task<List<IListBlobItem>> GetItemsAsync(this CloudBlobContainer container, string prefix, int maxResults = 100)
        {
            var blobContinuationToken = new BlobContinuationToken();
            var blobRequestOptions = new BlobRequestOptions();
            var operationContext = new OperationContext();
            var results = new List<IListBlobItem>();
            do
            {
                var result = await container.ListBlobsSegmentedAsync(prefix, false, BlobListingDetails.Metadata, maxResults, blobContinuationToken, blobRequestOptions, operationContext);
                blobContinuationToken = result.ContinuationToken;
                results.AddRange(result.Results);
            }
            while (blobContinuationToken != null);
            return results;
        }

        public static List<IListBlobItem> GetItems(this CloudBlobDirectory directory, int maxResults = 100)
        {
            var blobContinuationToken = new BlobContinuationToken();
            var blobRequestOptions = new BlobRequestOptions();
            var operationContext = new OperationContext();
            var results = new List<IListBlobItem>();
            do
            {
                var result = directory.ListBlobsSegmented(false, BlobListingDetails.Metadata, maxResults, blobContinuationToken, blobRequestOptions, operationContext);
                blobContinuationToken = result.ContinuationToken;
                results.AddRange(result.Results);
            }
            while (blobContinuationToken != null);
            return results;
        }

        public static List<IListBlobItem> GetItems(this CloudBlobContainer container, string prefix, int maxResults = 100)
        {
            var blobContinuationToken = new BlobContinuationToken();
            var blobRequestOptions = new BlobRequestOptions();
            var operationContext = new OperationContext();
            var results = new List<IListBlobItem>();
            do
            {
                var result = container.ListBlobsSegmented(prefix, false, BlobListingDetails.Metadata, maxResults, blobContinuationToken, blobRequestOptions, operationContext);
                blobContinuationToken = result.ContinuationToken;
                results.AddRange(result.Results);
            }
            while (blobContinuationToken != null);
            return results;
        }

        public static List<CloudBlobDirectory> GetBlobDirectories(this List<IListBlobItem> items) => items.OfType<CloudBlobDirectory>().ToList();

        public static List<CloudBlockBlob> GetBlobs(this List<IListBlobItem> items) => items.OfType<CloudBlockBlob>().ToList();

        public static AzureBlob GetAzureBlob(this CloudBlobContainer item)
        {
            return new AzureBlob
            {
                Name = item.Name,
                IsContainer = true,
                LastModified = item?.Properties.LastModified?.UtcDateTime ?? DateTime.UtcNow,
                Etag = item?.Properties.ETag
            };
        }

        public static AzureBlob GetAzureBlob(this IListBlobItem item)
        {
            var directory = item as CloudBlobDirectory;
            if (directory != null)
            {
                return new AzureBlob
                {
                    Name = directory.Prefix,
                    IsDirectory = true,
                    Url = directory.Uri.ToString()
                };
            }

            var blob = item as CloudBlockBlob;
            if (blob != null)
            {
                var name = blob.Name.Substring(blob.Name.LastIndexOf('/') == 0 ? 0 : blob.Name.LastIndexOf('/') + 1);
                return new AzureBlob
                {
                    Name = name.Substring(0, name.LastIndexOf('.') == 0 ? name.Length : name.LastIndexOf('.')),
                    IsBlob = true,
                    LastModified = blob.Properties?.LastModified?.UtcDateTime ?? DateTime.UtcNow,
                    Etag = blob.Properties?.ETag,
                    BlobType = blob.Properties?.BlobType.ToString(),
                    ContentType = blob.Properties?.ContentType,
                    Size = blob.Properties?.Length ?? 0,
                    Status = blob.Properties?.LeaseStatus.ToString(),
                    Url = blob.Uri.ToString()
                };
            }
            return null;
        }
    }
}
