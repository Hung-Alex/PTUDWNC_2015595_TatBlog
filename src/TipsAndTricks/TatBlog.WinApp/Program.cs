using Azure;
using Microsoft.VisualBasic;
using System;
using System.Linq;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;

namespace TatBlog.WinApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var context = new BlogDbContext();
            var seeder = new DataSeeder(context);
            seeder.Initialize();
            IBlogRepository blogRepo = new BlogRepository(context);


            var tagitems =  await blogRepo.GetAllTagssAsync();
            Console.WriteLine(await blogRepo.RemoveTagById(1));
            var category = await blogRepo.FindCategoryByUrlSlug(".net-core");
            Console.WriteLine("{0}{1}{2}", category.Id, category.Name, category.Description);
            //foreach (var tagitem in tagitems)
            //{
            //    Console.WriteLine("ID :{0}", tagitem.Id);
            //    Console.WriteLine("Name :{0}", tagitem.Name);
            //    Console.WriteLine("UrlSlug :{0}", tagitem.UrlSlug);
            //    Console.WriteLine("Description :{0}", tagitem.Description);
            //    Console.WriteLine("PostCount :{0}", tagitem.PostCount);
                
            //    Console.WriteLine("".PadRight(80, '-'));
            //}


                //var tag = await blogRepo.FindTagItemByUrlSlugAsync("google-application");
                //Console.WriteLine(tag.Name);


                //var authors = context.Authors.ToList();
                //Console.WriteLine("{0,-4}{1,-30}{2,-30}{3,12}", "ID", "Full Name", "Email", "Joined Date");
                //foreach (var author in authors)
                //{
                //    Console.WriteLine("{0,-4}{1,-30}{2,-30}{3,12:MM/dd/yyyy}", author.Id, author.FullName, author.Email, author.JoinedDate);
                //}
                //var posts = context.Posts.Where(p => p.Published).OrderBy(p => p.Title).Select(p => new
                //{
                //    Id = p.Id,
                //    Title = p.Title,
                //    ViewCount = p.ViewCount,
                //    PostedDate = p.PostedDate,
                //    Author = p.Author.FullName,
                //    Category = p.Category.Name,
                //}).ToList();
                //foreach (var post in posts)
                //{
                //    Console.WriteLine("ID :{0}", post.Id);
                //    Console.WriteLine("Title :{0}", post.Title);
                //    Console.WriteLine("View :{0}", post.ViewCount);
                //    Console.WriteLine("Date :{0}:MM/dd/yyyy", post.PostedDate);
                //    Console.WriteLine("Author :{0}", post.Author);
                //    Console.WriteLine("Category :{0}", post.Category);
                //    Console.WriteLine("".PadRight(80, '-'));

                //}
                //IBlogRepository blogRepo = new BlogRepository(context);
                //Console.WriteLine("=================================================================");
                //var postss = await blogRepo.GetPopularArticleAsync(3);
                //foreach (var post in postss)
                //{
                //    Console.WriteLine("ID :{0}", post.Id);
                //    Console.WriteLine("Title :{0}", post.Title);
                //    Console.WriteLine("View :{0}", post.ViewCount);
                //    Console.WriteLine("Date :{0}:MM/dd/yyyy", post.PostedDate);
                //    Console.WriteLine("Author :{0}", post.Author.FullName);
                //    Console.WriteLine("Category :{0}", post.Category.Name);
                //    Console.WriteLine("".PadRight(80, '-'));
                //}
                //var categories = await blogRepo.GetCategoriesAsync();
                //Console.WriteLine("{0,-5}{1,-50}{2,10}", "ID", "Name", "Count");
                //foreach (var item in categories)
                //{
                //    Console.WriteLine("{0,-5}{1,-50}{2,10}", item.Id, item.Name, item.PostCount);
                //}

                //var paginParams = new PagingParams()
                //{
                //    PageNumber = 1,
                //    PageSize = 5,
                //    SortColumn = "Name",
                //    SortOrder = "DESC"
                //};
                //var tagsList = await blogRepo.GetPagedTagsAsync(paginParams);
                //Console.WriteLine("{0,-5}{1,-50}{2,10}", "ID", "Name", "Count");
                //foreach (var item in tagsList)
                //{
                //    Console.WriteLine("{0,-5}{1,-50}{2,10}", item.Id, item.Name, item.PostCount);
                //}

                Console.ReadKey();
        }

    }
}
