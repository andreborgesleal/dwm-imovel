using App_Dominio.Controllers;
using DWM.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using DWM.Models.Enumeracoes;
using DWM.Models.Report;
using App_Dominio.Repositories;
using App_Dominio.Security;

namespace DWM.Controllers
{
    public class FechamentoReportController : ReportController<FechamentoMesViewModel>
    {
        #region Herança
        public override int _sistema_id() { return (int)Sistema.DWMIMOVEL; }

        public override string getListName()
        {
            return "Fechamento do Mês";
        }
        #endregion

        public override ActionResult List(int? index, int? PageSize, string descricao = null)
        {
            return ListParam(index, PageSize);
        }

        [AuthorizeFilter]
        public ActionResult ListParam(int? index, int? pageSize = 50, string data1 = "", string data2 = "", int? empreendimentoId = null, string totalizaDia = "", string totalizaId = "")
        {
            if (ViewBag.ValidateRequest)
            {
                if (data1 == "")
                {
                    data1 = DateTime.Today.ToString("yyyy-MM-") + "01";
                    data2 = DateTime.Today.ToString("yyyy-MM-dd");
                }

                FechamentoReport d = new FechamentoReport();
                return this._List(index, pageSize, "Browse", d, data1, data2, empreendimentoId, totalizaDia, totalizaId);
            }
            else
                return View();
        }

        public FileResult PDF(string export, string data1 = "", string data2 = "", int? empreendimentoId = null, string descricao_emrpeendimento = "", string totalizaDia = "S", string totalizaId = "N")
        {
            ReportParameter[] p = new ReportParameter[5];
            // o parâmetro p[0] fica reservado para ser preenchido automaticamente com o nome da empresa
            p[1] = new ReportParameter("empreendimento", "Empreendimento: " + (descricao_emrpeendimento == "" ? "Todos" : descricao_emrpeendimento), false);
            p[2] = new ReportParameter("periodo", "Período: " + data1 + " à " + data2, false);
            p[3] = new ReportParameter("totalizaDia", totalizaDia, false);
            p[4] = new ReportParameter("totalizaId", totalizaId, false);

            return _PDF(export, "FechamentoMes", new FechamentoReport(), p, null, null, data1, data2, empreendimentoId, "N", "N");
        }
    }
}