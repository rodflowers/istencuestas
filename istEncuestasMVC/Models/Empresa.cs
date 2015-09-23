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
        public string NombreEmpresa { get; set; }
        [Required(ErrorMessage = "RutEmpresa requerido.", AllowEmptyStrings = false)]
        public string RutEmpresa { get; set; }

        public int Codigo { get; set; }
        public string GlsError { get; set; }
        public string Campo { get; set; }
        
    }
}