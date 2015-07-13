using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;
using System;

namespace DWM.Models.Repositories
{
    public class EtapaViewModel : Repository
    {
        [DisplayName("ID")]
        public int etapaId { get; set; }

        [DisplayName("EmpreendimentoID")]
        public int empreendimentoId { get; set; }

        public string descricao_empreendimento { get; set; }

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