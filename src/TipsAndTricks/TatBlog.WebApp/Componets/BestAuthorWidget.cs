using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;
using TatBlog.Services.Blogs.Authors;

namespace TatBlog.WebApp.Componets
{
    public class BestAuthorWidget:ViewComponent
    {
        private readonly IAuthorRepository _authorRepository;
        public BestAuthorWidget(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var author = await _authorRepository.GetFourPopulationAuthor();
            return View(author);
        }
    }
}
