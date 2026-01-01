using Microsoft.AspNetCore.Mvc;
using stepik.Models;
using stepik.Services;

namespace stepik_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly CommentsService _commentsService;

        public CommentsController()
        {
            _commentsService = new CommentsService();
        }

        [HttpGet("GetCourseComments")]
        public IActionResult GetCourseComments(int id)
        {
            List<Comment> comments = _commentsService.Get(id);
            return (comments != null && comments.Any()) ? Ok(comments) : NotFound("Комментариев не найдено");
        }

        [HttpDelete("DeleteComment")]
        public IActionResult DeleteComment(int id)
        {
            var result = _commentsService.Delete(id);
            return result ? Ok("Комментарий удален") : BadRequest("Не удалось удалить комментарий.");
        }
    }
}
