using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace istEncuestasMVC.Models
{
    [Serializable]
    [XmlRoot("Empresa"), XmlType("Empresa")]
    public class Empresa
    {
        [Display(Name = "Razon Social", Prompt = "Razon Social")]
        public string NombreEmpresa { get; set; }
        [Required(ErrorMessage = "Rut Empresa requerido.", AllowEmptyStrings = false)]
        [Display(Name = "Rut Empresa", Prompt = "Rut Empresa")]
        public string RutEmpresa { get; set; }

        [Display(Name = "Nombre", Prompt = "Nombre")]
        [Required(ErrorMessage = "Nombre requerido.", AllowEmptyStrings = false)]
        public string RepNombre { get; set; }
        [Display(Name = "Rut", Prompt = "Rut")]
        [Required(ErrorMessage = "Rut requerido.", AllowEmptyStrings = false)]
        public string RepRut { get; set; }
        [Display(Name = "Cargo", Prompt = "Cargo")]
        [Required(ErrorMessage = "Cargo requerido.", AllowEmptyStrings = false)]
        public string RepCargo { get; set; }
        [Display(Name = "Telefono", Prompt = "Telefono")]
        [Required(ErrorMessage = "Telefono requerido.", AllowEmptyStrings = false)]
        public string RepTelefono { get; set; }
        [Required(ErrorMessage = "Email requerido.", AllowEmptyStrings = false)]
        [Display(Name = "E-Mail", Prompt = "E-Mail")]
        [EmailAddress(ErrorMessage = "Dirección e-mail inválida")]
        public string RepEmail { get; set; }

        public int Codigo { get; set; }
        public string GlsError { get; set; }
        public string Campo { get; set; }
        
    }
}