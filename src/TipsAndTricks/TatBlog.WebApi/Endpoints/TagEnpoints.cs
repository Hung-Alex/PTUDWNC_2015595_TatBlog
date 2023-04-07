using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class TagEnpoints
    {
        public static WebApplication MapTagEndpoints(this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/tags");

            // Nested Map with defined specific route
           

            routeGroupBuilder.MapGet("/all", GetAllTags)
                     .WithName("GetAllTags")
                     .Produces<ApiResponse<TagItem>>();

           

            return app;
        }

     

        private static async Task<IResult> GetAllTags(IBlogRepository blogRepository, IMapper mapper)
        {
            var tagList = await blogRepository.GetAllTagAsync();

            var tagDto = tagList.Select(t => mapper.Map<TagItem>(t));

            return Results.Ok(ApiResponse.Success(tagDto));
        }

       
    }
}
