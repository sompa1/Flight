using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Repules.Models
{
    public class CreateApplicationUserViewModel
    {
        [Display(Name = "Felhasználó név")]
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Display(Name = "Jelszó")]
        [Required]
        public string Password { get; set; }

        [Display(Name = "Admin?")]
        public bool IsAdmin { get; set; }
    }
}
