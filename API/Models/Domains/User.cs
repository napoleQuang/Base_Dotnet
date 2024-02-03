using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace API.Models.Domains
{
    public class User : IdentityUser
    {
        public string? ProfilePic { get; set; }
        
    }
}
