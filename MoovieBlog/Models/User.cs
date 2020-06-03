using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MoovieBlog.Models
{
    public class User:IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string RegisterDate { get; set; }
    }
}
