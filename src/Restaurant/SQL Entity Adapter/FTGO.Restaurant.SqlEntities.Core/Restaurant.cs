using AD.Messaging.SqlEF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FTGO.Restaurant.SqlEntities.Core
{
    public class Restaurant : IEntity<Guid>
    {
        public Guid Id { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = null!;

        public string Name { get; set; } = null!;

        public List<MenuItem> Menu { get; set; } = new();
    }

    public class MenuItem
    {
        public Guid Id { get; set; }

        public int Order { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }
    }

    public class RestaurantCreated : IMessage<Guid, Guid>
    {
        public Guid Id { get; set; }

        public Guid EntityId { get; set; }

        public string Name { get; set; } = null!;
    }

    public class RestaurantDbContext : DbContext, IOutboxDbContext<RestaurantCreated>
    {
        public RestaurantDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Restaurant> Restaurants { get; set; } = null!;
        public DbSet<RestaurantCreated> Outbox { get; set; } = null!;
    }

    public class RestaurantDataAccess
    {
        readonly RestaurantDbContext context;

        public RestaurantDataAccess(RestaurantDbContext context)
        {
            this.context = context;
        }

        public async Task<Restaurant> Create(Restaurant entity, RestaurantCreated @event)
        {
            for (int i = 0; i < entity.Menu.Count; i++)
            {
                entity.Menu[i].Order = i;
            }
            context.Restaurants.Add(entity);
            context.Publish(@event);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<Restaurant?> Read(Guid id) =>
            await context.Restaurants.Include(_ => _.Menu.OrderBy(_ => _.Order)).SingleOrDefaultAsync(_ => _.Id == id);
    }
}
