using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("Proposta")]
    public class Proposta
    {
        [Key]
        [DisplayName("ID")]
        public int propostaId { get; set; }

        public int empreendimentoId { get; set; }

        public int clienteId { get; set; }

        public DateTime dt_proposta { get; set; }

        public string torre { get; set; }

        public string unidade { get; set; }

        public Decimal valor { get; set; }

        public Decimal vr_comissao { get; set; }

        public int etapaId { get; set; }

        public DateTime dt_ultimo_status { get; set; }

        public Nullable<int> operacaoId { get; set; }

        public Nullable<int> corretor1Id { get; set; }

        public Nullable<int> corretor2Id { get; set; }

        public int usuarioId { get; set; }

        public string nome { get; set; }

        public string login { get; set; }

        public string situacao { get; set; }

    }
}