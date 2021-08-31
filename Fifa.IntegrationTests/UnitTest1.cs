using Fifa.Contracts;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Fifa.IntegrationTests
{
    public class UnitTest1
    {
        private readonly HttpClient client;

        public UnitTest1()
        {
            var appFactory = new WebApplicationFactory<Startup>();
            client = appFactory.CreateClient();
        }

        [Fact]
        public async Task Test1()
        {
          var response = await client.GetAsync(ApiRoutes.Posts.Get.Replace("{postId}", "1"));
        }
    }
}
