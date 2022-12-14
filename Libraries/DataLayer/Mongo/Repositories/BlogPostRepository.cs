using DataLayer.Mongo.Entities;
using Models.Blog;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
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
        public async Task<BlogPost> GetBlogPostByTitle(string blogTitle)
        {
            return await this._blogPosts.Find(x => x.BlogTitle == blogTitle).FirstOrDefaultAsync();
        }
        public async Task<BlogPost> GetBlogPostById(string id)
        {
            return await this._blogPosts.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateBlogPost(UpdateBlogPost body)
        {
            var filter = Builders<BlogPost>.Filter.Eq(x => x.Id, body.BlogId);
            var update = Builders<BlogPost>.Update
                .Set(x => x.BlogTitle, body.BlogTitle)
                .Set(x => x.BlogBody, body.BlogBody)
                .Set(x => x.ModifiedDate, DateTime.UtcNow);
            await this._blogPosts.UpdateOneAsync(filter, update);
        }
    }
}