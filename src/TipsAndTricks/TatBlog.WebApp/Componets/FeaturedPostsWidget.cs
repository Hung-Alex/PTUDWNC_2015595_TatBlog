using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Componets
{
    public class FeaturedPostsWidget:ViewComponent
    {
        private readonly IBlogRepository _blogRepository;

        public FeaturedPostsWidget(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var posts = await _blogRepository.GetFeaturedPostToTakeNumber(3);
            return View(posts);
        }
    }
}
