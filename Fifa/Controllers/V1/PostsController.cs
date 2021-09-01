using Fifa.Contracts;
using Fifa.Contracts.Requests;
using Fifa.Contracts.Responses;
using Fifa.Domain;
using Fifa.Extensions;
using Fifa.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Fifa.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostsController : Controller
    {

        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet(ApiRoutes.Posts.Getall)]
        //[Authorize(Roles ="Admin")]
        [Authorize(Policy = "MustWorkFor")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _postService.GetPostsAsync());
        }


        [HttpGet(ApiRoutes.Posts.Get)]
        public async Task<IActionResult> GetPostByIDAsync([FromRoute] Guid postId)
        {
            Post p = await _postService.GetPostAsync(postId);
            if (p == null)
            {
                return NotFound();
            }
            PostReponse response = new PostReponse
            {
                Id = p.Id,
                Name = p.Name
            };
            return Ok(response);
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public async Task<IActionResult> CreateAsync([FromBody] CreatePostRequest postRequest)
        {
            var post = new Post
            {
                Name = postRequest.Name,
                UserID = HttpContext.GetUserID()
            };

            await _postService.CreatePostAsync(post);

           var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
           var locationUrl = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("postId", post.Id.ToString());

            PostReponse response = new PostReponse
            {
                Id = post.Id,
                Name = post.Name
            };

            return Created(locationUrl, response);
        }

        [HttpPut(ApiRoutes.Posts.Update)]
        public async Task<IActionResult> UpdatePostAsync([FromRoute] Guid postId, [FromBody] UpdatePostRequest request)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserID());
            if(!userOwnsPost)
            {
                return BadRequest(new { error = "you donot own this post" });
            }
            var post = new Post
            {
                Id = postId,
                Name = request.Name
            };
            if (await _postService.EditPostAsync(post))
                return Ok(_postService.GetPostsAsync());
            else
                return NotFound();
        }


        [HttpDelete(ApiRoutes.Posts.Delete)]
        [Authorize(Policy = "PostDeleter")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid postId)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserID());
            if (!userOwnsPost)
            {
                return BadRequest(new { error = "you donot own this post" });
            }
            if (await _postService.DeletePostAsync(postId))
                return Ok(_postService.GetPostsAsync());
            else
                return NotFound();
        }

    }
}
