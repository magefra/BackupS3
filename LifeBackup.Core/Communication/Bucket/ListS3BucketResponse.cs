using System;
using System.Collections.Generic;
using System.Text;

namespace LifeBackup.Core.Communication.Bucket
{
    public class ListS3BucketResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public string BucktName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreationDate { get; set; }
    }
}
