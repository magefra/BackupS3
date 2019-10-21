using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xunit;

namespace LifeBackup.Integration.Test.Setup
{
    public class TestContext : IAsyncLifetime
    {

        private readonly DockerClient _dockerClient;
        private string _containerID;
        private const string ContainerImagerUri = "localstack/localstack";


        public TestContext()
        {
            _dockerClient = new DockerClientConfiguration(new Uri(DockerApiUri())).CreateClient();
        }

        public async Task InitializeAsync()
        {
            await PullImage();
            await StartContaniner();
        }

        private async Task PullImage()
        {
            await _dockerClient.Images
                .CreateImageAsync(new ImagesCreateParameters
                {
                    FromImage= ContainerImagerUri,
                    Tag = "latest"
                },
                new AuthConfig(),
                new Progress<JSONMessage>()
                
                );
        }

        private async Task StartContaniner()
        {
            var response = await _dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters
            {
                Image = ContainerImagerUri,
                ExposedPorts =  new Dictionary<string, EmptyStruct>
                {
                    {
                        "9003", default
                    }
                },
                HostConfig = new HostConfig
                {
                    PortBindings= new Dictionary<string, IList<PortBinding>>
                    {
                        {"9003", new List<PortBinding>{ new PortBinding { HostPort = "9003"} } }
                    }
                },
                Env = new List<string>
                {
                    "SERVICES=s3:9003"
                }
            });
            _containerID = response.ID;


            await _dockerClient.Containers.StartContainerAsync(_containerID, null);
        }


        private string DockerApiUri()
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            if (isWindows)
            {
                return "unix:/var/run/docker.sock";
            }

            var isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

            if (isLinux)
            {
                return "unix:/var/run/docker.sock";
            }

            throw new System.Exception("Unable to determine what OS this this running on");

        }


       

        public async Task DisposeAsync()
        {
            if(_containerID != null)
            {
                await _dockerClient.Containers.KillContainerAsync(_containerID,new ContainerKillParameters());
            }
        }



    }
}