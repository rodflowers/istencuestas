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
        [Display(Name = "Nombre Empresa", Prompt = "Nombre Empresa")]
        public string NombreEmpresa { get; set; }
        [Required(ErrorMessage = "Rut Empresa requerido.", AllowEmptyStrings = false)]
        [Display(Name = "Razon Social", Prompt = "Razon Social")]
        public string RutEmpresa { get; set; }

        [Display(Name = "Nombre", Prompt = "Nombre")]
        public string RepNombre { get; set; }
        [Display(Name = "Rut", Prompt = "Rut")]
        public string RepRut { get; set; }
        [Display(Name = "Cargo", Prompt = "Cargo")]
        public string RepCargo { get; set; }
        [Display(Name = "Telefono", Prompt = "Telefono")]
        public string RepTelefono { get; set; }
        [Required(ErrorMessage = "Email requerido.", AllowEmptyStrings = false)]
        [Display(Name = "E-Mail", Prompt = "E-Mail")]
        public string RepEmail { get; set; }

        public int Codigo { get; set; }
        public string GlsError { get; set; }
        public string Campo { get; set; }
        
    }
}