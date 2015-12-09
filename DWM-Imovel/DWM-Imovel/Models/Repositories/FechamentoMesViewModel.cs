using App_Dominio.Component;
using App_Dominio.Contratos;
using DWM.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DWM.Models.Repositories
{
    public class FechamentoMesViewModel : PropostaViewModel, IReportRepository<FechamentoMesViewModel>
    {
        public DateTime? dt_ocorrencia { get; set; }
        public int? empreendimentoId { get; set; }
        public DateTime? _dt_ocorrencia { get; set; }
        public int? _empreendimentoId { get; set; }

        public decimal? valor { get; set; }
        public decimal? vr_comissao { get; set; }


        #region métodos da Interface
        public object getValueColumn1()
        {
            return empreendimentoId;
        }

        public object getValueColumn2()
        {
            return dt_ocorrencia;
        }

        public void ClearColumn1()
        {
            empreendimentoId = null;
        }

        public void ClearColumn2()
        {
            dt_ocorrencia = null;
        }

        public FechamentoMesViewModel getKey(object group = null, object subGroup = null)
        {
            return new FechamentoMesViewModel() { empreendimentoId = (int?)group, dt_ocorrencia = (DateTime?)subGroup};
        }

        public FechamentoMesViewModel Create(FechamentoMesViewModel key, IEnumerable<FechamentoMesViewModel> list)
        {
            FechamentoMesViewModel d = new FechamentoMesViewModel();

            if (key.empreendimentoId == null && key.dt_ocorrencia == null)
            {
                d.valor = list.Sum(m => m.valor);
                d.vr_comissao = list.Sum(m => m.vr_comissao);
                d.nome_cliente = "<b>Total geral:</b> ";
            }
            else if (key.dt_ocorrencia == null) // coluna 2 
            {
                d.valor = list.Where(info => info._empreendimentoId.Equals(key.empreendimentoId)).Sum(m => m.valor);
                d.vr_comissao = list.Where(info => info._empreendimentoId.Equals(key.empreendimentoId)).Sum(m => m.vr_comissao);
                d.nome_cliente = "<b>Total do dia: </b>"; // grupo
            }
            else if (key.empreendimentoId != null) // coluna 1 
            {
                d.valor = list.Where(info => info._empreendimentoId.Equals(key.empreendimentoId) && info._dt_ocorrencia == key.dt_ocorrencia).Sum(m => m.valor);
                d.vr_comissao = list.Where(info => info._empreendimentoId.Equals(key.empreendimentoId) && info._dt_ocorrencia == key.dt_ocorrencia).Sum(m => m.vr_comissao);
                d.nome_cliente = "<b>Total do empreendimento:</b> "; // sub-grupo
            }

            return d;
        }

        #endregion

    }
}