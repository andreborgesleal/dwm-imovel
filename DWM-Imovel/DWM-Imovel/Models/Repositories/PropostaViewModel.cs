using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;
using System;
using System.Collections.Generic;
using DWM.Models.Entidades;

namespace DWM.Models.Repositories
{
    public class PropostaViewModel : Repository
    {
        [Key]
        [DisplayName("ID")]
        public int propostaId { get; set; }

        [DisplayName("Empreendimento ID")]
        [Required(ErrorMessage = "Por favor, informe o id do empreendimento")]
        public int empreendimentoId { get; set; }

        [DisplayName("Cliente ID")]
        [Required(ErrorMessage = "Por favor, informe o id do cliente")]
        public int clienteId { get; set; }

        [DisplayName("Data da Proposta")]
        [Required(ErrorMessage = "Por favor, informe o nome do cliente")]
        public DateTime dt_proposta { get; set; }

        [DisplayName("Unidade")]
        [StringLength(50, ErrorMessage = "A unidade deve ter no máximo 4 caracteres")]
        public string unidade { get; set; }

        [DisplayName("Modelo")]
        [StringLength(50, ErrorMessage = "O modelo deve ter no máximo 50 caracteres")]
        public string modelo { get; set; }

        [DisplayName("Valor")]
        [Required(ErrorMessage = "Por favor, informe o valor da proposta")]
        public Decimal valor { get; set; }

        [DisplayName("Valor da Comissão")]
        [Required(ErrorMessage = "Por favor, informe o valor da comissão")]
        public Decimal vr_comissao { get; set; }

        [DisplayName("Etapa ID")]
        [Required(ErrorMessage = "Por favor, informe o id da etapa")]
        public int etapaId { get; set; }

        [DisplayName("Data do último status")]
        [Required(ErrorMessage = "Por favor, informe a data do último status")]
        public DateTime dt_ultimo_status { get; set; }

        [DisplayName("Operação ID")]
        public int operacaoId { get; set; }
    }
}