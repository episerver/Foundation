using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;

namespace Foundation.Demo.Install
{
    public interface IStorageService
    {
        string BlobRootUrl { get; }
        bool IsInitialized { get; }
        List<IListBlobItem> GetBlobItems(string path, int index, int length);
        List<CloudBlobDirectory> GetBlobDirectories(string path = null);
        CloudBlobDirectory GetBlobDirectory(string path);
        CloudBlobContainer GetContainer(string containerName, BlobContainerPublicAccessType blobContainerPublicAccessType = BlobContainerPublicAccessType.Off);
        CloudBlockBlob GetCloudBlockBlob(string path);
        CloudBlockBlob GetCloudBlockBlob(Uri url);
        CloudBlockBlob Add(string filename, Stream stream, long length);
        void Delete(string path);
        void Delete(Uri url);
        void Rename(CloudBlockBlob blob, string newName);
        bool Exists(string path);
    }
}
