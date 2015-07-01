using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    
    [Table("Cliente")]
    public class Cliente
    {
        [Key]
        [DisplayName("ID")]
        public int clienteId { get; set; }

        [DisplayName("Nome")]
        public string nome { get; set; }

        [DisplayName("Ind Tipo Pessoa")]
        public string ind_tipo_pessoa { get; set; }

        [DisplayName("CPF/CNPJ")]
        public string cpf_cnpj { get; set; }

        [DisplayName("Dt_Inclusão")]
        public DateTime dt_inclusao { get; set; }

        [DisplayName("Dt_Alteração")]
        public DateTime dt_alteracao { get; set; }

        [DisplayName("Endereço")]
        public string endereco { get; set; }

        [DisplayName("Copmlemento")]
        public string complemento { get; set; }

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

        [DisplayName("Celular 3")]
        public string fone3 { get; set; }

        [DisplayName("Email")]
        public string email { get; set; }

        [DisplayName("Sexo")]
        public string sexo { get; set; }

        [DisplayName("Dt_Nascimento")]
        public Nullable<DateTime> dt_nascimento { get; set; }

        [DisplayName("Observação")]
        public string observacao { get; set; }
    }
}