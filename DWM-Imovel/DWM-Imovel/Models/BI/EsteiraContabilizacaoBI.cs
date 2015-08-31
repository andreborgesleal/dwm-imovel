using System;
using System.Collections.Generic;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Component;
using DWM.Models.Repositories;
using DWM.Models.Entidades;
using DWM.Models.Persistence;
using System.Web.Mvc;

namespace DWM.Models.BI
{
    public class EsteiraContabilizacaoBI : DWMContext<ApplicationContext>, IProcess<EsteiraContabilizacaoViewModel, ApplicationContext>
    {
        #region Constructor
        public EsteiraContabilizacaoBI() { }

        public EsteiraContabilizacaoBI(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        public virtual EsteiraContabilizacaoViewModel Run(Repository value)
        {
            EsteiraContabilizacaoViewModel r = (EsteiraContabilizacaoViewModel)value;
            try
            {
                EsteiraContabilizacaoModel model = new EsteiraContabilizacaoModel(this.db, this.seguranca_db);
                r = model.Insert(r);
            }
            catch (Exception ex)
            {
                r.mensagem = new Validate() { Code = 999, MessageBase = ex.Message, Message = "Ocorreu um erro na inclusão do arquivo" };
            }
            return r;
        }

        public IEnumerable<EsteiraContabilizacaoViewModel> List(params object[] param)
        {
            ListViewEsteiraContabilizacao list = new ListViewEsteiraContabilizacao(this.db, this.seguranca_db);
            return list.Bind(0, 100, param);
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {
            ListViewEsteiraContabilizacao list = new ListViewEsteiraContabilizacao(this.db, this.seguranca_db);
            return list.getPagedList(index, pageSize, param);
        }
    }
}