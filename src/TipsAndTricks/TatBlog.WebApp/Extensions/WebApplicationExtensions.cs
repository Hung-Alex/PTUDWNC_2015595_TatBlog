using Azure.Core;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Middlewares;

namespace TatBlog.WebApp.Extensions
{
    public static class WebApplicationExtensions
    {
        // them cac dich vu duoc yeu cau boi mvc framework
        public static WebApplicationBuilder ConfigureMvc(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllersWithViews();
            builder.Services.AddResponseCompression();
            return builder;
        }
        //dang ki cac dich vu voi DI Container
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<BlogDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IBlogRepository, BlogRepository>();
            builder.Services.AddScoped<IDataSeeder, DataSeeder>();
            builder.Services.AddScoped<IMediaManager,LocalFileSystemMediaManager>();

            return builder;
        }
        // Cấu hình việc sử dụng NLog
        public static WebApplicationBuilder ConfigureNLog(this WebApplicationBuilder builder)
        {
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            return builder;
        }
        //cau hinh http request pieline
        public static WebApplication UseRequestPieline(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Blog/Error");
                //them middware cho viec ap dung hsts (Them header)
                //strict-transport-sercurity vao http response
                app.UseHsts();
            }
            //them middware de chuyen huong http sang https
            app.UseHttpsRedirection();
            //them cac middware phuc vu cac yeu cac lien quan
            //toi cac tap tin noi tinh nhu hinh anh ,css
            app.UseStaticFiles();
            //them middware de lua chon enpoint phu hop nhat
            //de xu ly 1 http request
            app.UseRouting();
            app.UseMiddleware<UserActivityMiddleware>();
            return app;
        }

        //them du lieu mau vao CSDL
        public static IApplicationBuilder UseDataSeeder(this IApplicationBuilder app) 
        {
           using var scope=app.ApplicationServices.CreateScope();
            try
            {
                scope.ServiceProvider
                    .GetRequiredService<IDataSeeder>().Initialize();
            }
            catch (Exception ex)
            {

                scope.ServiceProvider.GetRequiredService<ILogger<Program>>().LogError(ex, "could not insert data into database");
            }

            return app;
        }
    }
}
