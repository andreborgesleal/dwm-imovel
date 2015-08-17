using System;
using System.Collections.Generic;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Component;
using DWM.Models.Repositories;
using DWM.Models.Entidades;
using DWM.Models.Persistence;
using System.Web.Mvc;
using App_Dominio.Models;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
//using System.Linq;

namespace DWM.Models.BI
{
    public class AprovarEtapaBI : DWMContext<ApplicationContext>, IProcess<EsteiraViewModel, ApplicationContext>
    {
        #region Constructor
        public AprovarEtapaBI() { }

        public AprovarEtapaBI(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        public EsteiraViewModel Run(Repository value)
        {
            EsteiraViewModel r = (EsteiraViewModel)value;
            try
            {
                #region Aprovar Etapa
                EsteiraModel model = new EsteiraModel(this.db, this.seguranca_db);

                int esteiraId = this.db.Esteiras.Where(info => info.propostaId == ((EsteiraViewModel)value).propostaId).OrderByDescending(info => info.esteiraId).FirstOrDefault().esteiraId;

                ((EsteiraViewModel)value).esteiraId = esteiraId;

                EsteiraViewModel esteiraViewModel = model.getObject((EsteiraViewModel)value);
                esteiraViewModel.observacao = ((EsteiraViewModel)value).observacao;
                esteiraViewModel.ind_aprovacao = "A";
                esteiraViewModel.dt_manifestacao = Funcoes.Brasilia();
                esteiraViewModel.uri = ((EsteiraViewModel)value).uri;
                esteiraViewModel = model.Update(esteiraViewModel);
                if (esteiraViewModel.mensagem.Code > 0)
                {
                    r = esteiraViewModel;
                    throw new Exception(esteiraViewModel.mensagem.MessageBase);
                }
                #endregion

                #region Incluir a proposta na próxima etapa
                EsteiraViewModel proximaEtapa = new EsteiraViewModel()
                {
                    propostaId = esteiraViewModel.propostaId,
                    etapaId = db.Etapas.Find(esteiraViewModel.etapaId).etapa_proxId.Value,
                    descricao_etapa = db.Etapas.Find(db.Etapas.Find(esteiraViewModel.etapaId).etapa_proxId).descricao,
                    dt_ocorrencia = ((EsteiraViewModel)value).dt_ocorrencia,
                    uri = ((EsteiraViewModel)value).uri
                };
                proximaEtapa = model.Insert(proximaEtapa);
                #endregion

                r = proximaEtapa;
            }
            catch(Exception ex)
            {
                r.mensagem = new Validate() { Code = 999, MessageBase = ex.Message, Message = "Ocorreu um erro na aprovação da etapa" };
            }
            return r;
        }

        public IEnumerable<EsteiraViewModel> List(params object[] param)
        {
            ListViewEsteira list = new ListViewEsteira(this.db, this.seguranca_db);
            return list.Bind(0, 100, param);
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {
            ListViewEsteira list = new ListViewEsteira(this.db, this.seguranca_db);
            return list.getPagedList(index, pageSize, param);
        }
    }
}