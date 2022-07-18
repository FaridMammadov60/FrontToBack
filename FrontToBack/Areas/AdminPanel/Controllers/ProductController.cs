using FrontToBack.DAL;
using FrontToBack.Extentions;
using FrontToBack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            return View(_context.Products.Include(p => p.Category).ToList());
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return NotFound();
            Product dbProduct = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
            if (dbProduct == null) return NotFound();
            return View(dbProduct);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.Categories = _context.Categories.ToList();

            if (product.Photo == null)
            {
                ModelState.AddModelError("Photo", "doldur");
                return View();
            }
            if (!product.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Sekil doldur");
                return View();
            }
            if (product.Photo.ValidSize(200))
            {
                ModelState.AddModelError("Photo", "Olcu oversize");
                return View();
            }
            ViewBag.Categories = _context.Categories.ToList();


            Product newProduct = new Product
            {
                Price = product.Price,
                Name = product.Name,
                CategoryId = product.CategoryId,
                ImageUrl = product.Photo.SaveImage(_env, "img"),
            };
            await _context.Products.AddAsync(newProduct);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null) return NotFound();
            Product dbProd = await _context.Products.FindAsync(Id);
            if (dbProd == null) return NotFound();

            string path = Path.Combine(_env.WebRootPath, "img", dbProd.ImageUrl);

            Helper.Helper.DeleteImage(path);

            _context.Products.Remove(dbProd);
            await _context.SaveChangesAsync();

            return RedirectToAction("index");
        }

        public async Task<IActionResult> Update(int? Id)
        {
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
            if (Id == null) return NotFound();
            Product dbProd = await _context.Products.FindAsync(Id);
            if (dbProd == null) return BadRequest();
            return View(dbProd);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Product product)
        {
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
            if (!ModelState.IsValid)
            {
                return View();
            }
            Product dbProd = await _context.Products.FindAsync(product.Id);
            if (dbProd == null)
            {
                return View();
            }
            else
            {
                Product dbProductName = await _context.Products.FirstOrDefaultAsync(p => p.Name.Trim().ToLower() == product.Name.Trim().ToLower());
                if (dbProductName != null)
                {
                    if (dbProductName.Name.Trim().ToLower() != dbProd.Name.Trim().ToLower())
                    {
                        ModelState.AddModelError("Name", "with this name product allready exist!!!");
                        return View();
                    }
                }

                if (product.Photo == null)
                {
                    dbProd.ImageUrl = dbProd.ImageUrl;
                }
                else
                {
                    if (!product.Photo.IsImage())
                    {
                        ModelState.AddModelError("Photo", "Choose the photo");
                        return View();
                    }
                    if (product.Photo.ValidSize(200))
                    {
                        ModelState.AddModelError("Photo", "oversize");
                        return View();
                    }
                    string oldPhoto = dbProd.ImageUrl;
                    string path = Path.Combine(_env.WebRootPath, "img", oldPhoto);
                    dbProd.ImageUrl = product.Photo.SaveImage(_env, "img");

                    Helper.Helper.DeleteImage(path);
                }

                dbProd.Name = product.Name;
                dbProd.Price = product.Price;
                dbProd.CategoryId = product.CategoryId;
                dbProd.Count = product.Count;
                await _context.SaveChangesAsync();

            }
            return RedirectToAction("index");

        }
    }
}
