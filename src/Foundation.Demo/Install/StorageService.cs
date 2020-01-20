using Foundation.Demo.Extensions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Foundation.Demo.Install
{
    public class StorageService : IStorageService
    {
        private readonly StorageCredentials _storageCredentials;
        private const string ContainerName = "foundation";

        public StorageService()
        {
            if (CloudStorageAccount.TryParse(Decrypt(ConfigurationManager.AppSettings["foundation:installBlobStorage"]), out var account))
            {
                _storageCredentials = account.Credentials;
                BlobRootUrl = VirtualPathUtility.AppendTrailingSlash(account.BlobStorageUri.PrimaryUri.ToString());
                IsInitialized = true;
            }
        }


        public string BlobRootUrl { get; }

        public bool IsInitialized { get; }

        public CloudBlockBlob Add(string filename, Stream stream, long length)
        {
            var blob = GetCloudBlockBlob(filename);
            var shouldUpload = true;
            if (blob.Exists())
            {
                blob.FetchAttributes();
                if (blob.Properties.Length == length)
                {
                    shouldUpload = false;
                }
            }

            if (shouldUpload)
            {
                blob.UploadFromStream(stream);
            }

            return blob;
        }

        public List<IListBlobItem> GetBlobItems(string path, int index, int length)
        {
            var container = GetContainer(ContainerName);
            var blobRequestOptions = new BlobRequestOptions();
            var operationContext = new OperationContext();

            return container.ListBlobs(path, false, BlobListingDetails.Metadata, blobRequestOptions, operationContext)
                .Skip(index)
                .Take(length)
                .ToList();
        }

        public CloudBlockBlob GetCloudBlockBlob(string path)
        {
            var container = GetContainer(ContainerName);
            return container.GetBlockBlobReference(path);
        }

        public bool Exists(string path)
        {
            var reference = GetCloudBlockBlob(path);
            return reference.Exists();
        }

        public void Delete(string path)
        {
            var reference = GetCloudBlockBlob(path);
            reference.DeleteIfExists();
        }

        public void Delete(Uri url)
        {
            var storageAccount = new CloudStorageAccount(_storageCredentials, true);
            var blobClient = storageAccount.CreateCloudBlobClient();
            blobClient.GetBlobReferenceFromServer(url)?.DeleteIfExists();
        }

        public List<CloudBlobDirectory> GetBlobDirectories(string path = null)
        {
            var container = GetContainer(ContainerName);
            var directory = container.GetDirectoryReference(path);
            if (directory == null)
            {
                return new List<CloudBlobDirectory>();
            }
            return GetFolders(directory);
        }

        public CloudBlobDirectory GetBlobDirectory(string path)
        {
            var container = GetContainer(ContainerName);
            return container.GetDirectoryReference(path);
        }

        public CloudBlobContainer GetContainer(string containerName,
            BlobContainerPublicAccessType blobContainerPublicAccessType = BlobContainerPublicAccessType.Off)
        {
            var storageAccount = new CloudStorageAccount(_storageCredentials, true);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference(containerName);
            blobContainer.CreateIfNotExists(blobContainerPublicAccessType);
            return blobContainer;
        }

        public void Rename(CloudBlockBlob blob, string newName)
        {
            if (blob == null)
            {
                return;
            }

            var blobCopy = blob.Container.GetBlockBlobReference(newName);

            if (blobCopy == null || blobCopy.Exists() || !blob.Exists())
            {
                return;
            }
            blobCopy.StartCopy(blob);
            blob.DeleteIfExists();
        }

        public CloudBlockBlob GetCloudBlockBlob(Uri url)
        {
            var storageAccount = new CloudStorageAccount(_storageCredentials, true);
            var blobClient = storageAccount.CreateCloudBlobClient();
            return blobClient.GetBlobReferenceFromServer(url) as CloudBlockBlob;
        }

        private List<CloudBlobDirectory> GetFolders(CloudBlobDirectory blobDirectory = null)
        {
            if (blobDirectory == null)
            {
                var container = GetContainer(ContainerName);
                var items = container.GetItems(string.Empty);
                return items.OfType<CloudBlobDirectory>().ToList();
            }
            var children = blobDirectory.GetItems();
            return children.OfType<CloudBlobDirectory>().ToList();
        }

        private const string mysecurityKey = "FoundationBlobKey";

        public static string Encrypt(string toEncrypt)
        {
            var toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
            var keyArray = Convert.FromBase64String("AAECAwQFBgcICQoLDA0ODw==");
            var tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            var cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string cipherString)
        {
            var toEncryptArray = Convert.FromBase64String(cipherString);
            var keyArray = Convert.FromBase64String("AAECAwQFBgcICQoLDA0ODw==");
            var tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            var cTransform = tdes.CreateDecryptor();
            var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Encoding.UTF8.GetString(resultArray);
        }

    }
}
