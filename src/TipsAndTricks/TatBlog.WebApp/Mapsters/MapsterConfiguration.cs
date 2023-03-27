using Mapster;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Mapsters
{
    public class MapsterConfiguration
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Post, PostItem>()
                .Map(dest => dest.CategoryName, src => src.Category.Name)
                .Map(dest => dest.AuthorName, src => src.Author.FullName)
                .Map(dest => dest.Tags, src => src.Tags.Select(tag => tag.Name));

            config.NewConfig<PostFilterModel, PostQuey>()
                .Map(dest => dest.PublishedOnly, src => true);

            config.NewConfig<CategoriesFilterModel, PostQuey>()
                .Map(dest => dest.PublishedOnly, src => true);

            config.NewConfig<TagsFilterModel, PostQuey>()
                .Map(dest => dest.PublishedOnly, src => true);

            config.NewConfig<AuthorFilterModel, PostQuey>()
                .Map(dest => dest.PublishedOnly, src => true);


            config.NewConfig<CategoriesFilterModel, Category>();

            config.NewConfig<AuthorFilterModel, Author>();

            config.NewConfig<AuthorEditModel, Author>()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.ImageURL);

            config.NewConfig<PostEditModel, Post>()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.ImageUrl);

            config.NewConfig<Post, PostItem>()
                .Map(dest => dest.SelectedTags, src => string.Join("r/n", src.Tags.Select(x => x.Name)))
                .Ignore(dest => dest.CategoryList)
                .Ignore(dest => dest.AuthorList)
                .Ignore(dest => dest.ImageFile);
        }
    }
}
