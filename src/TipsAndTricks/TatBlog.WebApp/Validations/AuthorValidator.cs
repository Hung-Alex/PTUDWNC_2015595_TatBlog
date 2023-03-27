using FluentValidation;
using System.Net.Mail;
using TatBlog.Services.Blogs;
using TatBlog.Services.Blogs.Authors;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations
{
    public class AuthorValidator : AbstractValidator<AuthorEditModel>
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorValidator(IAuthorRepository authorRepository)
        {
             _authorRepository = authorRepository;

            RuleFor(p => p.FullName)
            .NotEmpty()
            .WithMessage("Tên tác giả   không được để trống")
            .MaximumLength(500)
            .WithMessage("Tên tác giả dài tối đa '{MaxLength}'");

            RuleFor(p => p.Email)
            .Must(IsValid)
            .WithMessage("Điền email hợp lệ không được để trống");

            RuleFor(p => p.Notes)
           .NotEmpty()
           .WithMessage("Chú ý không được để trống");


            RuleFor(p => p.UrlSlug)
            .NotEmpty()
            .WithMessage("Slug của bài viết không được để trống")
            .MaximumLength(1000)
            .WithMessage("Slug dài tối đa '{MaxLength}'");

            RuleFor(p => p.UrlSlug)
            .MustAsync(async (postModel, slug, cancellationToken) => !await _authorRepository.IsAuthorSlugExistedAsync(postModel.Id, slug, cancellationToken))
            .WithMessage("Slug '{PropertyValue}' đã được sử dụng");

            When(p => p.Id <= 0, () => {
                RuleFor(p => p.ImageFile)
                .Must(f => f is { Length: > 0 })
                .WithMessage("Bạn phải chọn hình ảnh cho tác giả");
            })
            .Otherwise(() => {
                RuleFor(p => p.ImageFile)
        .MustAsync(SetImageIfNotExist)
        .WithMessage("Bạn phải chọn hình ảnh cho tác giả");
            });
        }

        // Kiểm tra xem bài viết đã có hình ảnh chưa
        // Nếu chưa có, bắt buộc người dùng phải chọn file
        private async Task<bool> SetImageIfNotExist(AuthorEditModel authorModel, IFormFile imageFile, CancellationToken cancellationToken)
        {
            // Lấy thông tin bài viết từ CSDL
            var author = await _authorRepository.FindAuthorbyId(authorModel.Id,cancellationToken);

            // Nếu bài viết đã có hình ảnh => Không bắt buộc chọn file
            if (!string.IsNullOrWhiteSpace(author?.ImageURL))
                return true;

            // Ngược lại (bài viết chưa có hình ảnh), kiểm tra xem
            // người dùng đã chọn file hay chưa. Nếu chưa thì báo lỗi
            return imageFile is { Length: > 0 };
        }
        public bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
