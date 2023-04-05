using FluentValidation;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs.Authors;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;
using TatBlog.Core.Contracts;
using Mapster;

namespace TatBlog.WebApi.Endpoints
{
    public static class PostEndpoints
    {
        public static WebApplication MapPostEnpoints(
            this WebApplication app)
        {

            var routeGroupBuilder = app.MapGroup("/api/posts");

            routeGroupBuilder.MapGet("/", GetPosts)
                             .WithName("GetPosts")
                              .Produces<ApiResponse<PaginationResult<PostItem>>>();

            return app;


        }

        private static async Task<IResult> GetPosts([AsParameters] PostFilterModel model, [AsParameters] PagingModel pagingModel, IBlogRepository blogRepository,IMapper mapper)
        {
           var query=mapper.Map<PostQuey>(model);
            var postList = await blogRepository.GetPostByQueryAsync(query, pagingModel, post => post.ProjectToType<PostItem>());
            var paginationResult = new PaginationResult<PostItem>(postList); 
            return Results.Ok(ApiResponse.Success(paginationResult));
        }


    }
}

