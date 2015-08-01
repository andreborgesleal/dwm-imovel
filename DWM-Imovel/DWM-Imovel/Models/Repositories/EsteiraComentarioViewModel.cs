using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;
using System;
using System.Collections.Generic;

namespace DWM.Models.Repositories
{
    public class EsteiraComentarioViewModel : Repository
    {
        [DisplayName("ID")]
        public int esteiraId { get; set; }

        [DisplayName("Data")]
        public DateTime dt_comentario { get; set; }

        [DisplayName("Observação")]
        [Required(ErrorMessage="Campo Observação deve ser preenchido")]
        [StringLength(4000, ErrorMessage="Campo Observação deve ter no máximo 4.000 caracteres")]
        public string observacao { get; set; }

        [DisplayName("Usuário")]
        public int usuarioId { get; set; }

        [DisplayName("Nome")]
        public string nome { get; set; }

        [DisplayName("Login")]
        public string login { get; set; }

        [DisplayName("Tempo Comentário")]
        public string tempo_comentario { get; set; }
    }
}