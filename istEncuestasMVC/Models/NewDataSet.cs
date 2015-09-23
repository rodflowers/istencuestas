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
    [XmlRoot("NewDataSet"), XmlType("NewDataSet")]
    public class NewDataSet
    {
        public string City { get; set; }
        public string State { get; set; }
        public int Zip { get; set; }
        public int Area_Code { get; set; }
        public string Time_Zone { get; set; }
    }
}