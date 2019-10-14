﻿using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using LifeBackup.Core.Communication.Files;
using LifeBackup.Core.Communication.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeBackup.Infrastructure.Reposiotories
{
    public class FilesRepository : IFilesRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IAmazonS3 _s3Client;


        public FilesRepository(IAmazonS3 s3Cliente)
        {
            _s3Client = s3Cliente;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="formFiles"></param>
        /// <returns></returns>
        public async Task<AddFileResponse> UploadFiles(string bucketName, IList<IFormFile> formFiles)
        {
            var response = new List<string>();

            foreach (var file in formFiles)
            {
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = file.OpenReadStream(),
                    Key = file.FileName,
                    BucketName = bucketName,
                    CannedACL = S3CannedACL.NoACL

                };


                using (var fileTransferUtility = new TransferUtility(_s3Client))
                {
                    await fileTransferUtility.UploadAsync(uploadRequest);

                }


                var expiryUrlRequest = new GetPreSignedUrlRequest
                {
                    BucketName = bucketName,
                    Key = file.FileName,
                    Expires = DateTime.Now.AddDays(1)
                };

                var url = _s3Client.GetPreSignedURL(expiryUrlRequest);


                response.Add(url);

            }
            return new AddFileResponse
            {
                PreSignedUrl = response
            };
        }




        public async Task<IEnumerable<ListFilesResponse>> ListFiles(string bucketName)
        {
            var responses = await _s3Client.ListObjectsAsync(bucketName);

            return responses.S3Objects.Select(b => new ListFilesResponse
            {
                BucketName = b.BucketName,
                Key = b.Key,
                Owner = b.Owner.DisplayName,
                Size = b.Size
            });


        }




        public async Task DownloadFile(string bucketName, string fileName)
        {
            var pathAndFileName = $"C:\\Users\\magef\\AppData\\Local\\CSReporterV3\\{fileName}";

            var downloadRequest = new TransferUtilityDownloadRequest
            {
                BucketName = bucketName,
                Key = fileName,
                FilePath = pathAndFileName
            };

            using (var transferUtility = new TransferUtility(_s3Client))
            {
                await transferUtility.DownloadAsync(downloadRequest);
            };



        }



        public async Task<DeleteFileResponse> DeleteFile(string bucketName, string fileName)
        {
            var multiObjectDeleteRequest = new DeleteObjectsRequest
            {
                BucketName = bucketName
            };

            multiObjectDeleteRequest.AddKey(fileName);


            var response = await _s3Client.DeleteObjectsAsync(multiObjectDeleteRequest);


            return new DeleteFileResponse
            {
                NumerOfDeletedObjects = response.DeletedObjects.Count
            };

        }



        public async Task AddJsonObject(string buckectName, AddJsonObjectRequest request)
        {
            var createOnUtc = DateTime.UtcNow;

            var s3Key = $"{createOnUtc:yyyy}/{createOnUtc:MM}/{createOnUtc:dd}/{request.Id}";

            var putObjectRequest = new PutObjectRequest
            {
                BucketName = buckectName,
                Key = s3Key,
                ContentBody = JsonConvert.SerializeObject(request)
            };

            await _s3Client.PutObjectAsync(putObjectRequest);


        }


        public async Task<GetJsonObjectResponse> GetJsonObject(string bucketName, string fileName)
        {
            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = fileName
            };

            var response = await _s3Client.GetObjectAsync(request);

            using (var reader = new StreamReader(response.ResponseStream))
            {
                var contest = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<GetJsonObjectResponse>(contest);
            }

        }



    }






}



