using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;
using System;
using System.Collections.Generic;
using System.IO;

namespace DWM.Models.Repositories
{
    public class EsteiraContabilizacaoViewModel : Repository
    {
        [DisplayName("ID")]
        public int esteiraId { get; set; }

        [DisplayName("Arquivo")]
        public string arquivo { get; set; }

        public string nome_original { get; set; }

        public string extensao
        {
            get
            {
                System.IO.FileInfo f = new System.IO.FileInfo(Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Users_Data"), nome_original));
                return f.Extension.ToLower().Replace(".","");
            }
        }
    }
}