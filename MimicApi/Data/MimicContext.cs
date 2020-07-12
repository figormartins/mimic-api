using Microsoft.EntityFrameworkCore;
using MimicApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicApi.Data
{
    public class MimicContext : DbContext
    {
        public MimicContext(DbContextOptions<MimicContext> options) : base(options)
        {
        }

        public DbSet<Word> Words { get; set; }
    }
}
