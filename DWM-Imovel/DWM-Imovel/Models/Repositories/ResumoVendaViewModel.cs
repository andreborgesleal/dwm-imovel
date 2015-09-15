using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DWM.Models.Repositories
{
    public class ResumoVendaViewModel : Repository
    {
        public int? empreendimentoId { get; set; }
        public string nome_empreendimento { get; set; }
        public Nullable<decimal> vgv_areceber { get; set; }
        public Nullable<decimal> vgv_recebido { get; set; }
        public Nullable<decimal> total_comissao { get; set; }
        public string descricao_etapa { get; set; }
        public Nullable<int> quantidade { get; set; }
    }
}