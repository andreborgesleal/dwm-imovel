using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("Esteira")]
    public class Esteira
    {
        [Key]
        [DisplayName("ID")]
        public int esteiraId { get; set; }

        [DisplayName("PropostaID")]
        public int propostaId { get; set; }

        [DisplayName("Dt_Evento")]
        public DateTime dt_evento { get; set; }

        [DisplayName("EtapaID")]
        public int etapaId { get; set; }

        [DisplayName("Dt_Ocorrencia")]
        public DateTime dt_ocorrencia { get; set; }

        [DisplayName("ind_aprovacao")]
        public string ind_aprovacao { get; set; }

        [DisplayName("Observacao")]
        public string observacao { get; set; }

        [DisplayName("UsuarioId")]
        public Nullable<int> usuarioId { get; set; }

        [DisplayName("Nome")]
        public string nome { get; set; }

        [DisplayName("Login")]
        public string login { get; set; }

        public virtual Proposta proposta { get; set; }
    }
}