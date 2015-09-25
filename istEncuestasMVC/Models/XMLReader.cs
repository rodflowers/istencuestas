using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace istEncuestasMVC.Models
{
    public class XMLReader
    {
       /// <summary>
       /// 
       /// </summary>
       /// <returns></returns>               
        public List<Empresa> ReturnListOfEmpresa(System.Xml.XmlDocument xmlData2)
        {
            var xmlData = xmlData2.SelectSingleNode("//informacion");
            
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(new System.Xml.XmlNodeReader(xmlData));
          
            var newempresa = new List<Empresa>();
            newempresa = (from rows in ds.Tables[0].AsEnumerable()
                          select new Empresa
                          {
                              Codigo = Convert.ToInt32(rows[0].ToString()), //Convert row to int  
                              GlsError = rows[1].ToString(),
                              Campo = rows[2].ToString(),                             
                          }).ToList();

            return newempresa;
        }

        public List<Encuesta> RetrunListOfEncuesta()
        {
            string xmlData = HttpContext.Current.Server.MapPath("~/App_Data/SelectEncuesta.xml");//Path of the xml script  
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);
            var encuesta = new List<Encuesta>();
            encuesta = (from rows in ds.Tables[0].AsEnumerable()
                        select new Encuesta
                        {
                            Cod_Par_Acceso = rows[0].ToString(), //Convert row to int  
                            Gls_Par_Alf_Num = rows[1].ToString(),                           
                        }).ToList();
            return encuesta;
        }
    }
}