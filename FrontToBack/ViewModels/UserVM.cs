using FrontToBack.Models;
using System.Collections.Generic;

namespace FrontToBack.ViewModels
{
    public class UserVM
    {
        public List<AppUser> Users { get; set; }
        public IList<string> userRoles { get; set; }
    }
}
