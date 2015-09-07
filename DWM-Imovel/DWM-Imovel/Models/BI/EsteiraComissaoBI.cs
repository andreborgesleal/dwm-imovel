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
    public class EsteiraComissaoBI : DWMContext<ApplicationContext>, IProcess<EsteiraComissaoViewModel, ApplicationContext>
    {
        #region Constructor
        public EsteiraComissaoBI() { }

        public EsteiraComissaoBI(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        public virtual EsteiraComissaoViewModel Run(Repository value)
        {
            EsteiraComissaoViewModel r = (EsteiraComissaoViewModel)value;
            try
            {
                EsteiraComissaoModel model = new EsteiraComissaoModel(this.db, this.seguranca_db);
                EsteiraComissaoViewModel esteiraComissaoViewModel = model.getObject((EsteiraComissaoViewModel)value);

                esteiraComissaoViewModel.valor = ((EsteiraComissaoViewModel)value).valor;
                esteiraComissaoViewModel.uri = ((EsteiraComissaoViewModel)value).uri;

                esteiraComissaoViewModel = model.Update(esteiraComissaoViewModel);
                if (esteiraComissaoViewModel.mensagem.Code > 0)
                {
                    r = esteiraComissaoViewModel;
                    throw new Exception(esteiraComissaoViewModel.mensagem.MessageBase);
                }
                else
                    r.mensagem = new Validate() { Code = 0, Message = "Registro alterado com sucesso", MessageBase = "" };
            }
            catch (Exception ex)
            {
                r.mensagem = new Validate() { Code = 999, MessageBase = ex.Message, Message = "Ocorreu um erro na edição da comissão" };
            }
            return r;
        }

        public IEnumerable<EsteiraComissaoViewModel> List(params object[] param)
        {
            ListViewComissao list = new ListViewComissao(this.db, this.seguranca_db);
            return list.Bind(0, 100, param);
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {
            ListViewEsteiraComissao list = new ListViewEsteiraComissao(this.db, this.seguranca_db);
            return list.getPagedList(index, pageSize, param);
        }

    }
}