

using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using Amazon.S3;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Net;

namespace LifeBackup.Integration.Test.Scenarios
{
    [Collection("api")]
    public class FilesControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {

        private readonly HttpClient _client;

        public FilesControllerTest(WebApplicationFactory<Startup> factory)
        {

            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
               {
                   services.AddAWSService<IAmazonS3>(new AWSOptions
                   {
                       DefaultClientConfig =
                       {
                           ServiceURL = "http://localhost:9093"
                       },
                       Credentials = new BasicAWSCredentials("FAKE", "FAKE")
                   }); ;
               });
            }).CreateClient();


            Task.Run(CreateBucket).Wait();
        }

        private async Task CreateBucket()
        {
            await _client.PostAsJsonAsync("api/bucket/create/testS3Bucket", "testS3Bucket");
        }


        [Fact]
        public async Task When_AddFiles_endPoint_is_it_hit_we_are_returned_ok_status()
        {
            var response = await UploadFileToS3Bucket();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }



        public async Task<HttpResponseMessage> UploadFileToS3Bucket()
        {
            const string path = @"C:\Users\magef\Desktop\xml\2015\1\1\A-2114\0896a267-d2be-a342-8d8b-c539cea2ed18.xml";

            var file = File.Create(path);

            HttpContent fileStreamContent = new StreamContent(file);


            var formData = new MultipartFormDataContent
            {
                { fileStreamContent, "formFiles", "0896a267-d2be-a342-8d8b-c539cea2ed18.xml" }
            };

            var response = await _client.PostAsync("api/files/testS3Bucket/add", formData);

            fileStreamContent.Dispose();
            formData.Dispose();

            return response;

        }
    }
}
