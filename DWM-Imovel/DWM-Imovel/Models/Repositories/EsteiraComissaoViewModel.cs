using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;
using System;
using System.Collections.Generic;

namespace DWM.Models.Repositories
{
    public class EsteiraComissaoViewModel : Repository
    {
        [DisplayName("ID")]
        public int esteiraId { get; set; }

        [DisplayName("Grupo ID")]
        public int grupoId { get; set; }

        [DisplayName("Dscrição")]
        public string nome_grupo { get; set; }

        [DisplayName("Valor")]
        [Required(ErrorMessage="Valor da comissão deve ser informado")]
        public decimal valor { get; set; }

        [DisplayName("Usuario ID")]
        public Nullable<int> usuarioId { get; set; }

        [DisplayName("Nome")]
        public string nome { get; set; }

        [DisplayName("Login")]
        public string login { get; set; }
    }
}