using App_Dominio.Controllers;
using App_Dominio.Security;
using DWM.Models.Enumeracoes;
using DWM.Models.Persistence;
using DWM.Models.Repositories;
using System.Web.Mvc;
using System;
using DWM.Models.Entidades;
using App_Dominio.Contratos;
using App_Dominio.Pattern;
using DWM.Models.BI;
using App_Dominio.Enumeracoes;

namespace DWM.Controllers
{
    public class ClientesController : DwmRootController<ClienteViewModel, ClienteModel, ApplicationContext>
    {
        public override int _sistema_id() { return (int)Sistema.DWMIMOVEL; }
        public override string getListName()
        {
            return "Listar Clientes";
        }

        public override ActionResult HomePage()
        {
            return RedirectToAction("Default", "Home");
        }

        #region List
        [AuthorizeFilter]
        public override ActionResult List(int? index, int? pageSize = 50, string descricao = null)
        {
            if (ViewBag.ValidateRequest)
            {
                ListViewCliente e = new ListViewCliente();
                return this._List(index, pageSize, "Browse", e, descricao);
            }
            else
                return View();
        }

        [AuthorizeFilter]
        public ActionResult _ListClienteModal(int? index, int? pageSize = 50, string descricao = null)
        {
            if (ViewBag.ValidateRequest)
            {
                LookupClienteFiltroModel l = new LookupClienteFiltroModel();
                return this.ListModal(index, pageSize, l, "Descrição", descricao);
            }
            else
                return View();
        }


        #region BeforeCreate
        public override void BeforeCreate(ref ClienteViewModel value, FormCollection collection)
        {
            if (value.ind_tipo_pessoa == "PF")
            {
                if (collection["dt_nascimento"] != "")
                    value.dt_nascimento = DateTime.Parse(collection["dt_nascimento"].Substring(6, 4) + "-" + collection["dt_nascimento"].Substring(3, 2) + "-" + collection["dt_nascimento"].Substring(0, 2));
            }
            else
            {
                value.sexo = null;
                value.dt_nascimento = null;
            }
        }
        #endregion
        #endregion

        #region Edit
        [AuthorizeFilter]
        public ActionResult Edit(int clienteId)
        {
            try
            {
                #region Verificar se o usuário tem permissão de acesso a esta proposta
                Factory<ClienteViewModel, ApplicationContext> facade = new Factory<ClienteViewModel, ApplicationContext>();
                ClienteViewModel result = facade.Execute(new CheckClienteBI(), new ClienteViewModel() { clienteId = clienteId });
                if (result.mensagem.Code > 0)
                    throw new App_DominioException(result.mensagem);
                #endregion

                BindBreadCrumb("Clientes", true);
                ViewBag.clienteId = clienteId.ToString();
                return _Edit(new ClienteViewModel() { clienteId = clienteId });
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

        public override void BeforeEdit(ref ClienteViewModel value, FormCollection collection)
        {
            BeforeCreate(ref value, collection);
        }
        #endregion

        #region Delete
        [AuthorizeFilter]
        public ActionResult Delete(int clienteId)
        {
            return Edit(clienteId);
        }
        #endregion

        #region CrudCliente Modal
        public JsonResult CrudClienteModal(string descricao)
        {
            return JSonCrud(new ClienteViewModel() { nome = descricao });
        }
        #endregion

        public JsonResult getNames()
        {
            return JSonTypeahead(null, new ListViewCliente());
        }

    }
}