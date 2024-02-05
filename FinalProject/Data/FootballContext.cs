using Microsoft.EntityFrameworkCore;
using Football.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Football.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public new DbSet<User> Users { get; set; }
        public DbSet<UserField> UserFields { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurarea relațiilor

            // Relație One-to-Many între User și Booking
            modelBuilder.Entity<User>()
                        .HasMany(u => u.Bookings)
                        .WithOne(b => b.User)
                        .HasForeignKey(b => b.UserId);

            // Relație Many-to-Many între User și Field prin UserField
            modelBuilder.Entity<UserField>()
                        .HasKey(uf => new { uf.UserId, uf.FieldId });

            modelBuilder.Entity<UserField>()
                        .HasOne(uf => uf.User)
                        .WithMany(u => u.UserFields)
                        .HasForeignKey(uf => uf.UserId);

            modelBuilder.Entity<UserField>()
                        .HasOne(uf => uf.Field)
                        .WithMany(f => f.UserFields)
                        .HasForeignKey(uf => uf.FieldId);

            // Relație One-to-Many între User și Review
            modelBuilder.Entity<User>()
                        .HasMany(u => u.Reviews)
                        .WithOne(r => r.User)
                        .HasForeignKey(r => r.UserId);

            // Configurarea relației one-to-one între User și Coupon
            modelBuilder.Entity<User>()
                .HasOne(u => u.Coupon)
                .WithOne(c => c.User)
                .HasForeignKey<Coupon>(c => c.UserId);


            // ...alte configurări specifice...

            base.OnModelCreating(modelBuilder);
        }
        
    }

}