using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Blogs.Authors;
using TatBlog.Services.Media;
using Microsoft.AspNetCore.Builder;
using TatBlog.WebApi.Models;
using TatBlog.WebApi.Filters;
using System.Net;

namespace TatBlog.WebApi.Endpoints
{
    public static class AuthorEndpoints
    {
        public static WebApplication MapAuthorEnpoints(
            this WebApplication app)
        {
            
            var routeGroupBuilder = app.MapGroup("/api/authors");

            // Nested Map with defined specific route
            routeGroupBuilder.MapGet("/", GetAuthors)
                             .WithName("GetAuthors")
                             .Produces<PaginationResult<AuthorItem>>();

            routeGroupBuilder.MapGet("/{id:int}", GetAuthorDetails)
                             .WithName("GetAuthorById")
                             .Produces<AuthorItem>()
                             .Produces(404);

            routeGroupBuilder.MapGet("/{slug::regex(^[a-z0-9_-]+$)}/posts", GetPostByAuthorSlug)
                             .WithName("GetPostByAuthorSlug")
                             .Produces<PaginationResult<PostDto>>();

            routeGroupBuilder.MapPost("/", AddAuthor)
                             .WithName("AddNewAuthor")
                             .AddEndpointFilter<ValidatorFilter<AuthorEditModel>>()
                             .Produces(201)
                             .Produces(400)
                             .Produces(409);

            routeGroupBuilder.MapPut("/{id:int}", UpdateAuthor)
                             .WithName("UpdateAuthor")
                             .AddEndpointFilter<ValidatorFilter<AuthorEditModel>>()
                             .Produces(204)
                             .Produces(400)
                             .Produces(409);

            routeGroupBuilder.MapDelete("/{id:int}", DeleteAuthor)
                             .WithName("DeleteAuthor")
                             .Produces(204)
                             .Produces(404);

            routeGroupBuilder.MapPost("/{id:int}/avatar", SetAuthorPicture)
                             .WithName("SetAuthorPicture")
                             .Accepts<IFormFile>("multipart/formdata")
                             .Produces<string>()
                             .Produces(400);

            routeGroupBuilder.MapGet("/best/{limit:int}", GetBestAuthors)
                             .WithName("GetBestAuthors")
                             .Produces<PagedList<Author>>();

            return app;


        }
        private static async Task<IResult> GetAuthors([AsParameters] AuthorFilterModel model, IAuthorRepository authorRepository)
        {
            var authorList = await authorRepository.GetPagedAuthorsAsync(model, model.Name);

            var paginationResult = new PaginationResult<AuthorItem>(authorList);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }
        private static async Task<IResult> GetAuthorDetails(int id, IAuthorRepository authorRepository, IMapper mapper)
        {
            var author = await authorRepository.GetCachedAuthorByIdAsync(id);

            return author == null ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,$"Không tìm thấy tác giả có mã số {id}") ): Results.Ok(ApiResponse.Success(mapper.Map<AuthorItem>(author)));
        }


        private static async Task<IResult> GetPostByAuthorId(int id, [AsParameters] PagingModel pagingModel, IBlogRepository blogRepository)
        {
            var postQuery = new PostQuey
            {
                AuthorId = id,
                PublishedOnly = true
            };

            var postsList = await blogRepository.GetPostByQueryAsync(postQuery, pagingModel, posts => posts.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(postsList);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetPostByAuthorSlug([FromRoute] string slug, [AsParameters] PagingModel pagingModel, IBlogRepository blogRepository)
        {
            var postQuery = new PostQuey
            {
                AuthorSlug = slug,
                PublishedOnly = true
            };

            var postsList = await blogRepository.GetPostByQueryAsync(postQuery, pagingModel, posts => posts.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(postsList);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }


        private static async Task<IResult> AddAuthor(AuthorEditModel model, IValidator<AuthorEditModel> validator, IAuthorRepository authorRepository, IMapper mapper)
        {
            if (await authorRepository.IsAuthorSlugExistedAsync(0, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,$"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var author = mapper.Map<Author>(model);
            await authorRepository.AddOrUpdateAsync(author);

            return Results.Ok(ApiResponse.Success( mapper.Map<AuthorItem>(author),HttpStatusCode.Created));
        }

        private static async Task<IResult> UpdateAuthor(int id, AuthorEditModel model, IValidator<AuthorEditModel> validator, IAuthorRepository authorRepository, IMapper mapper)
        {
            if (await authorRepository.IsAuthorSlugExistedAsync(id, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var author = mapper.Map<Author>(model);
            author.Id = id;

            return await authorRepository.AddOrUpdateAsync(author) 
                ? Results
                .Ok(ApiResponse.Success("author is updated",HttpStatusCode.NoContent)) 
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,"could not find author"));
        }

        private static async Task<IResult> DeleteAuthor(int id, IAuthorRepository authorRepository)
        {
            return await authorRepository.DeleteAuthorAsync(id) ? Results.Ok(ApiResponse.Success(HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,$"Could not find author with id = {id}"));
        }

        private static async Task<IResult> SetAuthorPicture(int id, IFormFile imageFile, IAuthorRepository authorRepository, IMediaManager mediaManager)
        {
            var imageUrl = await mediaManager.SaveFileAsync(imageFile.OpenReadStream(), imageFile.FileName, imageFile.ContentType);

            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, "Không lưu được tập tin"));
            }

            await authorRepository.SetImageUrlAsync(id, imageUrl);

            return Results.Ok(ApiResponse.Success(imageUrl));
        }

        private static async Task<IResult> GetBestAuthors(int limit, IAuthorRepository authorRepository)
        {
            var authors = await authorRepository.Find_N_MostPostByAuthorAsync(limit);

            var pagedResult = new PagedList<Author>(authors, 1, limit, authors.Count);

            return Results.Ok(ApiResponse.Success(pagedResult));
        }



    }
}

