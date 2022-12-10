using API.ControllerLogic;
using Microsoft.AspNetCore.Mvc;
using Models.Blog;

namespace API.Controllers
{
    [Route("api/[controller]")]
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
    }
}
