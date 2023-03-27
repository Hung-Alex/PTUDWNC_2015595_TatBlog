﻿using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations
{
    public class CategoryValidator:AbstractValidator<CategoriesEditModel>
    {
        private readonly IBlogRepository _blogRepository;
        public CategoryValidator(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;

            RuleFor(p=>p.Name).NotEmpty()
            .WithMessage("Tên của chủ đề không được để trống")
            .MaximumLength(500)
            .WithMessage("Tên chủ đề dài tối đa '{MaxLength}'");

            RuleFor(p => p.UrlSlug)
            .NotEmpty()
            .WithMessage("Slug của chủ đề không được để trống")
            .MaximumLength(1000)
            .WithMessage("Slug dài tối đa '{MaxLength}'");

            RuleFor(p => p.Description)
            .NotEmpty()
            .WithMessage("Mô tả về bài viết không được để trống");

            RuleFor(p => p.ShowOnMenu)
                .NotNull()
                .WithMessage("bạn Phải chọn chế hiển thị cho việc hiển thị");
        }
    }
}
