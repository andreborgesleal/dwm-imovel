using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("EsteiraContabilizacao")]
    public class EsteiraContabilizacao
    {
        [Key, Column(Order = 0)]
        [DisplayName("ID")]
        public int esteiraId { get; set; }

        [Key, Column(Order = 1)]
        [DisplayName("Arquivo")]
        public string arquivo { get; set; }
    }
}