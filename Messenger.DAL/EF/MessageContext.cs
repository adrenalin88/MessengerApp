using Messenger.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Messenger.DAL.EF
{
    public class MessageContext<T> : DbContext
        where T : MessageBase
    {
        public DbSet<T> Messages { get; set; }
        public MessageContext(DbContextOptions<MessageContext<T>> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
