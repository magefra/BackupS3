using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LifeBackup.Core.Communication.Bucket;
using LifeBackup.Core.Communication.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LifeBackup.Controllers
{
    [Route("api/bucket")]
    [ApiController]
    public class BucketController : ControllerBase
    {

        private readonly IBucketRepository _bucketRepository;


        public BucketController(IBucketRepository bucketRepository)
        {
            _bucketRepository = bucketRepository;
        }

        [HttpPost]
        [Route("create/{bucketName}")]
        public async Task<ActionResult<CreateBucketResponse>> CreaateS3Bucket([FromRoute] string bucketName)
        {
            var bucketExist = await _bucketRepository.DoesS3BucketExist(bucketName);


            if (bucketExist)
            {
                return BadRequest("S3 bucket already exists");
            }

            var result = await _bucketRepository.CreateBucket(bucketName);


            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);


        }



        [HttpGet]
        [Route("List")]
        public async Task<ActionResult<IEnumerable<ListS3BucketResponse>>> ListS3Buckest()
        {
            var result = await _bucketRepository.ListBuckets();


            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }



        [HttpDelete]
        [Route("delete/{bucketName}")]
        public async Task<IActionResult> DeleteS3Bucket(string bucketName)
        {
            await _bucketRepository.DeleteBucket(bucketName);

            return Ok();
        }



    }
}