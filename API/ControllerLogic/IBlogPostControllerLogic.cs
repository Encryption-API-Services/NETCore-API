using Microsoft.AspNetCore.Mvc;
using Models.Blog;

namespace API.ControllerLogic
{
    public interface IBlogPostControllerLogic
    {
        Task<IActionResult> CreatePost(CreateBlogPost body, HttpContext httpContext);
    }
}
