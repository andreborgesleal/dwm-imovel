using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

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

        public string nome_coordenador { get; set; }

        public string login_coordenador { get; set; }

        [DisplayName("Cliente")]
        [Required(ErrorMessage = "Por favor, informe o cliente")]
        public int clienteId { get; set; }

        public string nome_cliente { get; set; }

        public string cpf_cnpj { get; set; }

        public string fone1 { get; set; }

        public string fone2 { get; set; }

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

        public Nullable<int> qte_dias_esteira { get; set; }

        [DisplayName("Operação ID")]
        public Nullable<int> operacaoId { get; set; }

        [DisplayName("Corretor")]
        public Nullable<int> corretor1Id { get; set; }

        public string nome_corretor1 { get; set; }

        public string fone_corretor1 { get; set;}

        [DisplayName("Corretor 2")]
        public Nullable<int> corretor2Id { get; set; }

        public string nome_corretor2 { get; set; }

        public string fone_corretor2 { get; set; }
        
        [DisplayName("Gerente")]
        [Required(ErrorMessage="Gerente deve ser informado")]
        public int usuarioId { get; set; }

        [DisplayName("Gerente")]
        public string nome { get; set; }

        [DisplayName("Login")]
        public string login { get; set; }

        public IEnumerable<EsteiraViewModel> Esteira { get; set; }

        public IPagedList Comentarios { get; set; }

        public IEnumerable<EsteiraContabilizacaoViewModel> Arquivos { get; set; }

        public IEnumerable<EsteiraComissaoViewModel> Comissao { get; set; }

        public double percent_atual { get; set; }

        public double percent_restnte { get; set; }

        public string situacao { get; set; }

        public string ind_fechamento { get; set; }
    }
}

