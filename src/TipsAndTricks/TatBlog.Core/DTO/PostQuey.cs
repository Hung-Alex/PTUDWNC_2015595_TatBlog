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
        public bool PublishedOnly { get; set; }
        public bool NotPublished { get; set; }
        public string CategorySlug { get; set; }
        public string AuthorSlug { get; set; }
        public string  TagSlug { get; set; }
        public string Keyword { get; set; }
        public string TitleSlug { get; set; }
        public int Day { get; set; }
        //------------------------------------ phần cũ below
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public string UrlSlug { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        //public Tag Tag { get; set; }
    }
}
