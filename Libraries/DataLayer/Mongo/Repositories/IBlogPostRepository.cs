using DataLayer.Mongo.Entities;
using System.Threading.Tasks;

namespace DataLayer.Mongo.Repositories
{
    public interface IBlogPostRepository
    {
        public Task InsertBlogPost(BlogPost post);
    }
}
