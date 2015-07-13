using App_Dominio.Controllers;
using App_Dominio.Security;
using DWM.Models.Enumeracoes;
using DWM.Models.Persistence;
using DWM.Models.Repositories;
using System.Web.Mvc;
using DWM.Models.Entidades;

namespace DWM.Controllers
{
    public class EmpreendimentosController : DwmRootController<EmpreendimentoViewModel, EmpreendimentoModel, ApplicationContext>
    {
        #region Herança
        public override int _sistema_id() { return (int)Sistema.DWMIMOVEL; }
        public override string getListName()
        {
            return "Listar Empreendimentos";
        }
        #endregion

        #region List
        [AuthorizeFilter]
        public override ActionResult List(int? index, int? pageSize = 50, string descricao = null)
        {
            if (ViewBag.ValidateRequest)
            {
                ListViewEmpreendimento e = new ListViewEmpreendimento();
                return this._List(index, pageSize, "Browse", e, descricao);
            }
            else
                return View();
        }

        [AuthorizeFilter]
        public ActionResult _ListEmpreendimentoModal(int? index, int? pageSize = 50, string descricao = null)
        {
            if (ViewBag.ValidateRequest)
            {
                LookupEmpreendimentoFiltroModel l = new LookupEmpreendimentoFiltroModel();
                return this.ListModal(index, pageSize, l, "Descrição", descricao);
            }
            else
                return View();
        }
        #endregion

        #region Edit
        [AuthorizeFilter]
        public ActionResult Edit(int empreendimentoId)
        {
            return _Edit(new EmpreendimentoViewModel() { empreendimentoId = empreendimentoId });
        }
        #endregion

        #region Delete
        [AuthorizeFilter]
        public ActionResult Delete(int empreendimentoId)
        {
            return Edit(empreendimentoId);
        }
        #endregion

        #region CrudEmpreendimentoModal
        public JsonResult CrudClienteModal(string descricao)
        {
            return JSonCrud(new EmpreendimentoViewModel() { nome = descricao });
        }
        #endregion

        public JsonResult getNames()
        {
            return JSonTypeahead(null, new ListViewEmpreendimento());
        }

    }
}