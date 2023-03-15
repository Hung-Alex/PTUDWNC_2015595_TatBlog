using System.Runtime.CompilerServices;
using TatBlog.WebApp.Areas.Admin.Controllers;

namespace TatBlog.WebApp.Extensions
{
    public static class RouteExtensions
    {
        //dinh nghia route template route constraint cho cac
        //endpoint ket hop voi cac action trong cac controller
        public static IEndpointRouteBuilder UseBlogRoutes(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapControllerRoute(
                name: "posts-by-category",
                pattern: "blog/category/{slug}",
               defaults: new { controller = "Blog", action = "Category" });

            endpoints.MapControllerRoute(
              name: "posts-by-tag",
              pattern: "blog/tag/{slug}",
              defaults: new { controller = "Blog", action = "Tag" });

            endpoints.MapControllerRoute(
              name: "blog-post-author",
              pattern: "blog/author/{slug}",
              defaults: new { controller = "Blog", action = "Author" });

            endpoints.MapControllerRoute(
             name: "single-post",
             pattern: "blog/post/{year:int}/{month:int}/{day:int}/{slug}",
             defaults: new { controller = "Blog", action = "Post" });

            //-------------------------

            endpoints.MapControllerRoute(
              name: "default",
            pattern: "{controller=Blog}/{action=Index}/{id?}");

            //--------Admin-----
            endpoints.MapControllerRoute(
                 name: "admin-area",
                 pattern: "admin/{controller=Dashboard}/{action=Index}/{id?}",
                 defaults: new { area = "Admin" }
                 );



            return endpoints;
        }
    }
}
