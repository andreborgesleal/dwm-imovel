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
    public class EsteiraContabilizacaoDeleteBI : EsteiraContabilizacaoBI //DWMContext<ApplicationContext>, IProcess<EsteiraContabilizacaoViewModel, ApplicationContext>
    {
        #region Constructor
        public EsteiraContabilizacaoDeleteBI() { }

        public EsteiraContabilizacaoDeleteBI(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        public override EsteiraContabilizacaoViewModel Run(Repository value)
        {
            EsteiraContabilizacaoViewModel r = (EsteiraContabilizacaoViewModel)value;
            try
            {
                EsteiraContabilizacaoModel model = new EsteiraContabilizacaoModel(this.db, this.seguranca_db);
                r = model.Delete(r);
            }
            catch (Exception ex)
            {
                r.mensagem = new Validate() { Code = 999, MessageBase = ex.Message, Message = "Ocorreu um erro na exclusão do arquivo" };
            }
            return r;
        }
    }
}