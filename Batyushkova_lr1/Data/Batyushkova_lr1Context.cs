using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Batyushkova_lr1.Models;

namespace Batyushkova_lr1.Data
{
    public class Batyushkova_lr1Context : DbContext
    {
        public Batyushkova_lr1Context (DbContextOptions<Batyushkova_lr1Context> options)
            : base(options)
        {
        }

        public DbSet<Batyushkova_lr1.Models.Dish> Dish { get; set; } = default!;
        public DbSet<Batyushkova_lr1.Models.Order> Order { get; set; } = default!;
        public DbSet<Batyushkova_lr1.Models.Table> Table { get; set; } = default!;
    }
}
