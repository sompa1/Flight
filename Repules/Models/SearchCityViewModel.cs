using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Repules.Models
{
    public class SearchCityViewModel
    {
        // Annotations required to validate input data in our model.
        [Required(ErrorMessage = "Város nevet kell megadnia!")]
        //[RegularExpression("^[A-Za-z ]+$", ErrorMessage = "Csak szöveg engedélyezett!")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Vigyen be egy város nevet, ami hosszabb, mint 2 karakter, de rövidebb, mint 20!")]
        [Display(Name = "Város neve:")]
        public string CityName { get; set; }
    }
}
