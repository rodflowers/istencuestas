using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace istEncuestasMVC.Models
{
    public class Login
    {
        [Required(ErrorMessage ="Username requerido.", AllowEmptyStrings = false )]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password requerida.", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}