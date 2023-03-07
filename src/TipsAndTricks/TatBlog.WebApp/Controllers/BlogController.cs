using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System;
using TatBlog.Services.Blogs;
using TatBlog.Core.Contracts;
using TatBlog.Core.Entities;

namespace TatBlog.WebApp.Controllers
{
    public class BlogController : Controller
    {
        
        private readonly IBlogRepository _blogRepository;
        public BlogController( IBlogRepository blogRepository) 
        { 
            
            _blogRepository = blogRepository;
        } 
        public async Task<IActionResult> Index()
        {
            IPagedList<Post> posts = await _blogRepository.GetPagedPostsAsync(new Core.DTO.PostQuey() { PublishedOnly=true});
            
            return View("Index",posts);
        }
        public IActionResult About()
        {

            return View();
        }
        public IActionResult Contact()=>View();
        public IActionResult Rss() => Content("Nội dung sẽ được cập nhật");
    }
}
