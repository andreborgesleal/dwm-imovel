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

        public string nome { get; set; }

        public string cpf_cnpj { get; set; }

        public string telefone { get; set; }
    }
}