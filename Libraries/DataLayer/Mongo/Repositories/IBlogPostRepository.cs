using DataLayer.Mongo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Mongo.Repositories
{
    public interface IBlogPostRepository
    {
        public Task InsertBlogPost(BlogPost post);
        public Task<List<BlogPost>> GetHomeBlogPosts();
    }
}
