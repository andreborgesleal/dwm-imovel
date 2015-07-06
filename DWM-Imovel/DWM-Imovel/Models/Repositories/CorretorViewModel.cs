using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;
using System;
using System.Collections.Generic;
using DWM.Models.Entidades;

namespace DWM.Models.Repositories
{
    public class CorretorViewModel : Repository
    {
        [Key]
        [DisplayName("CorretorId")]
        public int corretorId { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "Nome do credor deve ser informado")]
        [StringLength(40, ErrorMessage = "Nome do cliente deve ter no mínimo 5 e no máximo 40 caracteres", MinimumLength = 5)]
        public string nome { get; set; }

        [DisplayName("CPF")]
        public string cpf { get; set; }

        [DisplayName("RG")]
        public string rg { get; set; }

        [DisplayName("Órgão Emissor")]
        public string orgao_emissor { get; set; }

        [DisplayName("CRECI")]
        public string creci { get; set; }

        [DisplayName("Email")]
        public string email { get; set; }

        [DisplayName("Endereço")]
        [StringLength(50, ErrorMessage = "Endereço deve ter no máximo 50 caracteres")]
        public string endereco { get; set; }

        [DisplayName("Complemento")]
        [StringLength(30, ErrorMessage = "Complemento deve ter no máximo 30 caracteres")]
        public string complemento_end { get; set; }

        [DisplayName("Cidade")]
        public string cidade { get; set; }

        [DisplayName("UF")]
        public string uf { get; set; }

        [DisplayName("CEP")]
        public string cep { get; set; }

        [DisplayName("Bairro")]
        public string bairro { get; set; }

        [DisplayName("Celular 1")]
        public string fone1 { get; set; }

        [DisplayName("Celular 2")]
        public string fone2 { get; set; }
    }
}