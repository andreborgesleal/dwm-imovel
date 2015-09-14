using App_Dominio.Component;
using App_Dominio.Contratos;
using App_Dominio.Controllers;
using App_Dominio.Entidades;
using App_Dominio.Models;
using App_Dominio.Pattern;
using App_Dominio.Security;
using DWM.Models.BI;
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

        [AuthorizeFilter]
        public ActionResult Default(int? index, int? pageSize = 15, int? empreendimentoId = null, string torre_unidade = "", string cpf_nome = "",
                                            int? etapaId = null, int? propostaId = null, DateTime? dt_proposta1 = null, DateTime? dt_proposta2 = null,
                                            string situacao = "", int? corretor1Id = null)
        {
            if (ViewBag.ValidateRequest)
            {
                #region ListPanorama
                if (!dt_proposta1.HasValue)
                    dt_proposta1 = Convert.ToDateTime(DateTime.Today.AddMonths(-2).ToString("yyyy-MM-") + "01");
                if (!dt_proposta2.HasValue)
                    dt_proposta2 = DateTime.Today;
                if (situacao == "")
                    situacao = "A";

                HomeViewModel home = new HomeViewModel()
                {
                    empreendimentoId = empreendimentoId,
                    torre_unidade = torre_unidade,
                    cpf_nome = cpf_nome,
                    etapaId = etapaId,
                    propostaId = propostaId,
                    dt_proposta1 = dt_proposta1,
                    dt_proposta2 = dt_proposta2,
                    situacao = situacao,
                    corretor1Id = corretor1Id
                };
                Factory<HomeViewModel, ApplicationContext> factory = new Factory<HomeViewModel, ApplicationContext>();
                return View(factory.Execute(new HomeBI(), home));

                //ListViewProposta model = new ListViewProposta();
                ////Facade<PropostaViewModel, PropostaModel, ApplicationContext> facade = new Facade<PropostaViewModel, PropostaModel, ApplicationContext>();
                //IPagedList pagedList = facade.getPagedList((ListViewModel<PropostaViewModel, ApplicationContext>)model, index, pageSize.Value,
                //                                                empreendimentoId, torre_unidade, cpf_nome, etapaId, propostaId, dt_proposta1, dt_proposta2,
                //                                                situacao, corretor1Id);
                #endregion
                //return ListPanorama(index, pageSize, empreendimentoId, torre_unidade, cpf_nome, etapaId, propostaId, dt_proposta1, dt_proposta2, situacao, corretor1Id);
            }
            else
                return View();
        }

        [AuthorizeFilter]
        public ActionResult ListPanorama(int? index, int? pageSize = 15, int? empreendimentoId = null, string torre_unidade = "", string cpf_nome = "", 
                                            int? etapaId = null, int? propostaId = null, DateTime? dt_proposta1 = null, DateTime? dt_proposta2 = null, 
                                            string situacao = "", int? corretor1Id = null)
        {
            if (ViewBag.ValidateRequest)
            {
                if (!dt_proposta1.HasValue)
                    dt_proposta1 = Convert.ToDateTime(DateTime.Today.AddMonths(-2).ToString("yyyy-MM-") + "01");
                if (!dt_proposta2.HasValue)
                    dt_proposta2 = DateTime.Today;
                if (situacao == "")
                    situacao = "A";

                ListViewProposta model = new ListViewProposta();
                Facade<PropostaViewModel, PropostaModel, ApplicationContext> facade = new Facade<PropostaViewModel, PropostaModel, ApplicationContext>();
                IPagedList pagedList = facade.getPagedList((ListViewModel<PropostaViewModel, ApplicationContext>)model, index, pageSize.Value, 
                                                                empreendimentoId, torre_unidade, cpf_nome, etapaId, propostaId, dt_proposta1, dt_proposta2,
                                                                situacao, corretor1Id);
                return View(pagedList);
            }
            else
                return View();
        }

        public ActionResult ListAllComentarios(int? index, int? pageSize = 10, string descricao = null)
        {
            ListViewComentarioByUsuario model = new ListViewComentarioByUsuario();
            Facade<EsteiraComentarioViewModel, EsteiraComentarioModel, ApplicationContext> facade = new Facade<EsteiraComentarioViewModel, EsteiraComentarioModel, ApplicationContext>();
            IPagedList pagedList = facade.getPagedList((ListViewModel<EsteiraComentarioViewModel, ApplicationContext>)model, index, pageSize.Value, descricao);

            return View(pagedList); ;
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

        [AuthorizeFilter]
        #region Formulário Modal Corretores
        public ActionResult LovCorretorModal(int? index, int? pageSize = 50)
        {
            Facade<CorretorViewModel, CorretorModel, ApplicationContext> facade = new Facade<CorretorViewModel, CorretorModel, ApplicationContext>();
            IPagedList pagedList = facade.getPagedList(new LookupCorretorModel(), index, pageSize.Value);
            return View(pagedList);
        }
        #endregion
        #endregion
    }
}