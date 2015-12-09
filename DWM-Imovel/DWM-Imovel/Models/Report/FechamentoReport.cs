using App_Dominio.Component;
using DWM.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DWM.Models.Report
{
    public class FechamentoReport : ReportViewModel<FechamentoMesViewModel>
    {
        #region Métodos da classe ReportRepository
        public override IEnumerable<FechamentoMesViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int? empreendimentoId = (int?)param[2];
            DateTime dt1 = Convert.ToDateTime(param[0].ToString());
            DateTime dt2 = Convert.ToDateTime(param[1].ToString());

            totalizaColuna1 = param[3].ToString();
            totalizaColuna2 = param[4].ToString();

            #region LINQ
            var q = (from p in db.Propostas
                     join c in db.Clientes on p.clienteId equals c.clienteId
                     join emp in db.Empreendimentos on p.empreendimentoId equals emp.empreendimentoId
                     where p.ind_fechamento == "S" 
                           && (!empreendimentoId.HasValue || p.empreendimentoId == empreendimentoId)
                           && p.dt_ultimo_status >= dt1 && p.dt_ultimo_status <= dt2
                     orderby p.empreendimentoId, p.dt_ultimo_status
                     select new FechamentoMesViewModel
                     {
                         empresaId = sessaoCorrente.empresaId,
                         clienteId = p.clienteId,
                         cpf_cnpj = c.cpf_cnpj,
                         nome_cliente = c.nome,
                         propostaId = p.propostaId,
                         empreendimentoId = p.empreendimentoId,
                         _empreendimentoId = p.empreendimentoId,
                         descricao_empreendimento = emp.nomeEmpreend,
                         dt_proposta = p.dt_proposta,
                         unidade = p.unidade,
                         torre = p.torre,
                         valor = p.valor,
                         vr_comissao = p.vr_comissao,
                         etapaId = p.etapaId,
                         descricao_etapa = "Faturamento",
                         dt_ultimo_status = p.dt_ultimo_status,
                         dt_ocorrencia = p.dt_ultimo_status,
                         _dt_ocorrencia = p.dt_ultimo_status,
                         ind_fechamento = p.ind_fechamento,
                         operacaoId = p.operacaoId,
                         PageSize = pageSize,
                         TotalCount = (from p1 in db.Propostas
                                       join c1 in db.Clientes on p1.clienteId equals c1.clienteId
                                       join emp1 in db.Empreendimentos on p1.empreendimentoId equals emp1.empreendimentoId
                                       where p1.ind_fechamento != "S"
                                             && (!empreendimentoId.HasValue || p1.empreendimentoId == empreendimentoId)
                                             && p1.dt_ultimo_status >= dt1 && p1.dt_ultimo_status <= dt2
                                       orderby p1.empreendimentoId, p1.dt_ultimo_status 
                                       select p1.propostaId).Count()
                     }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
            #endregion

            return q;
        }

        public override Repository getRepository(Object id)
        {
            throw new NotImplementedException();
        }

        public override string action()
        {
            return "ListParam";
        }

        public override string DivId()
        {
            return "div-list-static";
        }

        public override IEnumerable<FechamentoMesViewModel> BindReport(params object[] param)
        {
            int? empreendimentoId = (int?)param[2];
            DateTime dt1 = Convert.ToDateTime(param[0].ToString());
            DateTime dt2 = Convert.ToDateTime(param[1].ToString());

            totalizaColuna1 = param[3].ToString();
            totalizaColuna2 = param[4].ToString();

            #region LINQ
            var q = (from p in db.Propostas
                     join c in db.Clientes on p.clienteId equals c.clienteId
                     join emp in db.Empreendimentos on p.empreendimentoId equals emp.empreendimentoId
                     where p.ind_fechamento == "S"
                           && (!empreendimentoId.HasValue || p.empreendimentoId == empreendimentoId)
                           && p.dt_ultimo_status >= dt1 && p.dt_ultimo_status <= dt2
                     orderby p.empreendimentoId, p.dt_ultimo_status
                     select new FechamentoMesViewModel
                     {
                         empresaId = sessaoCorrente.empresaId,
                         clienteId = p.clienteId,
                         cpf_cnpj = c.cpf_cnpj,
                         nome_cliente = c.nome,
                         propostaId = p.propostaId,
                         empreendimentoId = p.empreendimentoId,
                         _empreendimentoId = p.empreendimentoId,
                         descricao_empreendimento = emp.nomeEmpreend,
                         dt_proposta = p.dt_proposta,
                         unidade = p.unidade,
                         torre = p.torre,
                         valor = p.valor,
                         vr_comissao = p.vr_comissao,
                         etapaId = p.etapaId,
                         descricao_etapa = "Faturamento",
                         dt_ultimo_status = p.dt_ultimo_status,
                         dt_ocorrencia = p.dt_ultimo_status,
                         _dt_ocorrencia = p.dt_ultimo_status,
                         ind_fechamento = p.ind_fechamento,
                         operacaoId = p.operacaoId
                     }).ToList();
            #endregion

            return q;
        }
        #endregion
    }
}