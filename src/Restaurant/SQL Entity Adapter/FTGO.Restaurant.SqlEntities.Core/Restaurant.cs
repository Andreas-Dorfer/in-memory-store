using AD.Messaging.SqlEF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FTGO.Restaurant.SqlEntities.Core
{
    public class Restaurant : IEntity<Guid>
    {
        public Guid Id { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; } = null!;

        public string Name { get; set; } = null!;

        public List<MenuItem> Menu { get; set; } = null!;
    }

    public class MenuItem
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }
    }

    public class RestaurantCreated : IMessage<Guid, Guid>
    {
        public Guid Id { get; set; }

        public Guid EntityId { get; set; }

        public string Name { get; set; } = null!;
    }

    public class RestaurantContext : DbContext
    {
        public DbSet<Restaurant> Restaurants { get; set; } = null!;
        public DbSet<RestaurantCreated> Outbox { get; set; } = null!;
    }
}
