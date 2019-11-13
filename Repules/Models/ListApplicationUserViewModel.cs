using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Repules.Models
{
    public class ListApplicationUserViewModel
    {
        [Display(Name = "Id")]
        public Guid UserId { get; set; }

        [Display(Name = "Felhasználónév")]
        public string UserName { get; set; }

        [Display(Name = "E-mail cím")]
        public string Email { get; set; }
    }
}
