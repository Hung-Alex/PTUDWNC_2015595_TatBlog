using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System;
using TatBlog.Services.Blogs;
using TatBlog.Core.Contracts;
using TatBlog.Core.Entities;
using TatBlog.Core.DTO;

namespace TatBlog.WebApp.Controllers
{
    public class BlogController : Controller
    {
        
        private readonly IBlogRepository _blogRepository;
        public BlogController( IBlogRepository blogRepository) 
        { 
            
            _blogRepository = blogRepository;
        } 
        public async Task<IActionResult> Index([FromQuery(Name ="k")]string keyword=null,
                                                [FromQuery(Name = "p")] int pageNumber=1,
                                                [FromQuery(Name = "ps")] int pageSize =10)
        {
            //tao doi tuong cha cac dieu kien truy van
            var postQuery = new PostQuey()
            {
                //chi layu nhung bai viet co trang thai published
                PublishedOnly=true,

                //tim bai viet theo tu khoa
                Keyword=keyword,
            };
            ViewBag.PostQuery = postQuery;
            var postsList = await _blogRepository.GetPagedPostsAsync(postQuery,pageNumber,pageSize);
            
            return View("Index",postsList);
        }
        public IActionResult About()
        {

            return View();
        }
        public IActionResult Contact()=>View();
        public IActionResult Rss() => Content("Nội dung sẽ được cập nhật");
    }
}
