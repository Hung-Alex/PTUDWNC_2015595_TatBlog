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
                UrlSlug = "food-and-daily",

                //tim bai viet theo tu khoa
                Keyword=keyword,
            };
            ViewBag.PostQuery = postQuery;
            var postsList = await _blogRepository.GetPagedPostsAsync(postQuery,pageNumber,pageSize);
            
            return View("Index",postsList);
        }
        public async Task<IActionResult> Category([FromRoute(Name = "slug")] string slugCate=null)
        {
            var postQuery = new PostQuey()
            {
                CategorySlug = slugCate,
                
            };
            ViewBag.PostQuery = postQuery;
            var postsList = await _blogRepository.GetPagedPostsAsync(postQuery);

            return View("Index", postsList);
        }
        public async Task<IActionResult> Author([FromRoute(Name = "slug")] string slug = null)
        {
            var postQuery = new PostQuey()
            {

                AuthorSlug = slug,

            };
            ViewBag.PostQuery = postQuery;
            var postsList = await _blogRepository.GetPagedPostsAsync(postQuery);

            return View("Index", postsList);
        }
        public async Task <IActionResult> Tag([FromRoute(Name ="slug")]string slug=null)
        {
            var postQuery = new PostQuey()
            {

                TagSlug = slug,

            };
            ViewBag.PostQuery = postQuery;
            var postsList = await _blogRepository.GetPagedPostsAsync(postQuery);

            return View("Index", postsList);
           
        }
        public async Task<IActionResult> Post([FromRoute(Name = "year")] int year =2022,
                                                [FromRoute(Name = "month")] int month = 9,
                                                [FromRoute(Name = "day")] int day = 5,
                                                [FromRoute(Name = "slug")] string slug = null
                                                    )
        {
            var postQuery = new PostQuey()
            {
                Month=month,
                Year=year,
                Day=day,
                UrlSlug=slug,

            };
            ViewBag.PostQuery = postQuery;
            var posts = await _blogRepository.GetPostAsync(postQuery);
            try
            {
                if (posts!=null &&!posts.Published)
                {
                    await _blogRepository.IncreaseViewCountAsync(posts.Id);
                }
            }
            catch (NullReferenceException)
            {

                return View("Error");
            }
           
           
            

            return View("DetailPost", posts);

        }
        public async Task<IActionResult> Archives([FromRoute(Name ="year")] int year=2021,
                                                    [FromRoute(Name ="month")]int month = 9)
        {
            var postQuery = new PostQuey()
            {
                Year=year,
                Month=month
            };
            var posts = _blogRepository.GetPagedPostsAsync(postQuery);
            return View();
        }
        public  IActionResult About()
        {
            return View("About");
        }
        public IActionResult Contact()=>View();
        public IActionResult Rss() => Content("Nội dung sẽ được cập nhật");
    }
}
