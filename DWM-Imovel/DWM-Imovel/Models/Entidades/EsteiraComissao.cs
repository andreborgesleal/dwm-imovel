using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("EsteiraComissao")]
    public class EsteiraComissao
    {
        [Key, Column(Order = 0)]
        [DisplayName("ID")]
        public int esteiraId { get; set; }

        [Key, Column(Order = 1)]
        [DisplayName("GrupoID")]
        public int grupoId { get; set; }

        [DisplayName("Nome")]
        public string nome_grupo { get; set; }

        [DisplayName("Valor")]
        public decimal valor { get; set; }

    }
}