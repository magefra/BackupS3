﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LifeBackup.Core.Communication.Files;
using LifeBackup.Core.Communication.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LifeBackup.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFilesRepository _filesRepository;


        public FilesController(IFilesRepository filesRepository)
        {
            _filesRepository = filesRepository;
        }


        [HttpPost]
        [Route("{bucketName}/Add")]
        public async Task<ActionResult<AddFileResponse>> AddFiles(string bucketName, IList<IFormFile> formFiles)
        {

            if(formFiles == null)
            {
                return BadRequest("The request doesn´t contain any files to be uploaded.");
            }


            var response = await _filesRepository.UploadFiles(bucketName, formFiles);


            if(response == null)
            {
                return BadRequest();
            }

            return Ok(response);
        }



        [HttpGet()]
        [Route("{bucketName}/list")]
        public async Task<ActionResult<IEnumerable<ListFilesResponse>>> ListFiles(string bucketName)
        {
            var response = await _filesRepository.ListFiles(bucketName);


            return Ok(response);
        }



        [HttpGet()]
        [Route("{bucketName}/download/{fileName}")]
        public async Task<IActionResult> DownloadFile(string bucketName, string fileName)
        {
            await _filesRepository.DownloadFile(bucketName, fileName);

            return Ok();
        }



        [HttpDelete]
        [Route("{bucketName}/delete/{fileName}")]
        public async Task<ActionResult<DeleteFileResponse>> DeleteFile(string bucketName, string fileName)
        {
            var response = await _filesRepository.DeleteFile(bucketName, fileName);

            return Ok(response);

        }


        [HttpPost]
        [Route("{bucketName}/addjsonobject")]
        public async Task<IActionResult> AddJsonObject(string bucketName, AddJsonObjectRequest request)
        {
            await _filesRepository.AddJsonObject(bucketName, request);

            return Ok();
        }

        [HttpGet]
        [Route("{bucketName}/getjsonobject")]
        public async Task<ActionResult<GetJsonObjectResponse>> GetJsonObject(string bucketName, string fileName)
        {
            var response = await _filesRepository.GetJsonObject(bucketName, fileName);


            return Ok(response);
        }

    }
}