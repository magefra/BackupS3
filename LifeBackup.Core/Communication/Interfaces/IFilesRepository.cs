using LifeBackup.Core.Communication.Files;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LifeBackup.Core.Communication.Interfaces
{
    public interface IFilesRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="formFiles"></param>
        /// <returns></returns>
        Task<AddFileResponse> UploadFiles(string bucketName, IList<IFormFile> formFiles);



        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        Task<IEnumerable<ListFilesResponse>> ListFiles(string bucketName);



        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task DownloadFile(string bucketName, string fileName);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<DeleteFileResponse> DeleteFile(string bucketName, string fileName);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="buckectName"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task AddJsonObject(string buckectName, AddJsonObjectRequest request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<GetJsonObjectResponse> GetJsonObject(string bucketName, string fileName);
    }
}
