using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Data.Seeders
{
    public class DataSeeder : IDataSeeder
    {
        private readonly BlogDbContext _dbContext;
        public DataSeeder(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Initialize()
        {
            _dbContext.Database.EnsureCreated();
            if (_dbContext.Posts.Any())
            {
                return;
            }
            var authors = AddAuthor();
            var categories = AddCategories();
            var tags = AddTags();
            var posts = AddPosts(authors, categories, tags);
        }

        private IList<Post> AddPosts(IList<Author> authors, IList<Category> categories, IList<Tag> tags)
        {
            var posts = new List<Post>()
            {
                  new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                      tags[0],

                    }
                  },
                   new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenario",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[1],
                    Category = categories[1],
                    Tags = new List<Tag>()
                    {
                      tags[0],
                      tags[3],

                    }
                  },
                    new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[3],
                    Category = categories[3],
                    Tags = new List<Tag>()
                    {
                      tags[0],
                      tags[10],

                    }
                  },
                     new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[2],
                    Category = categories[4],
                    Tags = new List<Tag>()
                    {
                      tags[5],
                      tags[2],
                    }
                  },
                      new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[3],
                    Category = categories[3],
                    Tags = new List<Tag>()
                    {
                      tags[0],
                      tags[7],

                      tags[18],

                    }
                  },
                       new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 30,
                    Author = authors[3],
                    Category = categories[2],
                    Tags = new List<Tag>()
                    {
                      tags[2],
                    }
                  },
                        new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[3],
                    Category = categories[1],
                    Tags = new List<Tag>()
                    {
                      tags[0],
                      tags[4],

                    }
                  },
                         new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 100,
                    Author = authors[2],
                    Category = categories[4],
                    Tags = new List<Tag>()
                    {
                      tags[0],
                      tags[2],
                      tags[4],


                    }
                  },
                          new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 12000,
                    Author = authors[1],
                    Category = categories[6],
                    Tags = new List<Tag>()
                    {
                      tags[0],
                      tags[4],
                      tags[3],

                    }
                  },
                           new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 1021121,
                    Author = authors[4],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                      tags[1],
                      tags[2],
                      tags[3],
                      tags[4],

                    }
                  },
                            new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                      tags[0],
                    }
                  },
                             new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                      tags[0],
                    }
                  },
                              new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                      tags[0],
                    }
                  },
                               new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                      tags[0],
                    }
                  },
                                new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                      tags[0],
                    }
                  },
                                 new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                      tags[0],
                    }
                  },
                                  new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                      tags[0],
                    }
                  },
                                   new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                      tags[0],
                    }
                  },
                                    new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                      tags[0],
                    }
                  },
                                     new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                      tags[0],
                    }
                  },
                                      new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                      tags[0],
                    }
                  },
                                       new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                      tags[0],
                    }
                  },
                                        new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                      tags[0],
                    }
                  },
                                         new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                      tags[0],
                    }
                  },
                                          new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                      tags[0],
                    }
                  },
                                           new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                      tags[0],
                    }
                  },
                                            new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                      tags[0],
                    }
                  },
                                             new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                      tags[0],
                    }
                  },
                                              new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                      tags[0],
                    }
                  },
                                               new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                      tags[0],
                    }
                  },
                                                new()
                  {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled.",
                    Description = "Here's a few great DON'T and DO example posts.",
                    Meta = "David and friends has a great repository filled.",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                      tags[0],
                    }
                  },
            };

            _dbContext.AddRange(posts);
            _dbContext.SaveChanges();

            return posts;
        }

        private IList<Author> AddAuthor()
        {
            var authors = new List<Author>()
           {
               new()
               {
                   FullName="Jason Mouth",
                   UrlSlug="jason-mouth",
                   JoinedDate=new DateTime(2022,10,21),
                   Email="json@gmail.com"
               },
               new()
               {
                   FullName="Jessica Wonder",
                   UrlSlug="jessica-wonder",
                   JoinedDate=new DateTime(2020,4,19),
                   Email="jessica665@motip.com"
               },
               new()
               {
                   FullName="Jessica Wonder-IS",
                   UrlSlug="jessica-wonder-IS",
                   JoinedDate=new DateTime(2020,4,20),
                   Email="jessica665IS@motip.com"
               },
                new()
               {
                   FullName="KangSang",
                   UrlSlug="kang-sang",
                   JoinedDate=new DateTime(2020,5,13),
                   Email="kangsang34@motip.com"
               },
                new()
               {
                   FullName="LeeHS",
                   UrlSlug="lee-hs",
                   JoinedDate=new DateTime(2020,3,20),
                   Email="lehs345@motip.com"
               },
           };
            _dbContext.Authors.AddRange(authors);
            _dbContext.SaveChanges();
            return authors;
        }

        private IList<Category> AddCategories()
        {
            var categories = new List<Category>()
            {
                new(){Name=".NET Core",Description=".Net Core",UrlSlug=".net-core" },
                new(){Name="Architeture",Description="Architeture",UrlSlug="architeture" },
                new(){Name="Messaging",Description="Messaging",UrlSlug="messaging" },
                new(){Name="OOP",Description="Object Oriented Programming",UrlSlug="object-oriented-programming" },
                new(){Name="Design Pattern",Description="Design Pattern",UrlSlug="design-pattern" },

                new(){Name="Life",Description="life",UrlSlug="life" },
                new(){Name="Food",Description="food",UrlSlug="food" },
                new(){Name="Animal",Description="Animal",UrlSlug="Animal" },
                new(){Name="Hack",Description="hack",UrlSlug="hack" },
                new(){Name="News",Description="news",UrlSlug="news" },

            };
            _dbContext.Categories.AddRange(categories);
            _dbContext.SaveChanges();
            return categories;
        }

        private IList<Tag> AddTags()
        {
            var tags = new List<Tag>()
            {
                  new()
                  {
                    Name = "Google",
                    UrlSlug = "google-application",
                    Description = "Google applications architecture design",
                  },
                  new()
                  {
                    Name = "ASP .NET MVC",
                    UrlSlug = "asp-mvc-application",
                    Description = "ASP .NET MVC application architecture design",
                  },
                  new()
                  {
                    Name = "Razor Page",
                    UrlSlug = "razor-page",
                    Description = "Razor Page application architecture design",
                  },
                  new()
                  {
                    Name = "Blazor",
                    UrlSlug = "blazor-application",
                    Description = "Blazor application architecture design",
                  },
                  new()
                  {
                    Name = "Deep Learning",
                    UrlSlug = "deep-learning",
                    Description = "Deeplearning application architecture design",
                  },
                  new()
                  {
                    Name = "Neural Network",
                    UrlSlug = "neural-networking",
                    Description = "Neural Network architecture design",
                  },
                  new()
                  {
                    Name = "Viet Nam Number 1",
                    UrlSlug = "Viet-Nam-Numeber-1",
                    Description = "Viet Nam Number 1",
                  },
                  new()
                  {
                    Name = "U23",
                    UrlSlug = "u23",
                    Description = "soccer",
                  },
                  new()
                  {
                    Name = "food",
                    UrlSlug = "food",
                    Description = "Viet Nam food",
                  },
                  new()
                  {
                    Name = "banh Xeo",
                    UrlSlug = "Banh-Xeo",
                    Description = "Banh Xeo food",
                  },
                  new()
                  {
                    Name = "Cha Ca",
                    UrlSlug = "cha-ca",
                    Description = "food",
                  },
                   new()
                  {
                    Name = "Pho",
                    UrlSlug = "pho",
                    Description = "food",
                  },
                    new()
                  {
                    Name = "bun rieu",
                    UrlSlug = "bun-rieu",
                    Description = "food",
                  },
                     new()
                  {
                    Name = "Ga Quay",
                    UrlSlug = "ga-quay",
                    Description = "food",
                  },
                      new()
                  {
                    Name = "Vit Quay",
                    UrlSlug = "vit-quay",
                    Description = "food",
                  },
                       new()
                  {
                    Name = "Heo Quay",
                    UrlSlug = "heo-quay",
                    Description = "food",
                  },
                        new()
                  {
                    Name = "Ga luoc",
                    UrlSlug = "ga-luoc",
                    Description = "food",
                  },
                         new()
                  {
                    Name = "Ga Nuong",
                    UrlSlug = "ga-nuong",
                    Description = "food",
                  },
                          new()
                  {
                    Name = "Tom Hap",
                    UrlSlug = "tom-hap",
                    Description = "food",
                  },
                           new()
                  {
                    Name = "Cha La Lot",
                    UrlSlug = "cha-la-lot",
                    Description = "food",
                  },



            };


            _dbContext.Tags.AddRange(tags);
            _dbContext.SaveChanges();

            return tags;
        }
    }
}
