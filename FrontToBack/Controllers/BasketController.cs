using FrontToBack.DAL;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FrontToBack.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;
        public BasketController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> AddItem(int? id)
        {
            //HttpContext.Session.SetString("name", "Farid");
            //Response.Cookies.Append("GROUP", "P322", new CookieOptions { MaxAge=TimeSpan.FromDays(14)});
            if (id == null) return NotFound();
            Product dbProduct = await _context.Products.FindAsync(id);
            if (dbProduct == null) return NotFound();

            List<BasketVM> products;

            if (Request.Cookies["basket"] == null)
            {
                products = new List<BasketVM>();
            }
            else
            {
                products = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            }
            BasketVM existProduct = products.Find(x => x.Id == id);
            if (existProduct == null)
            {
                BasketVM basketVM = new BasketVM()
                {
                    Id = dbProduct.Id,
                    Name = dbProduct.Name,
                    ImageUrl = dbProduct.ImageUrl,
                    Price = dbProduct.Price,
                    CategoryId = dbProduct.CategoryId,
                    ProductCount = 1
                };
            }
            else
            {
                existProduct.ProductCount++;
            }           

            
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products), new CookieOptions { MaxAge = TimeSpan.FromDays(14) });

            return RedirectToAction("index", "home");
        }
        public IActionResult ShowItem()
        {
            //string name= HttpContext.Session.GetString("name");
            //string group=Request.Cookies["GROUP"];
            string basket = Request.Cookies["basket"];
            List<BasketVM> p = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            return Json(p);
        }
    }
}
