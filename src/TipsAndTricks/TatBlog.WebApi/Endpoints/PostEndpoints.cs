﻿using FluentValidation;
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
using System.Net;
using System;
using NPOI.SS.Formula.Functions;

using System.Text.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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

            routeGroupBuilder.MapGet("/featured/{limit:int}", GetFeaturedPost)
                         .WithName("GetFeaturedPost")
                         .Produces<ApiResponse<IList<PostDto>>>();

            routeGroupBuilder.MapGet("/random/{limit:int}", GetRandomPost)
                             .WithName("GetRandomPost")
                             .Produces<ApiResponse<IList<PostItem>>>();

            routeGroupBuilder.MapGet("/archives/{limit:int}", GetArchivesPost)
                             .WithName("GetArchivesPost")
                             .Produces<ApiResponse<IList<DateItem>>>();

            routeGroupBuilder.MapGet("/{id:int}", GetPostDetails)
                             .WithName("GetPostById")
                             .Produces<ApiResponse<PostDetail>>();

            routeGroupBuilder.MapGet("/byslug/{slug::regex(^[a-z0-9_-]+$)}", GetPostBySlug)
                             .WithName("GetPostBySlug")
                             .Produces<ApiResponse<PaginationResult<PostDto>>>();

            routeGroupBuilder.MapPost("/", AddPost)
                             .WithName("AddNewPost")
                             .AddEndpointFilter<ValidatorFilter<PostEditModel>>()
                             .Produces(401)
                             .Produces<ApiResponse<PostItem>>();

            routeGroupBuilder.MapPut("/{id:int}", UpdatePost)
                             .WithName("UpdatePost")
                             .AddEndpointFilter<ValidatorFilter<PostEditModel>>()
                             .Produces(401)
                             .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeletePost)
                             .WithName("DeletePost")
                             .Produces(401)
                             .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapPost("/{id:int}/picture", SetPostPicture)
                             .WithName("SetPostPicture")
                             .Accepts<IFormFile>("multipart/formdata")
                             .Produces(401)
                             .Produces<string>();

            

            return app;


        }

        private static async Task<IResult> GetPosts([AsParameters] PostFilterModel model, [AsParameters] PagingModel pagingModel, IBlogRepository blogRepository,IMapper mapper)
        {
           var query=mapper.Map<PostQuey>(model);
            var postList = await blogRepository.GetPostByQueryAsync(query, pagingModel, post => post.ProjectToType<PostItem>());
          
            

            var paginationResult = new PaginationResult<PostItem>(postList); 
            return Results.Ok(ApiResponse.Success(paginationResult));
        }
   
        private static async Task<IResult> GetFeaturedPost(int limit, IBlogRepository blogRepository, IMapper mapper)
        {
            var posts = await blogRepository.GetFeaturedPostToTakeNumber(limit);

          

            return Results.Ok(ApiResponse.Success(posts.Select(t=>mapper.Map<PostDto>(t)).ToList()));
        }

        private static async Task<IResult> GetRandomPost(int limit, IBlogRepository blogRepository,IMapper mapper)
        {
            var posts = await blogRepository.GetPostRandomsAsync(limit);
            //try
            //{
            //    var testJson = System.Text.Json.JsonSerializer.Serialize(posts).Length;
            //}
            //catch (Exception e)
            //{

            //    Console.WriteLine(e.Message);
            //}
           
            List<PostItem> listDest = mapper.Map<IList<Post>, List<PostItem>>(posts);



            return Results.Ok(ApiResponse.Success(listDest));
        }

        private static async Task<IResult> GetArchivesPost(int limit, IBlogRepository blogRepository)
        {
            var postDate = await blogRepository.GetArchivesPostAsync(limit);

            return Results.Ok(ApiResponse.Success(postDate));
        }

        private static async Task<IResult> GetPostDetails(int id, IBlogRepository blogRepository, IMapper mapper)
        {
            var post = await blogRepository.GetCachedPostByIdAsync(id);

            var postItem= mapper.Map<PostDetail>(post);

            return post == null ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy bài viết có mã số {id}")) : Results.Ok(ApiResponse.Success(postItem));
        }

        private static async Task<IResult> GetPostBySlug([FromRoute] string slug, [AsParameters] PagingModel pagingModel, IBlogRepository blogRepository)
        {
            var postQuery = new PostQuey
            {
                UrlSlug = slug,
                PublishedOnly = true
            };

            var postsList = await blogRepository.GetPostByQueryAsync(postQuery, pagingModel, posts => posts.ProjectToType<PostDto>());

            var post = postsList.FirstOrDefault();

            return Results.Ok(ApiResponse.Success(post));
        }

        private static async Task<IResult> AddPost(PostEditModel model, IBlogRepository blogRepository, IMapper mapper)
        {
            if (await blogRepository.IsPostSlugExistedAsync(0, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var post = mapper.Map<Post>(model);
            await blogRepository.CreateOrUpdatePostAsync(post, model.GetSelectedTags());

            return Results.Ok(ApiResponse.Success(mapper.Map<PostItem>(post), HttpStatusCode.Created));
        }

        private static async Task<IResult> UpdatePost(int id, PostEditModel model, IBlogRepository blogRepository, IMapper mapper)
        {
            if (await blogRepository.IsPostSlugExistedAsync(id, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var post = mapper.Map<Post>(model);
            post.Id = id;

            return await blogRepository.CreateOrUpdatePostAsync(post, model.GetSelectedTags())!=null ? Results.Ok(ApiResponse.Success("Post is updated", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not found post"));
        }

        private static async Task<IResult> DeletePost(int id, IBlogRepository blogRepository)
        {
            return await blogRepository.DeletePost(id) ? Results.Ok(ApiResponse.Success("Post is deleted", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find post with id = {id}"));
        }

        private static async Task<IResult> SetPostPicture(int id, IFormFile imageFile, IBlogRepository blogRepository, IMediaManager mediaManager)
        {
            var post = await blogRepository.GetCachedPostByIdAsync(id);
            string newImagePath = string.Empty;

            // Nếu người dùng có upload hình ảnh minh họa cho bài viết
            if (imageFile?.Length > 0)
            {
                // Thực hiện việc lưu tập tin vào thư mực uploads
                newImagePath = await mediaManager.SaveFileAsync(imageFile.OpenReadStream(), imageFile.FileName, imageFile.ContentType);

                if (string.IsNullOrWhiteSpace(newImagePath))
                {
                    return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, "Không lưu được tập tin"));
                }

                // Nếu lưu thành công, xóa tập tin hình ảnh cũ (nếu có)
                await mediaManager.DeleteFileAsync(post.ImageUrl);
                post.ImageUrl = newImagePath;
            }

            return Results.Ok(ApiResponse.Success(newImagePath));
        }

       


    }
}

