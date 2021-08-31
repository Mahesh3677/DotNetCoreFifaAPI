using Fifa.Contracts;
using Fifa.Contracts.Requests;
using Fifa.Contracts.Responses;
using Fifa.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;
namespace Fifa.IntegrationTests
{
    public class IntegrationTest
    {
        protected readonly HttpClient client;

        public IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder=> 
                {
                    builder.ConfigureServices
                    (services =>
                    {
                        services.RemoveAll(typeof(DataContext));
                        services.AddDbContext<DataContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestDb");
                        });
                    });
                });
            client = appFactory.CreateClient();
        }

        protected async Task AuthenticateAsync()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync());
        }

        private async Task<string> GetJwtAsync()
        {
            var response = await client.PostAsJsonAsync(ApiRoutes.Identity.Register, new UserRegistrationRequest
            {
                Email = "test@int.com",
                Password = "SpmePass1234!"
            });

            var regResponse = await response.Content.ReadAsAsync<AuthSuccessResponse>();
            return regResponse.Token;
        }

        protected async Task<PostReponse> CreatePostAsync(CreatePostRequest request)
        {
            var response = await client.PostAsJsonAsync(ApiRoutes.Posts.Create, request);
            return await response.Content.ReadAsAsync<PostReponse>();
        }
    }
}
