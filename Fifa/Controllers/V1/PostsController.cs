﻿using Fifa.Contracts;
using Fifa.Contracts.Requests;
using Fifa.Contracts.Responses;
using Fifa.Domain;
using Fifa.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifa.Controllers.V1
{
    public class PostsController : Controller
    {

        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet(ApiRoutes.Posts.Getall)]
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
                Name = postRequest.Name
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
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid postId)
        {
            if (await _postService.DeletePostAsync(postId))
                return Ok(_postService.GetPostsAsync());
            else
                return NotFound();
        }

    }
}