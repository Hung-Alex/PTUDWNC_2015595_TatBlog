using TatBlog.WebApi.Endpoints;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Mapsters;
using TatBlog.WebApi.Validations;

var builder = WebApplication.CreateBuilder(args);
{
    builder.ConfigureCors()
        .ConfigureNlog()
        .ConfigureServices()
        .ConfigureSwaggerOpenApi()
        .ConfigureMapster()
        .ConfigureFluentValidation();
}

var app = builder.Build();
{
    app.SetupRequestPieLine();
    //configure Api endpoints
    app.MapAuthorEnpoints();
    app.MapCategoryEnpoints();
    app.MapPostEnpoints();
    app.MapTagEndpoints();



    app.Run();

}





