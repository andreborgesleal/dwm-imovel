using App_Dominio.Component;
using App_Dominio.Contratos;
using App_Dominio.Controllers;
using App_Dominio.Entidades;
using App_Dominio.Models;
using App_Dominio.Pattern;
using App_Dominio.Security;
using DWM.Models.Entidades;
using DWM.Models.Persistence;
using DWM.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DWM.Controllers
{
    public class HomeController : SuperController
    {
        #region Inheritance
        public override int _sistema_id() { return (int)DWM.Models.Enumeracoes.Sistema.DWMIMOVEL; }

        public override string getListName()
        {
            return "Detalhar";
        }

        public override ActionResult List(int? index, int? PageSize, string descricao = null)
        {
            throw new NotImplementedException();
        }
        #endregion

        public ActionResult Index()
        {
            return View();
        }

        //[AuthorizeFilter]
        public ActionResult Default()
        {
            return View();
            //if (ViewBag.ValidateRequest)
            //    return View();
            //else
            //    return View();
        }

        public ActionResult Chart()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult _Error()
        {
            return View();
        }

        #region Alerta - segurança
        public ActionResult ReadAlert(int? alertaId)
        {
            try
            {
                EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
                if (alertaId.HasValue && alertaId > 0)
                    security.ReadAlert(alertaId.Value);
            }
            catch
            {
                return null;
            }

            return null;
        }
        #endregion

        #region Formulário Modal
        #region Formulário Modal Genérico
        public ActionResult LOVModal(IPagedList pagedList)
        {
            return View(pagedList);
        }
        #endregion

        [AuthorizeFilter]
        #region Formulário Modal Clientes
        public ActionResult LovClienteModal(int? index, int? pageSize = 50)
        {
            Facade<ClienteViewModel, ClienteModel, ApplicationContext> facade = new Facade<ClienteViewModel, ClienteModel, ApplicationContext>();
            IPagedList pagedList = facade.getPagedList(new LookupClienteModel(), index, pageSize.Value);
            //return View("LOVModal", pagedList);
            return View(pagedList);

            //ViewBag.Header = header;

            //if (param != null && param.Count() > 0)
            //    return View(pagedList);
            //else
            //    return View("LOVModal", pagedList);

            //return this.ListModal(index, pageSize, new LookupClienteModel(), "Nome");
        }
        #endregion

        [AuthorizeFilter]
        #region Formulário Modal Empreendimentos
        public ActionResult LovEmpreendimentoModal(int? index, int? pageSize = 50)
        {
            Facade<EmpreendimentoViewModel, EmpreendimentoModel, ApplicationContext> facade = new Facade<EmpreendimentoViewModel, EmpreendimentoModel, ApplicationContext>();
            IPagedList pagedList = facade.getPagedList(new LookupEmpreendimentoModel(), index, pageSize.Value);
            return View(pagedList);
        }
        #endregion

        #endregion
    }
}