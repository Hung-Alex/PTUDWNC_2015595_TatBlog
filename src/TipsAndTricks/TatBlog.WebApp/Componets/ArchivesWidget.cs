using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Componets
{
    public class ArchivesWidget : ViewComponent
    {
        private readonly IBlogRepository _blogRepository;
        public ArchivesWidget(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var month = await _blogRepository.GetAllMonthOfPosts();
            return View(month);
        }
    }
}
