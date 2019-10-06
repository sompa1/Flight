using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Repules.Models
{
    public class ListApplicationUserViewModel
    {
        public Guid UserId { get; set; }

        [Display(Name = "Felhasználó neve")]
        public string UserName { get; set; }

        [Display(Name = "Felhasználó e-mail címe")]
        public string Email { get; set; }
    }
}
