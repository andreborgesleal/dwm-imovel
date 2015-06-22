using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("Empreendimento")]
    public class Empreendimento
    {
        [Key]
        [DisplayName("ID")]
        public int empreendimentoId { get; set; }

        public string nomeEmpreend { get; set; }

        public int usuarioId { get; set; }

        public string nome { get; set; }

        public string login { get; set; }
    }
}