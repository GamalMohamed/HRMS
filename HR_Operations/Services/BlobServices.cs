using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using CoEX_HRMS.Models;
using CoEX_HRMS.Utils;
using System.Web.Mvc;

namespace CoEX_HRMS.Services
{
    public class BlobServices
    {
        private const string StorageName = "coexhrstore";
        private const string AccountKey = "1e7qXDXytKhin9tNInTLNdIJsGxm4pJLaQzOJV1957KQENdqvbRuyV/tOg37ekB2mplNzHVLJH7M/hlw1NzB6w==";
        public readonly string BlobUrl = "https://coexhrstore.blob.core.windows.net/";

        public StorageCredentials Credentials { get; set; }

        public CloudStorageAccount StorageAccount { get; set; }

        public CloudBlobClient BlobClient { get; set; }

        public CloudBlobContainer BlobContainer { get; set; }

        public BlobServices()
        {
            Credentials = new StorageCredentials(StorageName, AccountKey);
            StorageAccount = new CloudStorageAccount(Credentials, true);
            BlobClient = StorageAccount.CreateCloudBlobClient();
        }

        private static byte[] ByteArray(Image img)
        {
            using (var ms = new MemoryStream())
            {
                img.Save(ms, img.RawFormat);
                return ms.ToArray();
            }
        }

        public void BlobImageUpload(string image, string path,string type)
        {
            
            BlobContainer = BlobClient.GetContainerReference(type);
            var blockBlob = BlobContainer.GetBlockBlobReference(image);
            var x = Path.GetExtension(path)?.ToLower();
            if (x != ".pdf")
            {
                var image1 = Image.FromFile(path);
                var arr = ByteArray(image1);
                var stream = new MemoryStream(arr);
                blockBlob.UploadFromStream(stream);
            }
            else
            {
                using (FileStream file = File.OpenRead(path))
                {
                    blockBlob.UploadFromStream(file);
                }
            }
        }
        
        public void BlobDelete(string blobName, string containerName)
        {
            BlobContainer = BlobClient.GetContainerReference(containerName);
            var blockBlob = BlobContainer.GetBlockBlobReference(blobName);
            blockBlob.Delete();
        }

        //download all files
        public  void DownlaodAll(string contianerName)
        {
            CloudBlobContainer contianner = BlobClient.GetContainerReference(contianerName);
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            string[] arr = userName.Split('\\');
            string path = $@"C:\Users\{arr[1]}\Downloads\";
            var list = contianner.ListBlobs();
            Console.WriteLine(list.Count());
            string[] FilesName = new string[list.Count()];
            int i = 0;
            foreach (var blob in list)
            {
                string[] Name = blob.Uri.AbsolutePath.Split('/');
                FilesName[i++] = Name[2];
                Console.WriteLine(Name[2]);
                CloudBlockBlob blockBlob = contianner.GetBlockBlobReference(Name[2]);
                System.IO.Directory.CreateDirectory($@"{path}ImagesPath");
                using (var fileStream = System.IO.File.OpenWrite($@"{path}\ImagesPath\{Name[2]}"))
                {
                    blockBlob.DownloadToStream(fileStream);
                }
                
            }

        }

    }
}