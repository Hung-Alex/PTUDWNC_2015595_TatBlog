using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;

namespace TatBlog.Core.DTO
{
    public  class PostQuey
    {
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public string UrlSlug { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        //public Tag Tag { get; set; }
    }
}
