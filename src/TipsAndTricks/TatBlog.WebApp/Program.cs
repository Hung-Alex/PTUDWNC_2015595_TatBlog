using Microsoft.EntityFrameworkCore;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;

var builder = WebApplication.CreateBuilder(args);
{
    //thêm các dịch vụ được yêu cầu từ mvc framework
    builder.Services.AddControllersWithViews();

    //dang ki dich vuj DI container
    builder.Services.AddDbContext<BlogDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddScoped<IBlogRepository, BlogRepository>();
    builder.Services.AddScoped<IDataSeeder, DataSeeder>();


}
var app = builder.Build();
{
    //config http request pieline

    //thêm middware để hiển thị thông báo lỗi
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
    //dinh nghia route template ,route constraint cho cac
    //endpoint ket hop voi cac action trong cac controler
    app.MapControllerRoute("defaut",pattern:"{controller=Blog}/{action=Index}/{id?}");
}
using (var scope=app.Services.CreateScope()) { 
    var seeder=scope.ServiceProvider.GetService<IDataSeeder>();
    seeder.Initialize();
}


    app.Run();
