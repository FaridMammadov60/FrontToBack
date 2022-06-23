﻿using FrontToBack.Models;
using Microsoft.EntityFrameworkCore;

namespace FrontToBack.DAL
{
    public class AppDbContext : DbContext
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
    }
}
