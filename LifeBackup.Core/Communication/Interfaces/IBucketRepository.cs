using LifeBackup.Core.Communication.Bucket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LifeBackup.Core.Communication.Interfaces
{
    public interface IBucketRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        Task<bool> DoesS3BucketExist(string bucketName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        Task<CreateBucketResponse> CreateBucket(string bucketName);


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ListS3BucketResponse>> ListBuckets();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        Task DeleteBucket(string bucketName);

    }
}
