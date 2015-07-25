using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;
using System;

namespace DWM.Models.Repositories
{
    public class PropostaViewModel : Repository
    {
        [DisplayName("ID")]
        public int propostaId { get; set; }

        [DisplayName("Empreendimento")]
        [Required(ErrorMessage = "Por favor, informe o empreendimento")]
        public int empreendimentoId { get; set; }

        public string descricao_empreendimento { get; set; }

        [DisplayName("Cliente")]
        [Required(ErrorMessage = "Por favor, informe o cliente")]
        public int clienteId { get; set; }

        public string nome_cliente { get; set; }

        public string cpf_cnpj { get; set; }

        [DisplayName("Data da Proposta")]
        [Required(ErrorMessage = "Por favor, informe a data da proposta")]
        public DateTime dt_proposta { get; set; }

        [DisplayName("Torre")]
        [Required(ErrorMessage="Por favor, informe a Torre")]
        [StringLength(25, ErrorMessage = "A Torre deve ter no máximo 25 caracteres")]
        public string torre { get; set; }

        [DisplayName("Unidade")]
        [Required(ErrorMessage = "Por favor, informe a Unidade")]
        [StringLength(4, ErrorMessage = "A unidade deve ter no máximo 4 caracteres")]
        public string unidade { get; set; }

        [DisplayName("Valor")]
        [Required(ErrorMessage = "Por favor, informe o valor geral da venda")]
        public Decimal valor { get; set; }

        [DisplayName("Comissão")]
        [Required(ErrorMessage = "Por favor, informe o valor da comissão")]
        public Decimal vr_comissao { get; set; }

        [DisplayName("Etapa ID")]
        public int etapaId { get; set; }

        [DisplayName("Etapa")]
        public string descricao_etapa { get; set; }

        [DisplayName("Último Status")]
        public DateTime dt_ultimo_status { get; set; }

        [DisplayName("Operação ID")]
        public Nullable<int> operacaoId { get; set; }

        [DisplayName("Corretor")]
        public Nullable<int> corretor1Id { get; set; }

        public string nome_corretor1 { get; set; }

        [DisplayName("Corretor 2")]
        public Nullable<int> corretor2Id { get; set; }

        public string nome_corretor2 { get; set; }
        
        [DisplayName("Gerente")]
        [Required(ErrorMessage="Gerente deve ser informado")]
        public int usuarioId { get; set; }

        [DisplayName("Gerente")]
        public string nome { get; set; }

        [DisplayName("Login")]
        public string login { get; set; }

        public double percent_atual { get; set; }

        public double percent_restnte { get; set; }

        public string situacao { get; set; }
    }
}

