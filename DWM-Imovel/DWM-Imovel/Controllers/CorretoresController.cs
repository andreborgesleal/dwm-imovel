using App_Dominio.Controllers;
using App_Dominio.Security;
using DWM.Models.Enumeracoes;
using DWM.Models.Persistence;
using DWM.Models.Repositories;
using System.Web.Mvc;
using System;
using DWM.Models.Entidades;
using App_Dominio.Contratos;

namespace DWM.Controllers
{
    public class CorretoresController : DwmRootController<CorretorViewModel, CorretorModel, ApplicationContext>
    {
        public override int _sistema_id() { return (int)Sistema.DWMIMOVEL; }
        public override string getListName()
        {
            return "Listar Corretores";
        }

        #region List
        [AuthorizeFilter]
        public override ActionResult List(int? index, int? pageSize = 50, string nome = null)
        {
            if (ViewBag.ValidateRequest)
            {
                ListViewCorretor e = new ListViewCorretor();
                return this._List(index, pageSize, "Browse", e, nome);
            }
            else
                return View();
        }
        #endregion

        #region Edit
        [AuthorizeFilter]
        public ActionResult Edit(int corretorId)
        {
            return _Edit(new CorretorViewModel() { corretorId = corretorId });
        }
        #endregion

        #region Delete
        [AuthorizeFilter]
        public ActionResult Delete(int corretorId)
        {
            return Edit(corretorId);
        }
        #endregion

        #region CrudCorretor Modal
        public JsonResult CrudCorretorModal(string descricao)
        {
            return JSonCrud(new CorretorViewModel() { nome = descricao });
        }
        #endregion

        public JsonResult getNames()
        {
            return JSonTypeahead(null, new ListViewCorretor());
        }
    }
}