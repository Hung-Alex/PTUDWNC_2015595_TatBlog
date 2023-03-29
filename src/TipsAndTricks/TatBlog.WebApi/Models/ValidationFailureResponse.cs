namespace TatBlog.WebApi.Models
{
    public class ValidationFailureResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public ValidationFailureResponse(IEnumerable<string> errorMesssages)
        {
            Errors = errorMesssages;
        }
    }
}
