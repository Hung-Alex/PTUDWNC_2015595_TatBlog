using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Services.Blogs
{
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly BlogDbContext _dbContext;
        private readonly IMailService _mailService;

        public SubscriberRepository(BlogDbContext dbContext, IMailService mailService)
        {
            _dbContext = dbContext;
            _mailService = mailService;
        }
        public async Task<bool> BlockSubscriberAsync(int id, string reason, string notes, CancellationToken cancellationToken = default)
        {
           var subscriber = await GetSubscriberByIdAsync(id);
            if (subscriber!=null)
            {
                subscriber.Flag = true;// true is admin block user 
                await _dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteSubscriberAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Subscriber>().Where(x => x.Id == id).ExecuteDeleteAsync()>0;
        }

        public async Task<Subscriber> GetSubscriberByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Subscriber>().FirstOrDefaultAsync(x => x.Email.Equals(email), cancellationToken);
        }

        public async Task<Subscriber> GetSubscriberByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Subscriber>().FirstOrDefaultAsync(x => x.Id==id, cancellationToken);
        }

        public Task<IPagedList<Subscriber>> SearchSubscribersAsync(IPagingParams pagingParams, string keyword, bool unsubscribed, bool involuntary, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public  async Task<bool> SubscribeAsync(string email, CancellationToken cancellationToken = default)
        {
            if (await GetSubscriberByEmailAsync(email) != null)
            {
                await _mailService.SendEmailMessage(email);
                return true;
            }
            Subscriber subscriber = new Subscriber{ Email=email,SignUpDate=DateTime.Now,UnSignUpDate=DateTime.Now,Flag=false,Reason=null,Notes=null,Status=true};
            _dbContext.Set<Subscriber>().Add(subscriber);
             await _dbContext.SaveChangesAsync();
             await _mailService.SendEmailMessage(email);
            return true;
        }

        public async Task<bool> UnsubscribeAsync(string email, string reason, bool voluntary, CancellationToken cancellationToken = default)
        {
            var subscriber = await GetSubscriberByEmailAsync(email);
            if (subscriber!=null)
            {
               subscriber.Reason=reason;
                subscriber.Status=voluntary;
                
            }
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
