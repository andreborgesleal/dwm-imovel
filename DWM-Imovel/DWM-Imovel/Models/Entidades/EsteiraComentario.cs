using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("EsteiraComentario")]
    public class EsteiraComentario
    {
        [Key, Column(Order = 0)]
        [DisplayName("ID")]
        public int esteiraId { get; set; }

        [Key, Column(Order = 1)]
        [DisplayName("Dt_Comentario")]
        public DateTime dt_comentario{ get; set; }

        [DisplayName("Observacao")]
        public string observacao { get; set; }

        [DisplayName("Usuario")]
        public int usuarioId { get; set; }

        [DisplayName("Nome")]
        public string nome { get; set; }

        [DisplayName("Login")]
        public string login { get; set; }
    }
}