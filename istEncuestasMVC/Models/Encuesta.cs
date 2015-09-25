using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace istEncuestasMVC.Models
{
    /// <summary>  
    /// This class is being serialized to XML.  
    /// </summary>  
    [Serializable]
    [XmlRoot("Encuesta"), XmlType("Encuesta")]
    public class Encuesta
    {
        public int Cod_Num { get; set; }
        public int Num_Orden { get; set; }
        public string Cod_Par_Acceso { get; set; }
        public string Val_Par_Alf_Num { get; set; }        
        public string Gls_Pregunta { get; set; }
        public string Gls_Par_Alf_Num { get; set; }
    }
}