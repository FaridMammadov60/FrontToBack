using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FrontToBack.Models
{
    public class AppUser:IdentityUser
    {     
        public string FullName { get; set; }       
       
    }
}
