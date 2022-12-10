using Common;
using DataLayer.Mongo.Entities;
using DataLayer.Mongo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Blog;

namespace API.ControllerLogic
{
    public class BlogControllerLogic : IBlogPostControllerLogic
    {
        private readonly IMethodBenchmarkRepository _methodBenchmarkRepository;
        private readonly IBlogPostRepository _blogPostRepository;
        public BlogControllerLogic(
            IMethodBenchmarkRepository methodBenchmarkRepository,
            IBlogPostRepository blogPostRepository)
        {
            this._methodBenchmarkRepository = methodBenchmarkRepository;
            this._blogPostRepository = blogPostRepository;
        }
        #region CreatePost
        public async Task<IActionResult> CreatePost(CreateBlogPost body, HttpContext httpContext)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(httpContext);
            IActionResult result = null;
            try
            {
                if (!string.IsNullOrEmpty(body.BlogTitle) && !string.IsNullOrEmpty(body.BlogBody))
                {
                    BlogPost newBlogPost = new BlogPost()
                    {
                        BlogTitle = body.BlogTitle,
                        BlogBody = body.BlogBody,
                        CreateDate = DateTime.UtcNow,
                        ModifiedDate = DateTime.UtcNow,
                        CreatedBy = httpContext.Items["UserID"].ToString()
                    };
                    await this._blogPostRepository.InsertBlogPost(newBlogPost);
                    result = new OkObjectResult(new { message = "You have create a new blog post" });
                }
                else
                {
                    result = new BadRequestObjectResult(new { error = "You need to enter a blog title and post" });
                }
            }
            catch (Exception ex)
            {
                result = new BadRequestObjectResult(new { error = "Something went wrong on our end." });
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }
        #endregion
    }
}
