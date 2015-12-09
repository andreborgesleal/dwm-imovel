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
    public class CancelarFaturamentoBI : DWMContext<ApplicationContext>, IProcess<EsteiraViewModel, ApplicationContext>
    {
        #region Constructor
        public CancelarFaturamentoBI() { }

        public CancelarFaturamentoBI(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        public virtual EsteiraViewModel Run(Repository value)
        {
            EsteiraViewModel r = (EsteiraViewModel)value;
            r.mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso!!" };
            try
            {
                EsteiraModel model = new EsteiraModel(this.db, this.seguranca_db);
                EsteiraViewModel esteiraViewModel = model.getObject((EsteiraViewModel)value);

                if (esteiraViewModel.ind_aprovacao != null || esteiraViewModel.ind_aprovacao == "A" && esteiraViewModel.etapaId == 6)
                {
                    #region Cancelar Etapa de Faturamento
                    esteiraViewModel.observacao = "Cancelamento de aprovação da etapa de faturamento";
                    esteiraViewModel.ind_aprovacao = null;
                    esteiraViewModel.dt_manifestacao = null;
                    esteiraViewModel.uri = ((EsteiraViewModel)value).uri;
                    esteiraViewModel = model.Update(esteiraViewModel);
                    if (esteiraViewModel.mensagem.Code > 0)
                    {
                        r = esteiraViewModel;
                        throw new Exception(esteiraViewModel.mensagem.MessageBase);
                    }
                    #endregion
                }
                else
                    throw new Exception("Esta etapa não pode ser cancelada");
            }
            catch (Exception ex)
            {
                r.mensagem = new Validate() { Code = 999, MessageBase = ex.Message, Message = "Ocorreu um erro no cancelamento da etapa" };
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