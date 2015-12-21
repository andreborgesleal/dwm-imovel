using App_Dominio.Controllers;
using App_Dominio.Security;
using DWM.Models.Enumeracoes;
using DWM.Models.Persistence;
using DWM.Models.Repositories;
using System.Web.Mvc;
using DWM.Models.Entidades;
using App_Dominio.Pattern;
using DWM.Models.BI;
using System;
using App_Dominio.Enumeracoes;

namespace DWM.Controllers
{
    public class PropostasController : DwmRootController<PropostaViewModel, PropostaModel, ApplicationContext>
    {
        public override int _sistema_id() { return (int)Sistema.DWMIMOVEL; }
        public override string getListName()
        {
            return "Listar Propostas";
        }

        #region List
        [AuthorizeFilter]
        public override ActionResult List(int? index, int? pageSize = 50, string nome = null)
        {
            return View();
        }
        #endregion


        public override ActionResult HomePage()
        {
            return RedirectToAction("Default", "Home");
        }

        public override void BeforeCreate(ref PropostaViewModel value, FormCollection collection)
        {
            value.valor = decimal.Parse(collection["valor1"].Replace(".", ""));
            value.vr_comissao = decimal.Parse(collection["vr_comissao1"].Replace(".", ""));
        }


        #region Edit
        [AuthorizeFilter]
        public ActionResult Edit(int propostaId)
        {
            try
            {
                #region Verificar se o usuário tem permissão de acesso a esta proposta
                Factory<PropostaViewModel, ApplicationContext> facade = new Factory<PropostaViewModel, ApplicationContext>();
                PropostaViewModel result = facade.Execute(new CheckPropostaBI(), new PropostaViewModel() { propostaId = propostaId });
                if (result.mensagem.Code > 0)
                    throw new App_DominioException(result.mensagem);
                #endregion

                BindBreadCrumb("Edição", true);
                ViewBag.propostaId = propostaId.ToString();
                return _Edit(new PropostaViewModel() { propostaId = propostaId });
            }
            catch (App_DominioException ex)
            {
                ModelState.AddModelError(ex.Result.Field, ex.Result.Message); // mensagem amigável ao usuário
                Error("Acesso não autorizado"); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
            }
            catch (Exception ex)
            {
                App_DominioException.saveError(ex, GetType().FullName);
                ModelState.AddModelError("", MensagemPadrao.Message(202).ToString()); // mensagem amigável ao usuário
                Error("Acesso não autorizado"); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
            }

            return View("Edit", null);
        }

        public override void BeforeEdit(ref PropostaViewModel value, FormCollection collection)
        {
            value.valor = decimal.Parse(collection["valor1"].Replace(".", ""));
            value.vr_comissao = decimal.Parse(collection["vr_comissao1"].Replace(".", ""));
        }
        #endregion

        #region Delete
        [AuthorizeFilter]
        public ActionResult Delete(int propostaId)
        {
            return Edit(propostaId);
        }
        #endregion

    }
}