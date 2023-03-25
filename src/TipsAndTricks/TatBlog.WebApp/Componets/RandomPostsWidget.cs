using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Componets
{
    public class RandomPostsWidget:ViewComponent
    {
        private readonly IBlogRepository _blogRepository;
        public RandomPostsWidget(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var posts = await _blogRepository.GetPostRandomsAsync(5);
            return View(posts);
        }
    }
}
