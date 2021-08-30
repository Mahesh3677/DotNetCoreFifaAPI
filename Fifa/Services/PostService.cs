using Fifa.Data;
using Fifa.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifa.Services
{
    public class PostService : IPostService
    {
        private readonly DataContext _dataContext;

        public PostService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Post> GetPostAsync(Guid postID)
        {
            return await _dataContext.Posts.SingleOrDefaultAsync(x => x.Id == postID);
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            return await _dataContext.Posts.ToListAsync();
        }

        public async Task<bool> CreatePostAsync(Post post)
        {
            await _dataContext.Posts.AddAsync(post);
            var updated = await _dataContext.SaveChangesAsync();
            return updated > 0;

        }

        public async Task<bool> EditPostAsync(Post post)
        {
            _dataContext.Posts.Update(post);
            var updated = await _dataContext.SaveChangesAsync();
            return updated > 0;

        }

        public async Task<bool> DeletePostAsync(Guid postID)
        {
            Post post = await GetPostAsync(postID);
            if(post == null)
            return false;

            _dataContext.Posts.Remove(post);
            var updated = await _dataContext.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<bool> UserOwnsPostAsync(Guid postId, string userId)
        {
            var post = await _dataContext.Posts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == postId);
            if(post==null)
            {
                return false;
            }
            if(post.UserID !=userId)
            {
                return false;
            }
            return true;
        }
    }
}
