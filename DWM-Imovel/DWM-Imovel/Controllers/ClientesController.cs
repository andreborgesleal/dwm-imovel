using App_Dominio.Controllers;
using App_Dominio.Security;
using DWM.Models.Enumeracoes;
using DWM.Models.Persistence;
using DWM.Models.Repositories;
using System.Web.Mvc;
using DWM.Models.Entidades;

namespace DWM.Controllers
{
    public class ClientesController : DwmRootController<ClienteViewModel, ClienteModel, ApplicationContext>
    {
        public override int _sistema_id() { return (int)Sistema.DWMIMOVEL; }
        public override string getListName()
        {
            return "Listar Clientes";
        }

        #region List
        [AuthorizeFilter]
        public override ActionResult List(int? index, int? pageSize = 50, string nome = null)
        {
            if (ViewBag.ValidateRequest)
            {
                ListViewCliente e = new ListViewCliente();
                return this._List(index, pageSize, "Browse", e, nome);
            }
            else
                return View();
        }
        #endregion

        #region Edit
        [AuthorizeFilter]
        public ActionResult Edit(int clienteId)
        {
            return _Edit(new ClienteViewModel() { clienteId = clienteId });
        }
        #endregion

        #region Delete
        [AuthorizeFilter]
        public ActionResult Delete(int clienteId)
        {
            return Edit(clienteId);
        }
        #endregion
    }
}