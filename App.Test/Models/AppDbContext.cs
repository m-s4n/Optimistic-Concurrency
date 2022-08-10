using App.Test.Models;
using Microsoft.EntityFrameworkCore;

namespace ConcurrencyWeb.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            // Db configurasyonu program cs için verilecek
        }

        public DbSet<Urun> Urun { get; set; }




        // Fluent Apı

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Urun>().UseXminAsConcurrencyToken();
            modelBuilder.Entity<Urun>().ToTable("urun").HasKey(x => x.Id);
            modelBuilder.Entity<Urun>().Property(x => x.Id).HasColumnName("id").HasColumnType("integer");
            modelBuilder.Entity<Urun>().Property(x => x.Ad).HasColumnName("ad").HasColumnType("varchar(250)");
            modelBuilder.Entity<Urun>().Property(x => x.Fiyat).HasColumnName("fiyat").HasColumnType("numeric").HasPrecision(18, 2);
            base.OnModelCreating(modelBuilder);
        }
    }
}
