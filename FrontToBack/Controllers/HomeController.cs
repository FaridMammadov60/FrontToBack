using FrontToBack.DAL;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FrontToBack.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Slider> sliders = _context.Sliders.ToList();
            HomeVM homeVM = new HomeVM();
            homeVM.Sliders = sliders;
            homeVM.SliderContent = _context.SliderContents.FirstOrDefault();
            homeVM.Categories = _context.Categories.ToList();
            homeVM.Products = _context.Products.Include(p => p.Category).ToList();
            homeVM.Abouts = _context.Abouts.ToList();
            homeVM.Experts = _context.Experts.ToList();
            homeVM.Blogs = _context.Blogs.ToList();
            homeVM.Instagrams = _context.Instagrams.ToList();
            homeVM.Says = _context.Says.ToList();


            return View(homeVM);
        }
        public IActionResult Detail(int? id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            Product dbProduct = _context.Products.FirstOrDefault(p => p.Id == id);
            if (dbProduct == null)
            {
                return NotFound();
            }
            return View(dbProduct);
        }
        public IActionResult SearchProduct(string search)
        {
            List<Product> products = _context.Products
                .Include(p=>p.Category)
                .OrderBy(p => p.Id)                
                .Where(p=>p.Name.ToLower()                
                .Contains(search.ToLower()))
                .Take(10)
                .ToList();
            return PartialView("_SearchPartial", products);
        }
    }
}
