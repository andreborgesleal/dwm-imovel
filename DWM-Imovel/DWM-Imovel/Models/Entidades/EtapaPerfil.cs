using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("EtapaPerfil")]
    public class EtapaPerfil
    {
        [Key, Column(Order = 0)]
        [DisplayName("EtapaID")]
        public int etapaId { get; set; }

        [Key, Column(Order=1)]
        [DisplayName("GrupoID")]
        public int grupoId { get; set; }

        [DisplayName("Grupo")]
        public string nome_grupo { get; set; }
    }
}