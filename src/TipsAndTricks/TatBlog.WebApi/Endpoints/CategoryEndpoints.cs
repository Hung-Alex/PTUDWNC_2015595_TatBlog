using Microsoft.AspNetCore.Http.HttpResults;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.Services.Blogs.Authors;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class CategoryEndpoints
    {
        public static WebApplication MapCategoryEnpoints(
           this WebApplication app)
        {


            var routeGroupBuilder = app.MapGroup("/api/categories");
            routeGroupBuilder.MapGet("/", GetCategories)
                            .WithName("GetCategories")
                            .Produces<PaginationResult<CategoryItem>>();
            return app;
        }
            public static async Task<IResult> GetCategories([AsParameters] CategoryFilterModel model,ICategoryResponsitory categoryResponsitory)
        {
            var categoriesList = await categoryResponsitory.GetPagedCategoriesAsync(model, model.Name);

            var paginationResult = new PaginationResult<CategoryItem>(categoriesList);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }
    }
}
