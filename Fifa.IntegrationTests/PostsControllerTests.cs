using Fifa.Contracts;
using Fifa.Contracts.Requests;
using Fifa.Domain;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Fifa.IntegrationTests
{
    public class PostsControllerTests : IntegrationTest
    {
        [Fact]
        public async Task GetAll()
        {
            //arrange
            await AuthenticateAsync();

            //act
            var response = await client.GetAsync(ApiRoutes.Posts.Getall);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<List<Post>>()).Should().BeEmpty();
        }

        [Fact]
        public async Task Get()
        {
            //arrange
            await AuthenticateAsync();
            var createdPost = await CreatePostAsync(new CreatePostRequest { Name = "Tesst Poist" });

            //act
            var response = await client.GetAsync(ApiRoutes.Posts.Get.Replace("{postId}", createdPost.Id.ToString()));

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnpost = await response.Content.ReadAsAsync<Post>();
            returnpost.Id.Should().Be(createdPost.Id);
            returnpost.Name.Should().Be("Tesst Poist");
        }
    }
}
