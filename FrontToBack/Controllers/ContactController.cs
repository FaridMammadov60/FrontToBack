using FrontToBack.DAL;
using FrontToBack.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Controllers
{  
    public class ContactController : Controller
    {

        private readonly AppDbContext _context;
        public ContactController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Contact> contacts = _context.Contacts.ToList();
            return View(contacts);
        }
       
       
        public IActionResult Send()
        {
            return View();
        }
        [HttpPost]
        
        public IActionResult Send([FromForm] Contact contact)
        {
            Contact dbContact = new Contact();
            dbContact.Id = contact.Id;
            dbContact.Name = contact.Name;
            dbContact.Phone = contact.Phone;
            dbContact.Email = contact.Email;
            dbContact.Subject = contact.Subject;
            dbContact.Message = contact.Message;

            _context.Contacts.Add(dbContact);
            _context.SaveChanges();

            return Ok();
        }
    }
}
