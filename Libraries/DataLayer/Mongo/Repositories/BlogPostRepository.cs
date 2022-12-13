using DataLayer.Mongo.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Mongo.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly IMongoCollection<BlogPost> _blogPosts;
        public BlogPostRepository(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.Connection);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            this._blogPosts = database.GetCollection<BlogPost>("BlogPosts");
        }

        public async Task InsertBlogPost(BlogPost post)
        {
            await this._blogPosts.InsertOneAsync(post);
        }
        public async Task<List<BlogPost>> GetHomeBlogPosts()
        {
            return await this._blogPosts.AsQueryable().OrderByDescending(x => x.CreateDate).ToListAsync();
        }
    }
}
