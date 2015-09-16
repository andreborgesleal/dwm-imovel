﻿using App_Dominio.Controllers;
using App_Dominio.Security;
using DWM.Models.Enumeracoes;
using DWM.Models.Persistence;
using DWM.Models.Repositories;
using System.Web.Mvc;
using DWM.Models.Entidades;

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
            BindBreadCrumb("Edição", true);
            return _Edit(new PropostaViewModel() { propostaId = propostaId });
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