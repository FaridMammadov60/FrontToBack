using FrontToBack.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FrontToBack.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SliderContent> SliderContents { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Experts> Experts { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Instagram> Instagrams { get; set; }
        public DbSet<Say> Says { get; set; }
        public DbSet<Bio> Bio { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SalesProduct> SalesProducts { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);  
        //    modelBuilder.Entity<Bio>().HasData(
        //        new Bio
        //        {
        //            Id = 1,
        //            ImageUrl = "logo.png",
        //            AuthorName = "Farid Mammadov",
        //            Linkedin = "www.linkedin.com",
        //            Facebook = "www.facebook.com",
        //        }
        //    );
        //    modelBuilder.Entity<Blog>().HasData(
        //        new Blog
        //        {
        //            Id = 1,
        //            Title = "From our Blog",
        //            Desc = "A perfect blend of creativity, energy, communication, happiness and love.Let us arrange  a smile for you"
        //        }
        //    ); 
        //    modelBuilder.Entity<Slider>().HasData(
        //        new Slider
        //        {
        //            Id = 1,
        //            ImageUrl = "h3-slider-background.jpg"
        //        },
        //        new Slider
        //        {
        //            Id = 2,
        //            ImageUrl = "h3-slider-background-2.jpg"
        //        },
        //        new Slider
        //        {
        //            Id = 3,
        //            ImageUrl = "h3-slider-background-3.jpg"
        //        }
        //    );           
        //}
    }   
}