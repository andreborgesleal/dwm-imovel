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
    public class ProgressBarEsteiraBI : DWMContext<ApplicationContext>, IProcess<PropostaViewModel, ApplicationContext>
    {
        #region Constructor
        public ProgressBarEsteiraBI() { }

        public ProgressBarEsteiraBI(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        public virtual PropostaViewModel Run(Repository value)
        {
            PropostaViewModel prop = new PropostaViewModel();
            try
            {
                PropostaModel propModel = new PropostaModel(this.db, this.seguranca_db);
                prop.propostaId = ((PropostaViewModel)value).propostaId;
                Proposta p = propModel.Find(prop);
                prop = propModel.MapToRepository(p);
            }
            catch (Exception ex)
            {
                prop.mensagem = new Validate() { Code = 999, MessageBase = ex.Message, Message = "Ocorreu um erro na consulta da proposta" };
            }
            return prop;
        }

        public IEnumerable<PropostaViewModel> List(params object[] param)
        {
            throw new NotImplementedException();
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {
            throw new NotImplementedException();
        }

    }
}