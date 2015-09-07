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
using App_Dominio.Models;
using App_Dominio.Contratos;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

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
            ViewBag.propostaId = propostaId.ToString();
            return _Edit(new PropostaViewModel() { propostaId = propostaId });
        }
        #endregion

        #region Incluir Comentário
        public ActionResult CreateComentario(int esteiraId, string observacao)
        {
            IPagedList result = InsertComentario(esteiraId, observacao);
            ViewBag.esteiraId = esteiraId.ToString();

            return View("_Comentarios", result);
        }

        private IPagedList InsertComentario(int esteiraId, string observacao)
        {
            IPagedList result = null;
            try
            {
                EsteiraComentarioViewModel value = new EsteiraComentarioViewModel();
                value.esteiraId = esteiraId;
                value.observacao = observacao;
                Factory<EsteiraComentarioViewModel, ApplicationContext> facade = new Factory<EsteiraComentarioViewModel, ApplicationContext>();
                value.uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
                result = facade.Execute(new InserirComentarioBI(), 0, 4, value, esteiraId);
                Success("Registro incluído com sucesso");
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

        public ActionResult ListComentarios(int? index, int? pageSize = 50, int? esteiraId = null)
        {
            Factory<EsteiraComentarioViewModel, ApplicationContext> facade = new Factory<EsteiraComentarioViewModel, ApplicationContext>();
            IPagedList result = facade.PagedList(new InserirComentarioBI(), index, 4, esteiraId);

            ViewBag.esteiraId = esteiraId.ToString();
            return View("_Comentarios", result);
        }
        #endregion

        #region Aprovar etapa
        [AuthorizeFilter(Order=1020)]
        public ActionResult Aprovar(int propostaId, string dt_ocorrencia, string observacao_etapa, int? esteiraId, string btnAprovarRecusar)
        {
            if (ViewBag.ValidateRequest)
            {
                IEnumerable<EsteiraViewModel> result = AprovarEtapa(propostaId, dt_ocorrencia, observacao_etapa, btnAprovarRecusar);
                ViewBag.propostaId = propostaId.ToString();
                return View("_Esteira", result);
            }
            else
            {
                Error("Acesso para esta funcionalidade negado");
                return ListEsteira(0, 50, propostaId);
            }
        }

        private IEnumerable<EsteiraViewModel> AprovarEtapa(int propostaId, string dt_ocorrencia, string observacao_etapa, string btnAprovarRecusar)
        {
            IEnumerable<EsteiraViewModel> result = new List<EsteiraViewModel>();
            try
            {
                EsteiraViewModel value = new EsteiraViewModel();
                value.propostaId = propostaId;
                if (dt_ocorrencia != null)
                    value.dt_ocorrencia = Funcoes.StringToDate(dt_ocorrencia).Value;
                else
                    throw new Exception("Data da ocorrência deve ser informada");
                value.observacao = observacao_etapa;
                Factory<EsteiraViewModel, ApplicationContext> facade = new Factory<EsteiraViewModel, ApplicationContext>();
                value.uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
                result = facade.Execute(getProcess(btnAprovarRecusar), value, propostaId);

                if (facade.Mensagem.Code > 0)
                    throw new App_DominioException(facade.Mensagem);
                
                Success("Registro incluído com sucesso");
            }
            catch (App_DominioException ex)
            {
                ViewBag.observacao = observacao_etapa;
                if (dt_ocorrencia != null)
                    ViewBag.dt_ocorrencia = Funcoes.StringToDate(dt_ocorrencia).Value;
                else
                    ViewBag.dt_ocorrencia = DateTime.Today;
                ViewBag.propostaId = propostaId.ToString();
                
                ModelState.AddModelError(ex.Result.Field, ex.Result.Message); // mensagem amigável ao usuário
                Error(ex.Result.MessageBase); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
            }
            catch (Exception ex)
            {
                ViewBag.observacao = observacao_etapa;
                if (dt_ocorrencia != null)
                    ViewBag.dt_ocorrencia = Funcoes.StringToDate(dt_ocorrencia).Value;
                else
                    ViewBag.dt_ocorrencia = DateTime.Today;
                ViewBag.propostaId = propostaId.ToString();

                App_DominioException.saveError(ex, GetType().FullName);
                ModelState.AddModelError("", MensagemPadrao.Message(17).ToString()); // mensagem amigável ao usuário
                Error(ex.Message); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
            }

            return result;
        }

        private IProcess<EsteiraViewModel, ApplicationContext> getProcess(string btnAprovarRecusar)
        {
            return btnAprovarRecusar == "approve" ? new AprovarEtapaBI() : new RecusarEtapaBI();
        }

        public ActionResult ListEsteira(int? index, int? pageSize = 50, int? propostaId = null)
        {
            Factory<EsteiraViewModel, ApplicationContext> facade = new Factory<EsteiraViewModel, ApplicationContext>();
            IEnumerable<EsteiraViewModel> result = facade.List(new AprovarEtapaBI(), propostaId);

            ViewBag.propostaId = propostaId.ToString();
            return View("_Esteira", result);
        }

        public ActionResult ProgressBarEsteira(int propostaId)
        {
            #region Atualizar a progressbar da esteira
            Factory<PropostaViewModel, ApplicationContext> progressFacade = new Factory<PropostaViewModel, ApplicationContext>();
            PropostaViewModel propostaViewModel = new PropostaViewModel() { propostaId = propostaId };
            propostaViewModel = progressFacade.Execute(new ProgressBarEsteiraBI(), propostaViewModel);
            #endregion

            return View("_ProgressBarEsteira", propostaViewModel);
        }
        #endregion

        #region Etapa de contabilização (Upload de arquivos)
        [AuthorizeFilter(Order=1040)]
        public ActionResult Upload(int esteiraId, string fileProposta, string nome_original)
        {
            if (ViewBag.ValidateRequest)
            {
                IEnumerable<EsteiraContabilizacaoViewModel> result = EsteiraContabilizacao(esteiraId, fileProposta, nome_original);
                ViewBag.esteiraId = esteiraId.ToString();
                return View("_DadosProposta", result);
            }
            else
            {
                Error("Acesso para esta funcionalidade negado");
                return ListArquivos(0, 50, esteiraId);
            }
        }

        private IEnumerable<EsteiraContabilizacaoViewModel> EsteiraContabilizacao(int esteiraId, string fileProposta, string nome_original)
        {
            IEnumerable<EsteiraContabilizacaoViewModel> result = new List<EsteiraContabilizacaoViewModel>();
            try
            {
                EsteiraContabilizacaoViewModel value = new EsteiraContabilizacaoViewModel();
                Factory<EsteiraContabilizacaoViewModel, ApplicationContext> facade = new Factory<EsteiraContabilizacaoViewModel, ApplicationContext>();
                value.esteiraId = esteiraId;
                value.arquivo = fileProposta;
                value.nome_original = nome_original;
                value.uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
                result = facade.Execute(new EsteiraContabilizacaoBI(), value, esteiraId);

                if (facade.Mensagem.Code > 0)
                    throw new App_DominioException(facade.Mensagem);

                Success("Registro incluído com sucesso");
            }
            catch (App_DominioException ex)
            {
                ViewBag.esteiraId = esteiraId.ToString();
                ModelState.AddModelError(ex.Result.Field, ex.Result.Message); // mensagem amigável ao usuário
                Error(ex.Result.MessageBase); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
            }
            catch (Exception ex)
            {
                ViewBag.esteiraId = esteiraId.ToString();
                App_DominioException.saveError(ex, GetType().FullName);
                ModelState.AddModelError("", MensagemPadrao.Message(17).ToString()); // mensagem amigável ao usuário
                Error(ex.Message); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
            }

            return result;
        }

        // Exclui arquivo
        [AuthorizeFilter(Order=1050)]
        public ActionResult Erase(int esteiraId, string fileProposta, string nome_original)
        {
            if (ViewBag.ValidateRequest)
            {
                IEnumerable<EsteiraContabilizacaoViewModel> result = EraseEsteiraContabilizacao(esteiraId, fileProposta, nome_original);
                ViewBag.esteiraId = esteiraId.ToString();
                return View("_DadosProposta", result);
            }
            else
            {
                Error("Acesso para esta funcionalidade negado");
                return ListArquivos(0, 50, esteiraId);
            }
        }

        private IEnumerable<EsteiraContabilizacaoViewModel> EraseEsteiraContabilizacao(int esteiraId, string fileProposta, string nome_original)
        {
            IEnumerable<EsteiraContabilizacaoViewModel> result = new List<EsteiraContabilizacaoViewModel>();
            try
            {
                EsteiraContabilizacaoViewModel value = new EsteiraContabilizacaoViewModel();
                Factory<EsteiraContabilizacaoViewModel, ApplicationContext> facade = new Factory<EsteiraContabilizacaoViewModel, ApplicationContext>();
                value.esteiraId = esteiraId;
                value.arquivo = fileProposta;
                value.nome_original = nome_original;
                value.uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
                result = facade.Execute(new EsteiraContabilizacaoDeleteBI(), value, esteiraId);

                if (facade.Mensagem.Code > 0)
                    throw new App_DominioException(facade.Mensagem);

                Success("Arquivo excluído com sucesso");
            }
            catch (App_DominioException ex)
            {
                ViewBag.esteiraId = esteiraId.ToString();
                ModelState.AddModelError(ex.Result.Field, ex.Result.Message); // mensagem amigável ao usuário
                Error(ex.Result.MessageBase); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
            }
            catch (Exception ex)
            {
                ViewBag.esteiraId = esteiraId.ToString();
                App_DominioException.saveError(ex, GetType().FullName);
                ModelState.AddModelError("", MensagemPadrao.Message(17).ToString()); // mensagem amigável ao usuário
                Error(ex.Message); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
            }

            return result;
        }

        public ActionResult ListArquivos(int? index, int? pageSize = 50, int? esteiraId = null)
        {
            Factory<EsteiraContabilizacaoViewModel, ApplicationContext> facade = new Factory<EsteiraContabilizacaoViewModel, ApplicationContext>();
            IEnumerable<EsteiraContabilizacaoViewModel> result = facade.List(new EsteiraContabilizacaoBI(), esteiraId);

            ViewBag.esteiraId = esteiraId.ToString();
            return View("_DadosProposta", result);
        }

        #endregion

        #region Comissão
        [AuthorizeFilter(Order = 1010)]
        public ActionResult Comissao(int esteiraComId, int grupoId, string nome_grupo, string valor_comissao)
        {
            if (ViewBag.ValidateRequest)
            {
                IEnumerable<EsteiraComissaoViewModel> result = EsteiraComissao(esteiraComId, grupoId, nome_grupo, valor_comissao);
                ViewBag.esteiraId = esteiraComId.ToString();
                ViewBag.vr_total = result.Select(info => info.valor).Sum().ToString("R$ ###,###,###,##0.00");

                return View("_Comissao", result);
            }
            else
            {
                Error("Acesso para esta funcionalidade negado");
                return ListComissao(esteiraComId);
            }
                
        }

        public ActionResult ListComissao(int? esteiraId)
        {
            IEnumerable<EsteiraComissaoViewModel> result = new List<EsteiraComissaoViewModel>();
            try
            {
                Factory<EsteiraComissaoViewModel, ApplicationContext> facade = new Factory<EsteiraComissaoViewModel, ApplicationContext>();
                result = facade.List(new EsteiraComissaoBI(), esteiraId);
                ViewBag.vr_total = result.Select(info => info.valor).Sum().ToString("R$ ###,###,###,##0.00");
            }
            catch (App_DominioException ex)
            {
                ViewBag.esteiraId = esteiraId.ToString();
                ModelState.AddModelError(ex.Result.Field, ex.Result.Message); // mensagem amigável ao usuário
                Error(ex.Result.MessageBase); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
            }
            catch (Exception ex)
            {
                ViewBag.esteiraId = esteiraId.ToString();
                App_DominioException.saveError(ex, GetType().FullName);
                ModelState.AddModelError("", MensagemPadrao.Message(17).ToString()); // mensagem amigável ao usuário
                Error(ex.Message); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
            }

            return View("_Comissao", result);
        }

        private IEnumerable<EsteiraComissaoViewModel> EsteiraComissao(int esteiraId, int grupoId, string nome_grupo, string valor)
        {
            IEnumerable<EsteiraComissaoViewModel> result = new List<EsteiraComissaoViewModel>();
            try
            {
                EsteiraComissaoViewModel value = new EsteiraComissaoViewModel();
                value.esteiraId = esteiraId;
                value.grupoId = grupoId;
                value.nome_grupo = nome_grupo;
                value.valor = Convert.ToDecimal(valor);

                Factory<EsteiraComissaoViewModel, ApplicationContext> facade = new Factory<EsteiraComissaoViewModel, ApplicationContext>();
                value.uri = this.ControllerContext.Controller.GetType().Name.Replace("Controller", "") + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
                result = facade.Execute(new EsteiraComissaoBI(), value, esteiraId);

                if (facade.Mensagem.Code > 0)
                    throw new App_DominioException(facade.Mensagem);

                Success("Registro alterado com sucesso");
            }
            catch (App_DominioException ex)
            {
                ViewBag.esteiraId = esteiraId.ToString();
                ViewBag.grupoId = grupoId.ToString();
                ViewBag.nome_grupo = nome_grupo;
                ViewBag.valor = valor;
                ModelState.AddModelError(ex.Result.Field, ex.Result.Message); // mensagem amigável ao usuário
                Error(ex.Result.MessageBase); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
            }
            catch (Exception ex)
            {
                ViewBag.esteiraId = esteiraId.ToString();
                ViewBag.grupoId = grupoId.ToString();
                ViewBag.nome_grupo = nome_grupo;
                ViewBag.valor = valor;
                App_DominioException.saveError(ex, GetType().FullName);
                ModelState.AddModelError("", MensagemPadrao.Message(17).ToString()); // mensagem amigável ao usuário
                Error(ex.Message); // Mensagem em inglês com a descrição detalhada do erro e fica no topo da tela
            }

            return result;
        }


        #endregion
    }
}