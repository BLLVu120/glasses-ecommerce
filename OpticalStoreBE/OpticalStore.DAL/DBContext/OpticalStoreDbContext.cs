using Microsoft.EntityFrameworkCore;
using OpticalStore.DAL.Entities;

namespace OpticalStore.DAL.DBContext
{
    public class OpticalStoreDbContext : DbContext
    {
        public OpticalStoreDbContext(DbContextOptions<OpticalStoreDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductVariant> ProductVariants { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Users
            modelBuilder.Entity<User>(b =>
            {
                b.ToTable("Users");
                b.HasKey(x => x.Id);
                b.Property(x => x.FullName).HasMaxLength(150).IsRequired();
                b.Property(x => x.Email).HasColumnType("varchar(255)").IsRequired();
                b.Property(x => x.Phone).HasColumnType("varchar(20)");
                b.Property(x => x.PasswordHash).HasMaxLength(255).IsRequired();
                b.Property(x => x.Address).HasMaxLength(300);
                b.Property(x => x.DateOfBirth).HasColumnType("date");
                b.Property(x => x.Role).HasColumnType("varchar(50)").IsRequired();
                b.Property(x => x.RefreshToken).HasMaxLength(500);
                b.Property(x => x.RefreshTokenExpiryTime).HasColumnType("datetime2");

                b.Property(x => x.IsDeleted).HasDefaultValue(false);
                b.Property(x => x.CreatedAt).HasColumnType("datetime2(0)").HasDefaultValueSql("SYSUTCDATETIME()");

                b.HasIndex(x => x.Email).IsUnique().HasDatabaseName("UX_Users_Email_Active").HasFilter("[IsDeleted] = 0");
                b.HasIndex(x => x.Phone).IsUnique().HasDatabaseName("UX_Users_Phone_Active").HasFilter("[Phone] IS NOT NULL AND [IsDeleted] = 0");

                b.HasQueryFilter(x => !x.IsDeleted);
            });

            // Products
            modelBuilder.Entity<Product>(b =>
            {
                b.ToTable("Products");
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).HasMaxLength(200).IsRequired();
                b.Property(x => x.Description).HasMaxLength(1000);
                b.Property(x => x.ImageUrl).HasMaxLength(500);
                b.Property(x => x.BasePrice).HasColumnType("decimal(18,2)").IsRequired();
                b.Property(x => x.ProductType).HasColumnType("varchar(50)").IsRequired();

                b.Property(x => x.IsDeleted).HasDefaultValue(false);
                b.Property(x => x.CreatedAt).HasColumnType("datetime2(0)").HasDefaultValueSql("SYSUTCDATETIME()");

                b.HasCheckConstraint("CK_Products_BasePrice", "[BasePrice] >= 0");

                b.HasQueryFilter(x => !x.IsDeleted);
            });

            // ProductVariants
            modelBuilder.Entity<ProductVariant>(b =>
            {
                b.ToTable("ProductVariants");
                b.HasKey(x => x.Id);
                b.Property(x => x.VariantType).HasMaxLength(100);
                b.Property(x => x.Color).HasMaxLength(50);
                b.Property(x => x.Size).HasColumnType("varchar(20)");
                b.Property(x => x.Material).HasMaxLength(100);

                b.Property(x => x.PriceAdjust).HasColumnType("decimal(18,2)").HasDefaultValue(0);
                b.Property(x => x.Quantity).HasDefaultValue(0);
                b.Property(x => x.IsAvailable).HasDefaultValue(true);
                b.Property(x => x.CreatedAt).HasColumnType("datetime2(0)").HasDefaultValueSql("SYSUTCDATETIME()");

                b.HasOne(x => x.Product).WithMany(p => p.ProductVariants).HasForeignKey(x => x.ProductId).HasConstraintName("FK_ProductVariants_Products");

                b.HasIndex(x => x.ProductId).HasDatabaseName("IX_ProductVariants_ProductId");

                b.HasCheckConstraint("CK_ProductVariants_PriceAdjust", "[PriceAdjust] >= 0");
                b.HasCheckConstraint("CK_ProductVariants_Quantity", "[Quantity] >= 0");
            });
        }
    }
}
