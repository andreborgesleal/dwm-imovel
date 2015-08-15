using App_Dominio.Controllers;
using App_Dominio.Security;
using DWM.Models.Enumeracoes;
using DWM.Models.Persistence;
using DWM.Models.Repositories;
using System.Web.Mvc;
using DWM.Models.Entidades;
using System;
using System.Collections.Generic;
using App_Dominio.Pattern;
using App_Dominio.Enumeracoes;
using DWM.Models.BI;


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
        public ActionResult CreateComentario(int esteiraId, string observacao)
        {
            IEnumerable<EsteiraComentarioViewModel> result = InsertComentario(esteiraId, observacao);
            return View("_Comentarios", result);

            //return new JsonResult()
            //{
            //    Data = result,
            //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
            //};
        }

        private IEnumerable<EsteiraComentarioViewModel> InsertComentario(int esteiraId, string observacao)
        {
            IEnumerable<EsteiraComentarioViewModel> result = new List<EsteiraComentarioViewModel>();
            try
            {
                EsteiraComentarioViewModel value = new EsteiraComentarioViewModel();
                value.esteiraId = esteiraId;
                value.observacao = observacao;
                Factory<EsteiraComentarioViewModel, ApplicationContext> facade = new Factory<EsteiraComentarioViewModel, ApplicationContext>();
                value.uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
                result = facade.Execute(new InserirComentarioBI(), value, esteiraId);
                if (value.mensagem.Code > 0)
                    throw new App_DominioException(value.mensagem);
            }
            catch (App_DominioException ex)
            {
                ModelState.AddModelError(ex.Result.Field, ex.Result.Message); // mensagem amigável ao usuário
                Error(ex.Result.MessageBase); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
            }
            catch (Exception ex)
            {
                App_DominioException.saveError(ex, GetType().FullName);
                ModelState.AddModelError("", MensagemPadrao.Message(17).ToString()); // mensagem amigável ao usuário
                Error(ex.Message); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
            }

            return result;
        }
        #endregion


    }
}