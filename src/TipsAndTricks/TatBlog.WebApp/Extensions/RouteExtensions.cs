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
             name: "blog-Month-post",
             pattern: "blog/Month/{month}/{year}",
             defaults: new { controller = "Blog", action = "Month" });

            endpoints.MapControllerRoute(
             name: "single-post",
             pattern: "blog/post/{year:int}/{month:int}/{day:int}/{slug}",
             defaults: new { controller = "Blog", action = "Post" });
            endpoints.MapControllerRoute(
             name: "newsletter-Unscribe-email",
             pattern: "Newsletter/Unsubscribe/{email}",
             defaults: new { controller = "Newsletter", action = "Newsletter" });

            //--------Admin-----
            endpoints.MapControllerRoute(
                 name: "admin-area",
                 pattern: "admin/{controller=Dashboard}/{action=Index}/{id?}",
                 defaults: new { area = "Admin" }
                 );

            //-------------------------

            endpoints.MapControllerRoute(
              name: "default",
            pattern: "{controller=Blog}/{action=Index}/{id?}");



            return endpoints;
        }
    }
}
