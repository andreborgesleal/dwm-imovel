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
using System.Linq;

namespace DWM.Models.BI
{
    public class RecusarEtapaBI : AprovarEtapaBI //DWMContext<ApplicationContext>, IProcess<EsteiraViewModel, ApplicationContext>
    {
        public override EsteiraViewModel Run(Repository value)
        {
            EsteiraViewModel r = (EsteiraViewModel)value;
            try
            {
                EsteiraModel model = new EsteiraModel(this.db, this.seguranca_db);
                int esteiraId = this.db.Esteiras.Where(info => info.propostaId == ((EsteiraViewModel)value).propostaId).OrderByDescending(info => info.esteiraId).FirstOrDefault().esteiraId;
                ((EsteiraViewModel)value).esteiraId = esteiraId;
                EsteiraViewModel esteiraViewModel = model.getObject((EsteiraViewModel)value);

                // Se estiver na etapa original não pode voltar à etapa anterior
                if (db.Etapas.Find(esteiraViewModel.etapaId).etapa_antId.HasValue)
                {
                    #region Reprovar Etapa
                    esteiraViewModel.observacao = ((EsteiraViewModel)value).observacao;
                    esteiraViewModel.ind_aprovacao = "R";
                    esteiraViewModel.dt_manifestacao = Funcoes.Brasilia();
                    esteiraViewModel.uri = ((EsteiraViewModel)value).uri;
                    esteiraViewModel = model.Update(esteiraViewModel);
                    if (esteiraViewModel.mensagem.Code > 0)
                    {
                        r = esteiraViewModel;
                        throw new ArgumentException(esteiraViewModel.mensagem.MessageBase);
                    }
                    #endregion

                    #region Incluir a proposta na etapa anterior
                    EsteiraViewModel etapaAnterior = new EsteiraViewModel()
                    {
                        propostaId = esteiraViewModel.propostaId,
                        etapaId = db.Etapas.Find(esteiraViewModel.etapaId).etapa_antId.Value,
                        descricao_etapa = db.Etapas.Find(db.Etapas.Find(esteiraViewModel.etapaId).etapa_antId).descricao,
                        dt_ocorrencia = ((EsteiraViewModel)value).dt_ocorrencia,
                        uri = ((EsteiraViewModel)value).uri
                    };
                    etapaAnterior = model.Insert(etapaAnterior);
                    #endregion

                    r = etapaAnterior;
                }
                else
                    throw new Exception("Esta etapa não pode ser recusada");
            }
            catch(Exception ex)
            {
                r.mensagem = new Validate() { Code = 999, MessageBase = ex.Message, Message = "Ocorreu um erro na aprovação da etapa" };
            }
            return r;
        }
    }
}