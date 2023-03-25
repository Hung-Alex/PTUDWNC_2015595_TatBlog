using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Componets
{
    public class TagCloudWidget:ViewComponent
    {
        private readonly IBlogRepository _blogRepository;

        public TagCloudWidget(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var tagList= await _blogRepository.GetAllTagAsync();
            return View(tagList);
        }
    }
}
