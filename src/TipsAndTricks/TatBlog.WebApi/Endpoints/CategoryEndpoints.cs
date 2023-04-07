using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Blogs.Authors;
using TatBlog.WebApi.Filters;
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

            routeGroupBuilder.MapGet("/{id:int}", GetCategoriesById)
                            .WithName("GetCategoriesById")
                            .Produces<CategoryItem>();

            routeGroupBuilder.MapGet("/{slug::regex(^[a-z0-9_-]+$)}/posts", GetPostsByCategorySlug)
                            .WithName("GetPostsByCategorySlug")
                            .Produces<PaginationResult<PostDto>>();
            routeGroupBuilder.MapPost("/", AddCategory)
                         .WithName("AddNewCategory")
                         .AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
                         .Produces(201)
                         .Produces(400)
                         .Produces(409);

            routeGroupBuilder.MapPut("/{id:int}", UpdateCategory)
                             .WithName("UpdateCategory")
                             .AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
                             .Produces(204)
                             .Produces(400)
                             .Produces(409);

            routeGroupBuilder.MapDelete("/{id:int}", DeleteCategory)
                             .WithName("DeleteCategory")
                             .Produces(204)
                             .Produces(404);


            return app;
        }

        public static async Task<IResult> GetCategories([AsParameters] CategoryFilterModel model,ICategoryResponsitory categoryResponsitory)
        {
            var categoriesList = await categoryResponsitory.GetPagedCategoriesAsync(model, model.Name);

            var paginationResult = new PaginationResult<CategoryItem>(categoriesList);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        public static async Task<IResult> GetCategoriesById(int id,ICategoryResponsitory categoryResponsitory,IMapper mapper)
        {
            var author = await categoryResponsitory.GetCategorysByIdAsync(id);

            return Results.Ok(author!=null?ApiResponse.Success(author): ApiResponse.Fail(HttpStatusCode.NotFound,$"Không tìm thấy chủ đề với Id = {id} "));
        }
        public static async Task<IResult> GetPostsByCategorySlug([FromRoute] string slug, [AsParameters] PagingModel model, IBlogRepository blogRepository)
        {
            var postQuery = new PostQuey
            {
                CategorySlug=slug
            };

            var postsList = await blogRepository.GetPostByQueryAsync(postQuery, model, posts => posts.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(postsList);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }
        private static async Task<IResult> AddCategory(CategoryEditModel model, ICategoryResponsitory categoryRepository, IMapper mapper)
        {
            if (await categoryRepository.CheckCategorySlugExisted(0, model.UrlSlug))
            {
                return Results.Conflict($"Slug '{model.UrlSlug}' đã được sử dụng");
            }

            var category = mapper.Map<Category>(model);
            await categoryRepository.AddOrUpdateCategoryAsync(category);

            return Results.CreatedAtRoute("GetCategoryById", new { category.Id }, mapper.Map<CategoryItem>(category));
        }

        private static async Task<IResult> UpdateCategory(int id, CategoryEditModel model, ICategoryResponsitory categoryRepository, IMapper mapper)
        {
            if (await categoryRepository.CheckCategorySlugExisted(id, model.UrlSlug))
            {
                return Results.Conflict($"Slug '{model.UrlSlug}' đã được sử dụng");
            }

            var category = mapper.Map<Category>(model);
            category.Id = id;

            return await categoryRepository.AddOrUpdateCategoryAsync(category) ? Results.Ok(ApiResponse.Success(HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Success(HttpStatusCode.NotFound));
        }

        private static async Task<IResult> DeleteCategory(int id, ICategoryResponsitory categoryRepository)
        {
            return await categoryRepository.DeleteCategoryByIdAsync(id) ? Results.Ok(HttpStatusCode.NoContent) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find category with id = {id}"));
        }
    }
}
