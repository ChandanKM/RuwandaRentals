using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace App.Common
{
    public  class BlobUtilities
    {
        // Retrieve storage account from connection string.
        // Credentials required for cloud blob images storage.


        // private static CloudStorageAccount StorageAccount =
        // CloudStorageAccount.Parse("DefaultEndpointsProtocol=http;AccountName=lmk;AccountKey=+iJrezRlA5j0l8zfl5cGhYOzo+e9DwMifpULgvmvB183Q8d6j4pG0CBs1/Kc8NjUIpfkD+bbfYv6Z9MxHR8v2Q==;");


        private static CloudStorageAccount StorageAccount =
         CloudStorageAccount.Parse("F:/New Dot net proj Pavan/LMKCloudBlob");

        /// <summary>
        /// creates the blob
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="blobName"></param>
        /// <param name="contentType"></param>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public static string CreateBlob(string containerName, string blobName, string contentType, Stream fileStream)
        {
            try
            {


                // Create the blob client.
                var blobClient = StorageAccount.CreateCloudBlobClient();

                // Retrieve reference to a previously created container.
                var container = blobClient.GetContainerReference(containerName.ToLower());

                // create if not exist and set permission to public
                if (!container.Exists())
                {

                    // Create the container if it doesn't already exist.
                    container.CreateIfNotExists();
                    //set public access
                    container.SetPermissions(
                        new BlobContainerPermissions
                            {
                                PublicAccess =
                                    BlobContainerPublicAccessType.Blob
                            });
                }

                // Retrieve reference to a blob 
                var blockBlob = container.GetBlockBlobReference(blobName.ToLower());
                //add content type
                blockBlob.Properties.ContentType = contentType;
                //upload the stream
                blockBlob.UploadFromStream(fileStream);
                //returs the url of the blob
                return blockBlob.Uri.AbsoluteUri;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        /// <summary>
        /// retrieves the blob if present
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="blobName"></param>
        /// <returns></returns>
        public static string RetrieveBlobUrl(string containerName, string blobName)
        {
            try
            {


                // Create the blob client.
                var blobClient = StorageAccount.CreateCloudBlobClient();

                // Retrieve reference to a previously created container.
                var container = blobClient.GetContainerReference(containerName.ToLower());

                // Retrieve reference to a blob 
                var blockBlob = container.GetBlockBlobReference(blobName.ToLower());

                //returs the url of the blob if exists
                if (BlobExists(blockBlob))
                {
                    return blockBlob.Uri.AbsoluteUri;
                }
                return null;

            }
            catch (Exception)
            {

                return null;
            }
        }

        /// <summary>
        /// to check if the blob exists or not
        /// </summary>
        /// <param name="blob"></param>
        /// <returns></returns>
        public static bool BlobExists(CloudBlockBlob blob)
        {
            try
            {
                // if the  blob does not exists it will give an exception
                blob.FetchAttributes();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        /// <summary>
        /// retrieves the url path of all the blobs by container name
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        public static List<string> GetAllblob(string containerName)
        {

            try
            {
                // Create the blob client.
                var blobClient = StorageAccount.CreateCloudBlobClient();

                // Retrieve reference to a previously created container.
                var container = blobClient.GetContainerReference(containerName.ToLower());

                //List blobs and directories in this container hierarchically (which is the default listing).
                return container.ListBlobs().Select(blobItem => blobItem.Uri.AbsoluteUri).ToList();

            }
            catch (Exception)
            {

                return null;
            }

        }
    }
}
