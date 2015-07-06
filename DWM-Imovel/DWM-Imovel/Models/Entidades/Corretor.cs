using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("Corretor")]
    public class Corretor
    {
        [Key]
        [DisplayName("CorretorId")]
        public int corretorId { get; set; }

        public string nome { get; set; }

        public string cpf { get; set; }

        public string rg { get; set; }

        public string orgao_emissor { get; set; }

        public string creci { get; set; }

        public string email { get; set; }

        public string endereco { get; set; }

        public string complemento_end { get; set; }

        public string cidade { get; set; }

        public string uf { get; set; }

        public string cep { get; set; }

        public string bairro { get; set; }

        public string fone1 { get; set; }

        public string fone2 { get; set; }
    }
}