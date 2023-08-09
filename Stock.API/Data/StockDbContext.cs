using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stock.API.Models;

namespace Stock.API.Data
{
    public class StockDbContext:DbContext
    {
        #region Properties
        public DbSet<Models.Stock> Stocks { get; set; }
        public DbSet<StockHistory> StockHistories { get; set; }
        public DbSet<StockOrder> StockOrders { get; set; }
        public DbSet<StockPosition> StockPositions { get; set; }
        #endregion

        #region Constructor
        public StockDbContext(DbContextOptions options):base(options)
        {

        }
        #endregion

        #region Methods
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Stock>()
                        .ToTable("Stock")
                        .HasKey(s => s.StockId);

            modelBuilder.Entity<StockHistory>()
                        .ToTable("StockHistory")
                        .HasKey(s => s.StockHistoryId);

            modelBuilder.Entity<StockHistory>()
                        .HasOne<Models.Stock>()
                        .WithMany(s => s.StockHistories)
                        .HasForeignKey(s => s.StockId);

            modelBuilder.Entity<StockOrder>()
                        .ToTable("StockOrder")
                        .HasKey(s => s.StockOrderId);

            modelBuilder.Entity<StockPosition>()
                        .ToTable("StockPosition")
                        .HasKey(s => s.StockPositionId);

            modelBuilder.Entity<StockPosition>()
                        .HasOne<Models.Stock>(s => s.Stock)
                        .WithMany()
                        .HasForeignKey(s => s.StockId);

            //modelBuilder.Entity<Models.Stock>()
            //           .ToTable("Stock")
            //           .HasMany<Models.StockPosition>()
            //           .WithOne(s => s.StockId)
            //           .HasForeignKey(s => s.StockId);

        }

        #endregion
    }
}
