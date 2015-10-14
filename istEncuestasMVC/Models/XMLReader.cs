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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlData2"></param>
        /// <returns></returns>
        public List<Encuesta> ReturnListOfEncuesta(System.Xml.XmlDocument xmlData2)
        {
            var xmlData = xmlData2.SelectSingleNode("//NewDataSet");

            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(new System.Xml.XmlNodeReader(xmlData));

            var newempresa = new List<Encuesta>();
            newempresa = (from rows in ds.Tables[0].AsEnumerable()
                          select new Encuesta
                          {
                              ID_ENCUESTA = rows[0].ToString(), //Convert row to int  
                              NOMBRE_ENCUESTA = rows[1].ToString(),                             
                          }).ToList();

            return newempresa;
        }

        public List<Encuesta> ReturnListOfSubFamilia(System.Xml.XmlDocument xmlData2)
        {
            var xmlData = xmlData2.SelectSingleNode("//NewDataSet");

            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(new System.Xml.XmlNodeReader(xmlData));

            var newempresa = new List<Encuesta>();
            newempresa = (from rows in ds.Tables[0].AsEnumerable()
                          select new Encuesta
                          {
                              ID_ENCUESTA = rows[0].ToString(), //Convert row to int  
                              ID_ENCUESTA_SUB = rows[1].ToString(),
                              NOM_ENCUESTA_SUB = rows[2].ToString(),
                              Cant_Preguntas = Convert.ToInt32(rows[3].ToString()),
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

        public List<Encuesta> RetrunListOfSubFamilia()
        {
            string xmlData = HttpContext.Current.Server.MapPath("~/App_Data/SubFamilia.xml");//Path of the xml script  
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);
            var subfamilia = new List<Encuesta>();
            subfamilia = (from rows in ds.Tables[0].AsEnumerable()
                        select new Encuesta
                        {
                            Cod_Par_Acceso = rows[0].ToString(), //Convert row to int  
                            Val_Par_Alf_Num = rows[1].ToString(),
                            Gls_Par_Alf_Num = rows[2].ToString(),
                            Cant_Preguntas = Convert.ToInt32(rows[3].ToString()),
                        }).ToList();
            return subfamilia;
        }

        public List<Encuesta> RetrunListOfDetSubFamilia()
        {
            string xmlData = HttpContext.Current.Server.MapPath("~/App_Data/DetalleSubFamilia.xml");//Path of the xml script  
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);
            var detsubfamilia = new List<Encuesta>();
            detsubfamilia = (from rows in ds.Tables[0].AsEnumerable()
                          select new Encuesta
                          {
                              Cod_Par_Acceso = rows[0].ToString(), //Convert row to int  
                              Val_Par_Alf_Num = rows[1].ToString(),
                              Cod_Num = Convert.ToInt32(rows[2].ToString()),
                              Num_Orden = Convert.ToInt32(rows[3].ToString()),
                              Gls_Pregunta = rows[4].ToString(),
                          }).ToList();
            return detsubfamilia;
        }
    }
}