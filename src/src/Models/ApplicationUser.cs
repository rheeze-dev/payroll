using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace src.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [StringLength(250)]
        [Display(Name = "Profile Picture")]
        public string ProfilePictureUrl { get; set; } = "/images/empty-profile.png";

        [StringLength(250)]
        [Display(Name = "Wallpaper Picture")]
        public string WallpaperPictureUrl { get; set; } = "/images/wallpaper1.jpg";

        public string IdNumber { get; set; }

        public string Role { get; set; }

        public bool IsSuperAdmin { get; set; } = false;

        [Display(Name = "Position")]
        public string Position { get; set; }

        [Display(Name = "Basic Pay")]
        public int BasicPay { get; set; }

        [Display(Name = "Birth Date")]
        public string BirthDate { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }
    }
}
