using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Services.Extensions
{
    public static class IEnumerableExtension
    {
        public static string GenerateSlug(this string strings)
        {
            return strings.ToLower();
        }
    }
}
