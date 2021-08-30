using Fifa.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifa.Services
{
    public interface IPostService
    {
        Task<List<Post>> GetPostsAsync();

        Task<Post> GetPostAsync(Guid postID);

        Task<bool> CreatePostAsync(Post post);

        Task<bool> EditPostAsync(Post post);

        Task<bool> DeletePostAsync(Guid postID);
        Task<bool>UserOwnsPostAsync(Guid postId, string v);
    }
}
