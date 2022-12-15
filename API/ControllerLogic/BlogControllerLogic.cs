using Common;
using DataLayer.Mongo.Entities;
using DataLayer.Mongo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models.Blog;
using Twilio.TwiML.Messaging;

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

        #region DeletePost
        public async Task<IActionResult> DeletePost(HttpContext httpContext, DeleteBlogPost body)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(httpContext);
            IActionResult result = null;
            try
            {
                if (!string.IsNullOrEmpty(body.id))
                {
                    await this._blogPostRepository.DeleteBlogPost(body.id);
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

        #region GetBlogPosts
        public async Task<IActionResult> GetBlogPosts(HttpContext httpContext)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(httpContext);
            IActionResult result = null;
            try
            {
                List<BlogPost> blogPosts = await this._blogPostRepository.GetHomeBlogPosts();
                return new OkObjectResult(new { posts = blogPosts });
            }
            catch (Exception ex)
            {
                result = new BadRequestObjectResult(new { error = "Something went wrong on our end getting blog posts" });
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }
        #endregion

        #region GetPost
        public async Task<IActionResult> GetPost(HttpContext httpContext, string blogPostTitle)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(httpContext);
            IActionResult result = null;
            try
            {
                if (!string.IsNullOrEmpty(blogPostTitle))
                {
                    blogPostTitle = blogPostTitle.Replace("-", " ");
                    BlogPost blogPost = await this._blogPostRepository.GetBlogPostByTitle(blogPostTitle);
                    if (blogPost != null)
                    {
                        result = new OkObjectResult(new { post = blogPost });
                    }
                    else
                    {
                        result = new BadRequestObjectResult(new { error = "There is no blog post with that title." });
                    }
                }
            }
            catch (Exception ex)
            {
                result = new BadRequestObjectResult(new { error = "Something went wrong on our end getting the post" });
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }
        #endregion

        #region GetPostById
        public async Task<IActionResult> GetPostById(HttpContext httpContext, string id)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(httpContext);
            IActionResult result = null;
            try
            {
                BlogPost post = await this._blogPostRepository.GetBlogPostById(id);
                result = new OkObjectResult(new { post = post });
            }
            catch (Exception ex)
            {
                result = new BadRequestObjectResult(new { error = "Something went wrong on our end getting the post" });
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }
        #endregion

        #region UpdatePost
        public async Task<IActionResult> UpdatePost(HttpContext httpContext, UpdateBlogPost body)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(httpContext);
            IActionResult result = null;
            try
            {
                BlogPost post = await this._blogPostRepository.GetBlogPostById(body.BlogId);
                if (post != null)
                {
                    await this._blogPostRepository.UpdateBlogPost(body);
                    result = new OkObjectResult(new { message = "" });
                }
                else
                {
                    result = new BadRequestObjectResult(new { error = "We were unable to find that post" });
                }
            }
            catch (Exception ex)
            {
                result = new BadRequestObjectResult(new { error = "Something went wrong on our updating the post" });
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }
        #endregion
    }
}
