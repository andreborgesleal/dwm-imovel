using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("ComissaoDefault")]
    public class ComissaoDefault
    {
        [Key]
        [DisplayName("ID")]
        public int grupoId { get; set; }

        [DisplayName("Comissao")]
        public decimal vr_comissao { get; set; }

        [DisplayName("Nome")]
        public string nome_grupo { get; set; }

        [DisplayName("Source")]
        //Fonte de onde deverá ser recuperado o usuário beneficiário da comissão: 0-Empreendimento, 1-Proposta, 2-Corretor, 3-Imobiliária ou 4-Outros
        public int source { get; set; }
    }
}