using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;
using System;
using System.Collections.Generic;

namespace DWM.Models.Repositories
{
    public class EsteiraContabilizacaoViewModel : Repository
    {
        [DisplayName("ID")]
        public int esteiraId { get; set; }

        [DisplayName("Arquivo")]
        public string arquivo { get; set; }
    }
}