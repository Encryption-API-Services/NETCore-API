using API.ControllerLogic;
using Microsoft.AspNetCore.Mvc;
using Models.Blog;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogPostControllerLogic _blogPostControllerLogic;
        public BlogController(IBlogPostControllerLogic blogPostControllerLogic)
        {
            this._blogPostControllerLogic = blogPostControllerLogic;
        }

        [HttpPost]
        [Route("CreatePost")]
        public async Task<IActionResult> CreatePost([FromBody] CreateBlogPost body)
        {
            return await this._blogPostControllerLogic.CreatePost(body, HttpContext);
        }

        [HttpGet]
        [Route("GetBlogPosts")]
        public async Task<IActionResult> GetBlogPosts()
        {
            return await this._blogPostControllerLogic.GetBlogPosts(HttpContext);
        }

        [HttpGet]
        [Route("GetPost/{title}")]
        public async Task<IActionResult> GetPost([FromRoute]string title)
        {
            return await this._blogPostControllerLogic.GetPost(HttpContext, title);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetPostById([FromRoute]string id)
        {
            return await this._blogPostControllerLogic.GetPostById(HttpContext, id);
        }

        [HttpPut]
        [Route("UpdatePost")]
        public async Task<IActionResult> UpdatePost([FromBody]UpdateBlogPost body)
        {
            return await this._blogPostControllerLogic.UpdatePost(HttpContext, body);
        }

        [HttpPost]
        [Route("DeletePost")]
        public async Task<IActionResult> DeletePost([FromBody]DeleteBlogPost body)
        {
            return await this._blogPostControllerLogic.DeletePost(HttpContext, body);
        }

        [HttpPost]
        [Route("Newsletter")]
        public async Task<IActionResult> AddEmailToNewsletter([FromBody]AddEmailToNewsletter body)
        {
            return await this._blogPostControllerLogic.AddEmailToNewsletter(HttpContext, body);
        }
    }
}