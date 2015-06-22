using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;
using System;
using System.Collections.Generic;
using DWM.Models.Entidades;

namespace DWM.Models.Repositories
{
    public class EmpreendimentoViewModel : Repository
    {
        [Key]
        [DisplayName("ID")]
        public int empreendimentoId { get; set; }

        [DisplayName("Nome do Empreendimento")]
        [Required(ErrorMessage = "Por favor, informe o nome do empreendimento")]
        [StringLength(60, ErrorMessage = "O nome do empreendimento deve ter no máximo 60 caracteres")]
        public string nomeEmpreend { get; set; }

        [DisplayName("Coordenador")]
        public int usuarioId { get; set; }

        public string nome { get; set; }

        public string login { get; set; }

    }
}