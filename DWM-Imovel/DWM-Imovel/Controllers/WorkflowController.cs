using App_Dominio.Controllers;
using App_Dominio.Security;
using DWM.Models.Enumeracoes;
using DWM.Models.Persistence;
using DWM.Models.Repositories;
using System.Web.Mvc;
using DWM.Models.Entidades;
using System;


namespace DWM.Controllers
{
    public class WorkflowController : DwmRootController<PropostaViewModel, PropostaModel, ApplicationContext>
    {
        public override int _sistema_id() { return (int)Sistema.DWMIMOVEL; }
        public override string getListName()
        {
            return "Workflow";
        }

        #region List
        [AuthorizeFilter]
        public override ActionResult List(int? index, int? pageSize = 50, string nome = null)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Edit
        [AuthorizeFilter]
        public ActionResult Edit(int propostaId)
        {
            return _Edit(new PropostaViewModel() { propostaId = propostaId });
        }
        #endregion

        #region Incluir Comentário
        //public JsonResult JSonCrud()
        //{
        //    R result = CreateModal(value);

        //    return new JsonResult()
        //    {
        //        Data = result,
        //        JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //    };
        //}
        #endregion


    }
}