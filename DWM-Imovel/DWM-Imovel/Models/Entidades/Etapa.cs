using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("Etapa")]
    public class Etapa
    {
        [Key]
        [DisplayName("ID")]
        public int etapaId { get; set; }

        [DisplayName("EmpreendimentoID")]
        public int empreendimentoId { get; set; }

        [DisplayName("Prox_EtapaID")]
        public Nullable<int> etapa_proxId { get; set; }

        [DisplayName("Ant_EtapaID")]
        public Nullable<int> etapa_antId { get; set; }

        [DisplayName("IDX")]
        public int idx { get; set; }

        [DisplayName("Descricao")]
        public string descricao { get; set; }
    }
}